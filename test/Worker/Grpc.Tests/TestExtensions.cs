// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Dapr.DurableTask.Worker.Grpc.Tests;

public static class TestExtensions
{
    /// <summary>
    /// Helper method for fluent assertions with async methods.
    /// </summary>
    public static Func<Task> Invoking(Func<Task> action) => action;
}
