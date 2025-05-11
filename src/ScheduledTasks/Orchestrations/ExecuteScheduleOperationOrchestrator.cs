// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.DurableTask.Entities;

namespace Microsoft.DurableTask.ScheduledTasks;

// TODO: logging
// TODO: May need separate orchs, result is obj now

/// <summary>
/// Orchestrator that executes operations on schedule entities.
/// Calls the specified operation on the target entity and returns the result.
/// </summary>
[DurableTask]
public class ExecuteScheduleOperationOrchestrator : TaskOrchestrator<ScheduleOperationRequest, object>
{
    /// <inheritdoc/>
    public override async Task<object> RunAsync(TaskOrchestrationContext context, ScheduleOperationRequest input)
    {
        return await context.Entities.CallEntityAsync<object>(input.EntityId, input.OperationName, input.Input);
    }
}

/// <summary>
/// Request for executing a schedule operation.
/// </summary>
public record ScheduleOperationRequest
{
    /// <summary>
    /// Request for executing a schedule operation.
    /// </summary>
    /// <param name="EntityId">The ID of the entity to execute the operation on.</param>
    /// <param name="OperationName">The name of the operation to execute.</param>
    /// <param name="Input">Optional input for the operation.</param>
    public ScheduleOperationRequest(EntityInstanceId EntityId, string OperationName, object? Input = null)
    {
        this.EntityId = EntityId;
        this.OperationName = OperationName;
        this.Input = Input;
    }

    /// <summary>The ID of the entity to execute the operation on.</summary>
    public EntityInstanceId EntityId { get; init; }

    /// <summary>The name of the operation to execute.</summary>
    public string OperationName { get; init; }

    /// <summary>Optional input for the operation.</summary>
    public object? Input { get; init; }

    public void Deconstruct(out EntityInstanceId EntityId, out string OperationName, out object? Input)
    {
        EntityId = this.EntityId;
        OperationName = this.OperationName;
        Input = this.Input;
    }
}
