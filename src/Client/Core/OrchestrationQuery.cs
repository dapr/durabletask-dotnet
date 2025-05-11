// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DurableTask.Client;

/// <summary>
/// A filter for querying orchestration instances.
/// </summary>
public record OrchestrationQuery
{
    /// <summary>
    /// A filter for querying orchestration instances.
    /// </summary>
    /// <param name="CreatedFrom">Creation date of instances to query from.</param>
    /// <param name="CreatedTo">Creation date of instances to query to.</param>
    /// <param name="Statuses">Runtime statuses of instances to query.</param>
    /// <param name="TaskHubNames">Names of task hubs to query across.</param>
    /// <param name="InstanceIdPrefix">Prefix of instance IDs to include.</param>
    /// <param name="PageSize">Max item count to include per page.</param>
    /// <param name="FetchInputsAndOutputs">Whether to include instance inputs or outputs in the query results.</param>
    /// <param name="ContinuationToken">The continuation token to continue a paged query.</param>
    public OrchestrationQuery(DateTimeOffset? CreatedFrom = null,
        DateTimeOffset? CreatedTo = null,
        IEnumerable<OrchestrationRuntimeStatus>? Statuses = null,
        IEnumerable<string>? TaskHubNames = null,
        string? InstanceIdPrefix = null,
        int PageSize = OrchestrationQuery.DefaultPageSize,
        bool FetchInputsAndOutputs = false,
        string? ContinuationToken = null)
    {
        this.CreatedFrom = CreatedFrom;
        this.CreatedTo = CreatedTo;
        this.Statuses = Statuses;
        this.TaskHubNames = TaskHubNames;
        this.InstanceIdPrefix = InstanceIdPrefix;
        this.PageSize = PageSize;
        this.FetchInputsAndOutputs = FetchInputsAndOutputs;
        this.ContinuationToken = ContinuationToken;
    }

    /// <summary>
    /// The default page size when not supplied.
    /// </summary>
    public const int DefaultPageSize = 100;

    /// <summary>Creation date of instances to query from.</summary>
    public DateTimeOffset? CreatedFrom { get; init; }

    /// <summary>Creation date of instances to query to.</summary>
    public DateTimeOffset? CreatedTo { get; init; }

    /// <summary>Runtime statuses of instances to query.</summary>
    public IEnumerable<OrchestrationRuntimeStatus>? Statuses { get; init; }

    /// <summary>Names of task hubs to query across.</summary>
    public IEnumerable<string>? TaskHubNames { get; init; }

    /// <summary>Prefix of instance IDs to include.</summary>
    public string? InstanceIdPrefix { get; init; }

    /// <summary>Max item count to include per page.</summary>
    public int PageSize { get; init; }

    /// <summary>Whether to include instance inputs or outputs in the query results.</summary>
    public bool FetchInputsAndOutputs { get; init; }

    /// <summary>The continuation token to continue a paged query.</summary>
    public string? ContinuationToken { get; init; }

    public void Deconstruct(
        out DateTimeOffset? CreatedFrom,
        out DateTimeOffset? CreatedTo,
        out IEnumerable<OrchestrationRuntimeStatus>? Statuses,
        out IEnumerable<string>? TaskHubNames,
        out string? InstanceIdPrefix,
        out int PageSize,
        out bool FetchInputsAndOutputs,
        out string? ContinuationToken)
    {
        CreatedFrom = this.CreatedFrom;
        CreatedTo = this.CreatedTo;
        Statuses = this.Statuses;
        TaskHubNames = this.TaskHubNames;
        InstanceIdPrefix = this.InstanceIdPrefix;
        PageSize = this.PageSize;
        FetchInputsAndOutputs = this.FetchInputsAndOutputs;
        ContinuationToken = this.ContinuationToken;
    }
}
