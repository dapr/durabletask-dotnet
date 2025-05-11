// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace Microsoft.DurableTask.Worker.Shims;

/// <summary>
/// Initializes a new instance of the <see cref="OrchestrationInvocationContext"/> class.
/// </summary>
record OrchestrationInvocationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrchestrationInvocationContext"/> class.
    /// </summary>
    /// <param name="Name">The invoked orchestration name.</param>
    /// <param name="Options">The Durable Task worker options.</param>
    /// <param name="LoggerFactory">The logger factory for this orchestration.</param>
    /// <param name="Parent">The orchestration parent details.</param>
    public OrchestrationInvocationContext(TaskName Name,
        DurableTaskWorkerOptions Options,
        ILoggerFactory LoggerFactory,
        ParentOrchestrationInstance? Parent = null)
    {
        this.Name = Name;
        this.Options = Options;
        this.LoggerFactory = LoggerFactory;
        this.Parent = Parent;
    }

    /// <summary>The invoked orchestration name.</summary>
    public TaskName Name { get; init; }

    /// <summary>The Durable Task worker options.</summary>
    public DurableTaskWorkerOptions Options { get; init; }

    /// <summary>The logger factory for this orchestration.</summary>
    public ILoggerFactory LoggerFactory { get; init; }

    /// <summary>The orchestration parent details.</summary>
    public ParentOrchestrationInstance? Parent { get; init; }

    public void Deconstruct(
        out TaskName Name,
        out DurableTaskWorkerOptions Options,
        out ILoggerFactory LoggerFactory,
        out ParentOrchestrationInstance? Parent)
    {
        Name = this.Name;
        Options = this.Options;
        LoggerFactory = this.LoggerFactory;
        Parent = this.Parent;
    }
}
