// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using DurableTask.Core.Entities;
using Dapr.DurableTask.Entities;
using Dapr.DurableTask.Worker.Shims;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dapr.DurableTask.Worker.Tests.Shims;

public class DurableTaskShimFactoryTests
{
    [Fact]
    public void Constructor_WithNullParameters_UsesDefaultValues()
    {
        // Act
        var factory = new DurableTaskShimFactory(null, null);

        // Assert - No exception means success
        Assert.NotNull(factory);
    }

    [Fact]
    public void Default_Property_ReturnsNonNullInstance()
    {
        // Act
        var factory = DurableTaskShimFactory.Default;

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public void CreateActivity_WithValidParameters_ReturnsTaskActivity()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var factory = new DurableTaskShimFactory(options, loggerFactory);
        var taskName = new TaskName("TestActivity");
        var mockActivity = new Mock<ITaskActivity>();

        // Act
        var result = factory.CreateActivity(taskName, mockActivity.Object);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TaskActivityShim>(result);
    }

    [Fact]
    public void CreateActivity_WithDefaultName_ThrowsArgumentException()
    {
        // Arrange
        var factory = DurableTaskShimFactory.Default;
        var taskName = default(TaskName);
        var mockActivity = new Mock<ITaskActivity>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateActivity(taskName, mockActivity.Object));
    }

    [Fact]
    public void CreateActivity_WithNullActivity_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = DurableTaskShimFactory.Default;
        var taskName = new TaskName("TestActivity");
        ITaskActivity activity = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateActivity(taskName, activity));
    }

    [Fact]
    public void CreateActivity_WithGenericDelegate_ReturnsTaskActivity()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var factory = new DurableTaskShimFactory(options, loggerFactory);
        var taskName = new TaskName("TestActivity");
        Func<TaskActivityContext, string?, Task<string?>> implementation = (ctx, input) => Task.FromResult(input);

        // Act
        var result = factory.CreateActivity<string, string>(taskName, implementation);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TaskActivityShim>(result);
    }

    [Fact]
    public void CreateOrchestration_WithValidParameters_ReturnsTaskOrchestration()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var factory = new DurableTaskShimFactory(options, loggerFactory);
        var taskName = new TaskName("TestOrchestration");
        var mockOrchestrator = new Mock<ITaskOrchestrator>();

        // Act
        var result = factory.CreateOrchestration(taskName, mockOrchestrator.Object);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TaskOrchestrationShim>(result);
    }

    [Fact]
    public void CreateOrchestration_WithProperties_ReturnsTaskOrchestration()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var factory = new DurableTaskShimFactory(options, loggerFactory);
        var taskName = new TaskName("TestOrchestration");
        var mockOrchestrator = new Mock<ITaskOrchestrator>();
        var properties = new Dictionary<string, object?> { { "key", "value" } };

        // Act
        var result = factory.CreateOrchestration(taskName, mockOrchestrator.Object, properties);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TaskOrchestrationShim>(result);
    }

    [Fact]
    public void CreateOrchestration_WithNullProperties_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = DurableTaskShimFactory.Default;
        var taskName = new TaskName("TestOrchestration");
        var mockOrchestrator = new Mock<ITaskOrchestrator>();
        IReadOnlyDictionary<string, object?> properties = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateOrchestration(taskName, mockOrchestrator.Object, properties));
    }

    [Fact]
    public void CreateOrchestration_WithGenericDelegate_ReturnsTaskOrchestration()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var factory = new DurableTaskShimFactory(options, loggerFactory);
        var taskName = new TaskName("TestOrchestration");
        Func<TaskOrchestrationContext, string?, Task<string?>> implementation = (ctx, input) => Task.FromResult(input);

        // Act
        var result = factory.CreateOrchestration<string, string>(taskName, implementation);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TaskOrchestrationShim>(result);
    }

    [Fact]
    public void CreateEntity_WithValidParameters_ReturnsTaskEntity()
    {
        // Arrange
        var options = new DurableTaskWorkerOptions();
        var loggerFactory = new NullLoggerFactory();
        var factory = new DurableTaskShimFactory(options, loggerFactory);
        var taskName = new TaskName("TestEntity");
        var mockEntity = new Mock<ITaskEntity>();
        var entityId = new EntityId("TestEntityType", "TestEntityKey");

        // Act
        var result = factory.CreateEntity(taskName, mockEntity.Object, entityId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TaskEntityShim>(result);
    }

    [Fact]
    public void CreateEntity_WithNullEntity_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = DurableTaskShimFactory.Default;
        var taskName = new TaskName("TestEntity");
        ITaskEntity entity = null!;
        var entityId = new EntityId("TestEntityType", "TestEntityKey");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateEntity(taskName, entity, entityId));
    }
}
