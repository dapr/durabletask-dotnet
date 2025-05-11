// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DurableTask.Client;

/// <summary>
/// Query for purging orchestration instances.
/// </summary>
public record PurgeInstancesFilter
{
    /// <summary>
    /// Query for purging orchestration instances.
    /// </summary>
    /// <param name="CreatedFrom">Date created from.</param>
    /// <param name="CreatedTo">Date created to.</param>
    /// <param name="Statuses">The statuses.</param>
    public PurgeInstancesFilter(DateTimeOffset? CreatedFrom = null,
        DateTimeOffset? CreatedTo = null,
        IEnumerable<OrchestrationRuntimeStatus>? Statuses = null)
    {
        this.CreatedFrom = CreatedFrom;
        this.CreatedTo = CreatedTo;
        this.Statuses = Statuses;
    }

    /// <summary>Date created from.</summary>
    public DateTimeOffset? CreatedFrom { get; init; }

    /// <summary>Date created to.</summary>
    public DateTimeOffset? CreatedTo { get; init; }

    /// <summary>The statuses.</summary>
    public IEnumerable<OrchestrationRuntimeStatus>? Statuses { get; init; }

    public void Deconstruct(
        out DateTimeOffset? CreatedFrom,
        out DateTimeOffset? CreatedTo,
        out IEnumerable<OrchestrationRuntimeStatus>? Statuses)
    {
        CreatedFrom = this.CreatedFrom;
        CreatedTo = this.CreatedTo;
        Statuses = this.Statuses;
    }
}
