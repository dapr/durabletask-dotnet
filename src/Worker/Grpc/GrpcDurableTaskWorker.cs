// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics;
using Dapr.DurableTask.Worker.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dapr.DurableTask.Worker.Grpc;

/// <summary>
/// The gRPC Durable Task worker.
/// </summary>
partial class GrpcDurableTaskWorker : DurableTaskWorker
{
    readonly GrpcDurableTaskWorkerOptions grpcOptions;
    readonly DurableTaskWorkerOptions workerOptions;
    readonly IServiceProvider services;
    readonly ILoggerFactory loggerFactory;
    readonly ILogger logger;
    int reconnectAttempts;

    /// <summary>
    /// Initializes a new instance of the <see cref="GrpcDurableTaskWorker" /> class.
    /// </summary>
    /// <param name="name">The name of the worker.</param>
    /// <param name="factory">The task factory.</param>
    /// <param name="grpcOptions">The gRPC-specific worker options.</param>
    /// <param name="workerOptions">The generic worker options.</param>
    /// <param name="services">The service provider.</param>
    /// <param name="loggerFactory">The logger.</param>
    public GrpcDurableTaskWorker(
        string name,
        IDurableTaskFactory factory,
        IOptionsMonitor<GrpcDurableTaskWorkerOptions> grpcOptions,
        IOptionsMonitor<DurableTaskWorkerOptions> workerOptions,
        IServiceProvider services,
        ILoggerFactory loggerFactory)
        : base(name, factory)
    {
        this.grpcOptions = Check.NotNull(grpcOptions).Get(name);
        this.workerOptions = Check.NotNull(workerOptions).Get(name);
        this.services = Check.NotNull(services);
        this.loggerFactory = Check.NotNull(loggerFactory);
        this.logger = loggerFactory.CreateLogger("Dapr.DurableTask");
    }

    /// <summary>
    /// Creates call options with appropriate settings for long-running connections.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>CallOptions configured for long-running connections.</returns>
    internal CallOptions CreateCallOptions(CancellationToken cancellationToken)
    {
        // Create call options with NO deadline to ensure unlimited connection time
        // This aligns with our channel settings for long-running connections
        var options = new CallOptions(cancellationToken: cancellationToken);

        // By not setting a Deadline property, we ensure the connection can
        // stay open indefinitely, which matches our channel settings
        this.logger.ConfiguringGrpcCallOptions();

        return options;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Reset reconnect counter when we start a new attempt
                if (this.reconnectAttempts > 0)
                {
                    this.logger.StartingReconnectAttempt(this.reconnectAttempts);
                }

                await using AsyncDisposable disposable =
                    this.GetCallInvoker(out CallInvoker callInvoker, out string address);
                this.logger.StartingTaskHubWorker(address);

                var stopwatch = Stopwatch.StartNew();
                await new Processor(this, new(callInvoker)).ExecuteAsync(stoppingToken);
                stopwatch.Stop();

                this.logger.TaskHubWorkerExited(stopwatch.ElapsedMilliseconds);

                // If we got here without an exception, break out of the retry loop
                break;
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                this.reconnectAttempts++;

                // Log exception with detailed context
                this.logger.TaskHubWorkerError(this.reconnectAttempts, ex.GetType().Name, ex.Message, ex);

                // Add a brief delay before retrying to avoid tight CPU-bound loops
                await Task.Delay(
                    TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, Math.Min(this.reconnectAttempts, 5)))),
                    stoppingToken);
            }
            catch (Exception ex)
            {
                this.logger.UnexpectedError(ex, nameof(GrpcDurableTaskWorker));
                throw;
            }
        }

        this.logger.CancellationRequested($"Cancellation handled at {nameof(this.ExecuteAsync)} in {nameof(GrpcDurableTaskWorker)}");
    }

    static GrpcChannel GetChannel(string? address)
    {
        if (string.IsNullOrEmpty(address))
        {
            address = "http://localhost:4001";
        }

        // Create and configure the gRPC channel options for long-lived connections
        var channelOptions = new GrpcChannelOptions
        {
            // No message size limit
            MaxReceiveMessageSize = null,

            // Configure keep-alive settings to maintain long-lived connections
            HttpHandler = new SocketsHttpHandler
            {
                // Enable keep-alive
                KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always,
                KeepAlivePingDelay = TimeSpan.FromSeconds(30),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),

                // Pooled connections are reused and won't time out from inactivity
                EnableMultipleHttp2Connections = true,

                // Set a very long connection lifetime - this allows a controlled connection refresh strategy
                PooledConnectionLifetime = TimeSpan.FromDays(1),

                // Disable idle timeout entirely
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            },

            DisposeHttpClient = true,
        };

        return GrpcChannel.ForAddress(address, channelOptions);
    }

    AsyncDisposable GetCallInvoker(out CallInvoker callInvoker, out string address)
    {
        if (this.grpcOptions.Channel is { } c)
        {
            this.logger.GrpcChannelTarget(c.Target);
            callInvoker = c.CreateCallInvoker();
            address = c.Target;
            return default;
        }

        if (this.grpcOptions.CallInvoker is { } invoker)
        {
            this.logger.SelectGrpcCallInvoker();
            callInvoker = invoker;
            address = "(unspecified)";
            return default;
        }

        this.logger.CreatingGrpcChannelForAddress(this.grpcOptions.Address);
        c = GetChannel(this.grpcOptions.Address);
        callInvoker = c.CreateCallInvoker();
        address = c.Target;
        return new AsyncDisposable(() =>
        {
            this.logger.ShuttingDownGrpcChannel(c.Target);
            return new(c.ShutdownAsync());
        });
    }
}
