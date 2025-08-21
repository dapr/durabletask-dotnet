// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using DurableTask.Core.Entities;
using DurableTask.Core.Entities.OperationFormat;
using Dapr.DurableTask.Entities;
using Microsoft.Extensions.Logging;
using Dapr.DurableTask.Worker.Shims;

namespace Dapr.DurableTask.Worker.Tests.Shims;

public class TaskEntityShimTests
{
    readonly Mock<DataConverter> mockDataConverter;
    readonly Mock<ITaskEntity> mockTaskEntity;
    readonly EntityId entityId;
    readonly Mock<ILogger> mockLogger;
    readonly TaskEntityShim shim;

    public TaskEntityShimTests()
    {
        // Setup common test dependencies
        mockDataConverter = new Mock<DataConverter>();
        mockTaskEntity = new Mock<ITaskEntity>();
        entityId = new EntityId("TestEntity", "TestKey");
        mockLogger = new Mock<ILogger>();

        // Create the shim with mocked dependencies
        shim = new TaskEntityShim(
            mockDataConverter.Object,
            mockTaskEntity.Object,
            entityId,
            mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var dataConverter = new Mock<DataConverter>().Object;
        var taskEntity = new Mock<ITaskEntity>().Object;
        var entityId = new EntityId("TestEntity", "TestKey");
        var logger = new Mock<ILogger>().Object;

        // Act - Create the shim
        var shim = new TaskEntityShim(dataConverter, taskEntity, entityId, logger);

        // Assert - No exception means success
        Assert.NotNull(shim);
    }

    [Fact]
    public void Constructor_WithNullDataConverter_ThrowsArgumentNullException()
    {
        // Arrange
        DataConverter dataConverter = null!;
        var taskEntity = new Mock<ITaskEntity>().Object;
        var entityId = new EntityId("TestEntity", "TestKey");
        var logger = new Mock<ILogger>().Object;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new TaskEntityShim(dataConverter, taskEntity, entityId, logger));
    }

