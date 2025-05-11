// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.DurableTask.Analyzers.Functions.AttributeBinding;

namespace Microsoft.DurableTask.Analyzers.Tests.Functions.AttributeBinding;

public class DurableClientBindingAnalyzerTests : MatchingAttributeBindingSpecificationTests<DurableClientBindingAnalyzer, DurableClientBindingFixer>
{
    protected override string ExpectedDiagnosticId
    {
        get
        {
            return DurableClientBindingAnalyzer.DiagnosticId;
        }
    }

    protected override string ExpectedAttribute
    {
        get
        {
            return "[DurableClient]";
        }
    }

    protected override string ExpectedType
    {
        get
        {
            return "DurableTaskClient";
        }
    }
}
