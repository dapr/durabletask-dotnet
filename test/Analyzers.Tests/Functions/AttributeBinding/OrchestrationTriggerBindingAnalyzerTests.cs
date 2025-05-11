// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.DurableTask.Analyzers.Functions.AttributeBinding;

namespace Microsoft.DurableTask.Analyzers.Tests.Functions.AttributeBinding;

public class OrchestrationTriggerBindingAnalyzerTests : MatchingAttributeBindingSpecificationTests<OrchestrationTriggerBindingAnalyzer, OrchestrationTriggerBindingFixer>
{
    protected override string ExpectedDiagnosticId
    {
        get
        {
            return OrchestrationTriggerBindingAnalyzer.DiagnosticId;
        }
    }

    protected override string ExpectedAttribute
    {
        get
        {
            return "[OrchestrationTrigger]";
        }
    }

    protected override string ExpectedType
    {
        get
        {
            return "TaskOrchestrationContext";
        }
    }
}