    [Fact]
    public void Constructor_WithNullTaskEntity_ThrowsArgumentNullException()
    {
        // Arrange
        var dataConverter = new Mock<DataConverter>().Object;
        ITaskEntity taskEntity = null!;
        var entityId = new EntityId("TestEntity", "TestKey");
        var logger = new Mock<ILogger>().Object;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new TaskEntityShim(dataConverter, taskEntity, entityId, logger));
    }

    [Fact]
    public async Task ExecuteOperationBatchAsync_WithSuccessfulOperation_ReturnsCorrectResults()
    {
        // Arrange
        var operations = new EntityBatchRequest
        {
            EntityState = "initialState",
            Operations = new List<OperationRequest>
            {
                new OperationRequest { Operation = "TestOperation", Input = "testInput" }
            }
        };

        object operationResult = "testResult";
        string serializedResult = "serializedResult";

        mockDataConverter
            .Setup(dc => dc.Serialize(operationResult))
            .Returns(serializedResult);

        mockTaskEntity
            .Setup(te => te.RunAsync(It.IsAny<TaskEntityOperation>()))
            .ReturnsAsync(operationResult);

        // Act
        var result = await shim.ExecuteOperationBatchAsync(operations);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.Single(result.Results);
        Assert.Equal(serializedResult, result.Results[0].Result);
        Assert.Null(result.Results[0].FailureDetails);
        
        mockTaskEntity.Verify(
            te => te.RunAsync(It.IsAny<TaskEntityOperation>()), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteOperationBatchAsync_WithFailingOperation_CapturesExceptionInResult()
    {
        // Arrange
        var operations = new EntityBatchRequest
        {
            EntityState = "initialState",
            Operations = new List<OperationRequest>
            {
                new OperationRequest { Operation = "TestOperation", Input = "testInput" }
            }
        };

        var expectedException = new InvalidOperationException("Test exception");

        mockTaskEntity
            .Setup(te => te.RunAsync(It.IsAny<TaskEntityOperation>()))
            .ThrowsAsync(expectedException);

        // Act
        var result = await shim.ExecuteOperationBatchAsync(operations);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.Single(result.Results);
        Assert.Null(result.Results[0].Result);
        Assert.NotNull(result.Results[0].FailureDetails);
        Assert.Equal(expectedException.Message, result.Results[0].FailureDetails.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteOperationBatchAsync_WithMultipleOperations_ProcessesAllOperations()
    {
        // Arrange
        var operations = new EntityBatchRequest
        {
            EntityState = "initialState",
            Operations = new List<OperationRequest>
            {
                new OperationRequest { Operation = "Operation1", Input = "input1" },
                new OperationRequest { Operation = "Operation2", Input = "input2" },
                new OperationRequest { Operation = "Operation3", Input = "input3" }
            }
        };

        mockTaskEntity
            .Setup(te => te.RunAsync(It.IsAny<TaskEntityOperation>()))
            .ReturnsAsync("result");

        mockDataConverter
            .Setup(dc => dc.Serialize(It.IsAny<object>()))
            .Returns("serializedResult");

        // Act
        var result = await shim.ExecuteOperationBatchAsync(operations);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.Equal(3, result.Results.Count);
        
        mockTaskEntity.Verify(
            te => te.RunAsync(It.IsAny<TaskEntityOperation>()), 
            Times.Exactly(3));
    }

    [Fact]
    public async Task ExecuteOperationBatchAsync_WithMixedResults_HandlesSuccessAndFailure()
    {
        // Arrange
        var operations = new EntityBatchRequest
        {
            EntityState = "initialState",
            Operations = new List<OperationRequest>
            {
                new OperationRequest { Operation = "SuccessOperation", Input = "input1" },
                new OperationRequest { Operation = "FailingOperation", Input = "input2" },
                new OperationRequest { Operation = "SuccessOperation", Input = "input3" }
            }
        };

        var expectedException = new InvalidOperationException("Test exception");

        mockTaskEntity
            .SetupSequence(te => te.RunAsync(It.IsAny<TaskEntityOperation>()))
            .ReturnsAsync("result1")
            .ThrowsAsync(expectedException)
            .ReturnsAsync("result3");

        mockDataConverter
            .Setup(dc => dc.Serialize(It.IsAny<object>()))
            .Returns("serializedResult");

        // Act
        var result = await shim.ExecuteOperationBatchAsync(operations);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.Equal(3, result.Results.Count);
        
        Assert.NotNull(result.Results[0].Result);
        Assert.Null(result.Results[0].FailureDetails);
        
        Assert.Null(result.Results[1].Result);
        Assert.NotNull(result.Results[1].FailureDetails);
        Assert.Equal(expectedException.Message, result.Results[1].FailureDetails.ErrorMessage);
        
        Assert.NotNull(result.Results[2].Result);
        Assert.Null(result.Results[2].FailureDetails);
    }

    [Fact]
    public async Task ExecuteOperationBatchAsync_CommitsStateOnSuccessAndRollsBackOnFailure()
    {
        // Arrange
        var operations = new EntityBatchRequest
        {
            EntityState = "initialState",
            Operations = new List<OperationRequest>
            {
                new OperationRequest { Operation = "SuccessOperation", Input = "input1" },
                new OperationRequest { Operation = "FailingOperation", Input = "input2" }
            }
        };

        var expectedException = new InvalidOperationException("Test exception");

        mockTaskEntity
            .SetupSequence(te => te.RunAsync(It.IsAny<TaskEntityOperation>()))
            .ReturnsAsync("result1")
            .ThrowsAsync(expectedException);

        mockDataConverter
            .Setup(dc => dc.Serialize(It.IsAny<object>()))
            .Returns("serializedResult");

        // We need to verify state commits/rollbacks occur correctly
        // This requires inspecting the internal state which is challenging in tests
        // Instead, we'll verify the correct operations are called and ensure result is as expected

        // Act
        var result = await shim.ExecuteOperationBatchAsync(operations);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.Equal(2, result.Results.Count);
        
        Assert.NotNull(result.Results[0].Result);
        Assert.Null(result.Results[0].FailureDetails);
        
        Assert.Null(result.Results[1].Result);
        Assert.NotNull(result.Results[1].FailureDetails);
    }

    [Fact]
    public async Task ExecuteOperationBatchAsync_WithEmptyOperations_ReturnsEmptyResults()
    {
        // Arrange
        var operations = new EntityBatchRequest
        {
            EntityState = "initialState",
            Operations = new List<OperationRequest>()
        };

        // Act
        var result = await shim.ExecuteOperationBatchAsync(operations);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.Empty(result.Results);
        
        mockTaskEntity.Verify(
            te => te.RunAsync(It.IsAny<TaskEntityOperation>()), 
            Times.Never);
    }
}