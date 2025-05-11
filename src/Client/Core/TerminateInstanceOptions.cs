// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DurableTask.Client;

/// <summary>
///  Options to terminate an orchestration.
/// </summary>
public record TerminateInstanceOptions
{
    /// <summary>
    ///  Options to terminate an orchestration.
    /// </summary>
    /// <param name="Output">The optional output to set for the terminated orchestration instance.</param>
    /// <param name="Recursive">The optional boolean value indicating whether to terminate sub-orchestrations as well.</param>
    public TerminateInstanceOptions(object? Output = null, bool Recursive = false)
    {
        this.Output = Output;
        this.Recursive = Recursive;
    }

    /// <summary>The optional output to set for the terminated orchestration instance.</summary>
    public object? Output { get; init; }

    /// <summary>The optional boolean value indicating whether to terminate sub-orchestrations as well.</summary>
    public bool Recursive { get; init; }

    public void Deconstruct(out object? Output, out bool Recursive)
    {
        Output = this.Output;
        Recursive = this.Recursive;
    }
}
