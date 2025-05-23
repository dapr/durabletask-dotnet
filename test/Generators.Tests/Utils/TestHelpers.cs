﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Dapr.DurableTask.Generators.Tests.Utils;

static class TestHelpers
{
    public static Task RunTestAsync<TSourceGenerator>(
        string expectedFileName,
        string inputSource,
        string expectedOutputSource,
        bool isDurableFunctions) where TSourceGenerator : ISourceGenerator, new()
    {
        CSharpSourceGeneratorVerifier<TSourceGenerator>.Test test = new()
        {
            TestState =
            {
                Sources = { inputSource },
                GeneratedSources =
                {
                    (typeof(TSourceGenerator), expectedFileName, SourceText.From(expectedOutputSource, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                },
                AdditionalReferences =
                {
                    // Durable Task SDK
                    typeof(TaskActivityContext).Assembly,
                },
            },
        };

        if (isDurableFunctions)
        {
            // Durable Functions code generation is triggered by the presence of the
            // Durable Functions worker extension for .NET Isolated.
            // Assembly functionsWorkerAbstractions = typeof(TriggerBindingAttribute).Assembly;
            // test.TestState.AdditionalReferences.Add(functionsWorkerAbstractions);

            // Assembly functionsWorkerCore = typeof(FunctionContext).Assembly;
            // test.TestState.AdditionalReferences.Add(functionsWorkerCore);
            
            // Assembly durableExtension = typeof(OrchestrationTriggerAttribute).Assembly;
            // test.TestState.AdditionalReferences.Add(durableExtension);

            Assembly dependencyInjection = typeof(ActivatorUtilities).Assembly;
            test.TestState.AdditionalReferences.Add(dependencyInjection);
        }

        return test.RunAsync();
    }

    public static string WrapAndFormat(string generatedClassName, string methodList, bool isDurableFunctions = false)
    {
        string formattedMethodList = IndentLines(spaces: 8, methodList);
        string usings = @"
using System;
using System.Threading.Tasks;
using Dapr.DurableTask.Internal;";

        if (isDurableFunctions)
        {
            usings += @"
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;";
        }

        return $@"
// <auto-generated/>
#nullable enable
{usings}

namespace Microsoft.DurableTask
{{
    public static class {generatedClassName}
    {{
        {formattedMethodList.TrimStart()}
    }}
}}
".TrimStart();
    }

    static string IndentLines(int spaces, string multilineText)
    {
        string indent = new(' ', spaces);
        StringBuilder sb = new();

        foreach (string line in multilineText.Trim().Split(Environment.NewLine))
        {
            if (line.Length > 0)
            {
                sb.Append(indent);
            }

            sb.AppendLine(line);
        }

        return sb.ToString().TrimEnd();
    }

    internal static object DeIndent(string code, int spacesToRemove)
    {
        StringBuilder sb = new(code.Length);
        foreach (string line in code.Split(Environment.NewLine))
        {
            int charsToSkip = Math.Min(spacesToRemove, line.Length);
            sb.AppendLine(line[charsToSkip..]);
        }

        return sb.ToString();
    }

    internal static string GetDefaultInputType(string inputType)
    {
        static bool IsValueType(string typeExpression)
        {
            // This list is obviously incomplete, but should be enhanced as necessary for testing
            switch (typeExpression)
            {
                case "int":
                case "float":
                case "double":
                case "byte":
                case "Guid":
                case "TimeSpan":
                case "DateTime":
                case "DateTimeOffset":
                    return true;
                default:
                    Type? runtimeType = Type.GetType(typeExpression, throwOnError: false);
                    return runtimeType != null && runtimeType.IsValueType;
            }
        };

        if (inputType.StartsWith("(") || inputType.EndsWith('?') || IsValueType(inputType))
        {
            return inputType;
        }

        return inputType + "?";
    }
}
