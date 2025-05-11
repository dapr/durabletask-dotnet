// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DurableTask.Client;

/// <summary>
/// Options to purge an orchestration.
/// </summary>
public record PurgeInstanceOptions
{
    /// <summary>
    /// Options to purge an orchestration.
    /// </summary>
    /// <param name="Recursive">The optional boolean value indicating whether to purge sub-orchestrations as well.</param>
    public PurgeInstanceOptions(bool Recursive = false)
    {
        this.Recursive = Recursive;
    }

    /// <summary>The optional boolean value indicating whether to purge sub-orchestrations as well.</summary>
    public bool Recursive { get; init; }

    public void Deconstruct(out bool Recursive)
    {
        Recursive = this.Recursive;
    }
}
