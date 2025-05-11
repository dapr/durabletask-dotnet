// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.DurableTask.Analyzers.Functions.AttributeBinding;

namespace Microsoft.DurableTask.Analyzers.Tests.Functions.AttributeBinding;

public class EntityTriggerBindingAnalyzerTests : MatchingAttributeBindingSpecificationTests<EntityTriggerBindingAnalyzer, EntityTriggerBindingFixer>
{
    protected override string ExpectedDiagnosticId
    {
        get
        {
            return EntityTriggerBindingAnalyzer.DiagnosticId;
        }
    }

    protected override string ExpectedAttribute
    {
        get
        {
            return "[EntityTrigger]";
        }
    }

    protected override string ExpectedType
    {
        get
        {
            return "TaskEntityDispatcher";
        }
    }
}
