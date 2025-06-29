// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dapr.DurableTask.Worker.Grpc.Tests;

public class GrpcDurableTaskWorkerTests
{
    [Fact]
    public async Task ExecuteAsync_CancellationRequested_ExitsCleanly()
    {
        // Arrange
        var mockFactory = new Mock<IDurableTaskFactory>();
        var mockGrpcOptions = new Mock<IOptionsMonitor<GrpcDurableTaskWorkerOptions>>();
        var mockWorkerOptions = new Mock<IOptionsMonitor<DurableTaskWorkerOptions>>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockLogger = new Mock<ILogger>();

        mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);
        mockGrpcOptions.Setup(o => o.Get(It.IsAny<string>())).Returns(new GrpcDurableTaskWorkerOptions());
        mockWorkerOptions.Setup(o => o.Get(It.IsAny<string>())).Returns(new DurableTaskWorkerOptions());

        var cancellationTokenSource = new CancellationTokenSource();

        // Create a test worker with the mocked dependencies
        var worker = new GrpcDurableTaskWorker(
            "TestWorker",
            mockFactory.Object,
            mockGrpcOptions.Object,
            mockWorkerOptions.Object,
            mockServiceProvider.Object,
            mockLoggerFactory.Object);

        // Act
        // Start the worker
        var workerTask = worker.StartAsync(cancellationTokenSource.Token);

        // Immediately request cancellation
        await cancellationTokenSource.CancelAsync();

        // Wait for the worker to exit (with a timeout to prevent test hangs)
        await Task.WhenAny(workerTask, Task.Delay(5000, cancellationTokenSource.Token));

        // Assert
        // The worker should have exited cleanly without throwing
        workerTask.IsCompleted.Should().BeTrue("worker should exit after cancellation");
        await TestExtensions.Invoking(() => workerTask).Should().NotThrowAsync();
    }

    [Fact]
    public void CreateCallOptions_HasNoDeadline()
    {
        // Arrange
        var mockFactory = new Mock<IDurableTaskFactory>();
        var mockGrpcOptions = new Mock<IOptionsMonitor<GrpcDurableTaskWorkerOptions>>();
        var mockWorkerOptions = new Mock<IOptionsMonitor<DurableTaskWorkerOptions>>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockLogger = new Mock<ILogger>();

        mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);
        mockGrpcOptions.Setup(o => o.Get(It.IsAny<string>())).Returns(new GrpcDurableTaskWorkerOptions());
        mockWorkerOptions.Setup(o => o.Get(It.IsAny<string>())).Returns(new DurableTaskWorkerOptions());

        var worker = new GrpcDurableTaskWorker(
            "TestWorker",
            mockFactory.Object,
            mockGrpcOptions.Object,
            mockWorkerOptions.Object,
            mockServiceProvider.Object,
            mockLoggerFactory.Object);

        // Act
        var options = worker.CreateCallOptions(CancellationToken.None);

        // Assert
        // The CallOptions should have a null deadline
        options.Deadline.Should().BeNull("Deadline should be null to allow unlimited connection time");
    }
}
