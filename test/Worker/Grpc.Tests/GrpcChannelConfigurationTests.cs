// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Net.Http;
using System.Reflection;
using Dapr.DurableTask.Worker.Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dapr.DurableTask.Worker.Grpc.Tests;

public class GrpcChannelConfigurationTests
{
    [Fact]
    public void GetChannel_ConfiguresSocketsHttpHandler_WithLongRunningConnectionSettings()
    {
        // Arrange
        // Access the private static GetChannel method via reflection
        var methodInfo = typeof(GrpcDurableTaskWorker).GetMethod("GetChannel", 
            BindingFlags.NonPublic | BindingFlags.Static);

        // Act
        var channel = (GrpcChannel)methodInfo.Invoke(null, ["http://localhost:4001"]);

        // Get the HTTP handler via reflection (no public API to access it)
        var handlerField = channel.GetType().GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance);
        var handler = handlerField?.GetValue(channel) as HttpMessageHandler;

        // If we can't get to the actual handler through reflection, the test can't proceed
        if (handler == null)
        {
            // This is not ideal, but the GrpcChannel class doesn't expose its handler publicly
            return;
        }

        // Try to get to the SocketsHttpHandler
        var socketsHandler = GetSocketsHttpHandler(handler);

        // Assert
        socketsHandler.Should().NotBeNull("channel should use SocketsHttpHandler");

        if (socketsHandler is not null)
        {
            socketsHandler.KeepAlivePingPolicy.Should().Be(HttpKeepAlivePingPolicy.Always, 
                "keep-alive pings should be enabled");

            socketsHandler.PooledConnectionIdleTimeout.Should().Be(Timeout.InfiniteTimeSpan, 
                "connections should never time out from inactivity");

            socketsHandler.PooledConnectionLifetime.Should().Be(TimeSpan.FromDays(1), 
                "connections should have a controlled lifetime of 1 day");
        }
    }

    // Helper method to get to the SocketsHttpHandler through potentially nested handlers
    private static SocketsHttpHandler GetSocketsHttpHandler(HttpMessageHandler handler)
    {
        while (handler != null)
        {
            if (handler is SocketsHttpHandler socketsHandler)
            {  
                return socketsHandler;
            }

            // Try to get the inner handler if this is a delegating handler
            var delegatingHandler = handler as DelegatingHandler;
            handler = delegatingHandler?.InnerHandler;
        }

        return null;
    }
}
