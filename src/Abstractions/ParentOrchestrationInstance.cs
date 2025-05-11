// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DurableTask;

/// <summary>
/// Represents a parent orchestration details.
/// </summary>
public record ParentOrchestrationInstance
{
    /// <summary>
    /// Represents a parent orchestration details.
    /// </summary>
    /// <param name="Name">The name of the parent orchestration.</param>
    /// <param name="InstanceId">The instance ID of the parent orchestration.</param>
    public ParentOrchestrationInstance(TaskName Name, string InstanceId)
    {
        this.Name = Name;
        this.InstanceId = InstanceId;
    }

    /// <summary>The name of the parent orchestration.</summary>
    public TaskName Name { get; init; }

    /// <summary>The instance ID of the parent orchestration.</summary>
    public string InstanceId { get; init; }

    public void Deconstruct(out TaskName Name, out string InstanceId)
    {
        Name = this.Name;
        InstanceId = this.InstanceId;
    }
}
