// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using DurableTask.Core;
using Microsoft.Extensions.Logging.Abstractions;
using Dapr.DurableTask.Worker.Shims;

namespace Dapr.DurableTask.Worker.Tests.Shims;

public class TaskOrchestrationShimTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var context = new OrchestrationInvocationContext(
            new TaskName("TestOrchestration"),
            options,
            loggerFactory);
        var mockOrchestrator = new Mock<ITaskOrchestrator>();

        // Act - Create the shim
        var shim = new TaskOrchestrationShim(context, mockOrchestrator.Object);

        // Assert - No exception means success
        Assert.NotNull(shim);
    }

    [Fact]
    public void Constructor_WithProperties_InitializesCorrectly()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var context = new OrchestrationInvocationContext(
            new TaskName("TestOrchestration"),
            options,
            loggerFactory);
        var mockOrchestrator = new Mock<ITaskOrchestrator>();
        var properties = new Dictionary<string, object?> { { "key", "value" } };

        // Act - Create the shim with properties
        var shim = new TaskOrchestrationShim(context, mockOrchestrator.Object, properties);

        // Assert - No exception means success
        Assert.NotNull(shim);
    }

    [Fact]
    public void Constructor_WithNullInvocationContext_ThrowsArgumentNullException()
    {
        // Arrange
        OrchestrationInvocationContext context = null!;
        var mockOrchestrator = new Mock<ITaskOrchestrator>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TaskOrchestrationShim(context, mockOrchestrator.Object));
    }

    [Fact]
    public void Constructor_WithNullImplementation_ThrowsArgumentNullException()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var context = new OrchestrationInvocationContext(
            new TaskName("TestOrchestration"),
            options,
            loggerFactory);
        ITaskOrchestrator implementation = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TaskOrchestrationShim(context, implementation));
    }

    [Fact]
    public void Constructor_WithNullProperties_ThrowsArgumentNullException()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var context = new OrchestrationInvocationContext(
            new TaskName("TestOrchestration"),
            options,
            loggerFactory);
        var mockOrchestrator = new Mock<ITaskOrchestrator>();
        IReadOnlyDictionary<string, object?> properties = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new TaskOrchestrationShim(context, mockOrchestrator.Object, properties));
    }

    [Fact]
    public void GetStatus_ReturnsNull_WhenContextNotInitialized()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var context = new OrchestrationInvocationContext(
            new TaskName("TestOrchestration"),
            options,
            loggerFactory);
        var mockOrchestrator = new Mock<ITaskOrchestrator>();

        var shim = new TaskOrchestrationShim(context, mockOrchestrator.Object);

        // Act
        string? status = shim.GetStatus();

        // Assert
        Assert.Null(status);
    }
}