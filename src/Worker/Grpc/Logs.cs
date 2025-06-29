// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace Dapr.DurableTask.Worker.Grpc
{
    /// <summary>
    /// Log messages.
    /// </summary>
    static partial class Logs
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Durable Task gRPC worker starting and connecting to {endpoint}.")]
        public static partial void StartingTaskHubWorker(this ILogger logger, string endpoint);

        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Durable Task gRPC worker has disconnected from gRPC server.")]
        public static partial void SidecarDisconnected(this ILogger logger);

        [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Sidecar unavailable after {connectionDuration}: {status} {statusCode} {message}")]
        public static partial void SidecarUnavailableWithDetails(this ILogger logger, string connectionDuration, Status status, StatusCode statusCode, string message);

        [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Sidecar work-item streaming connection established.")]
        public static partial void EstablishedWorkItemConnection(this ILogger logger);

        [LoggerMessage(EventId = 5, Level = LogLevel.Warning, Message = "Task hub NotFound. Will continue retrying.")]
        public static partial void TaskHubNotFound(this ILogger logger);

        [LoggerMessage(EventId = 10, Level = LogLevel.Debug, Message = "{instanceId}: Received request to run orchestrator '{name}' with {oldEventCount} replay and {newEventCount} new history events.")]
        public static partial void ReceivedOrchestratorRequest(this ILogger logger, string name, string instanceId, int oldEventCount, int newEventCount);

        [LoggerMessage(EventId = 11, Level = LogLevel.Debug, Message = "{instanceId}: Sending {count} action(s) [{actionsList}] for '{name}' orchestrator.")]
        public static partial void SendingOrchestratorResponse(this ILogger logger, string name, string instanceId, int count, string actionsList);

        [LoggerMessage(EventId = 12, Level = LogLevel.Warning, Message = "{instanceId}: '{name}' orchestrator failed with an unhandled exception: {details}.")]
        public static partial void OrchestratorFailed(this ILogger logger, string name, string instanceId, string details);

        [LoggerMessage(EventId = 13, Level = LogLevel.Debug, Message = "{instanceId}: Received request to run activity '{name}#{taskId}' with {sizeInBytes} bytes of input data.")]
        public static partial void ReceivedActivityRequest(this ILogger logger, string name, int taskId, string instanceId, int sizeInBytes);

        [LoggerMessage(EventId = 14, Level = LogLevel.Debug, Message = "{instanceId}: Sending {successOrFailure} response for '{name}#{taskId}' activity with {sizeInBytes} bytes of output data.")]
        public static partial void SendingActivityResponse(this ILogger logger, string successOrFailure, string name, int taskId, string instanceId, int sizeInBytes);

        [LoggerMessage(EventId = 20, Level = LogLevel.Error, Message = "Unexpected error in handling of instance ID '{instanceId}'.")]
        public static partial void UnexpectedError(this ILogger logger, Exception ex, string instanceId);

        [LoggerMessage(EventId = 21, Level = LogLevel.Warning, Message = "Received and dropped an unknown '{type}' work-item from the sidecar.")]
        public static partial void UnexpectedWorkItemType(this ILogger logger, string type);

        [LoggerMessage(EventId = 55, Level = LogLevel.Information, Message = "{instanceId}: Evaluating custom retry handler for failed '{name}' task. Attempt = {attempt}.")]
        public static partial void RetryingTask(this ILogger logger, string instanceId, string name, int attempt);

        [LoggerMessage(EventId = 56, Level = LogLevel.Warning, Message = "Channel to backend has stopped receiving traffic, will attempt to reconnect.")]
        public static partial void ConnectionTimeout(this ILogger logger);

        [LoggerMessage(EventId = 57, Level = LogLevel.Warning, Message = "Orchestration version did not meet worker versioning requirements. Error action = '{errorAction}'. Version error = '{versionError}'")]
        public static partial void OrchestrationVersionFailure(this ILogger logger, string errorAction, string versionError);

        [LoggerMessage(EventId = 58, Level = LogLevel.Information, Message = "Abandoning orchestration. InstanceId = '{instanceId}'. Completion token = '{completionToken}'")]
        public static partial void AbandoningOrchestrationDueToVersioning(this ILogger logger, string instanceId, string completionToken);

        [LoggerMessage(EventId = 59, Level = LogLevel.Debug, Message = "Cancellation requested. Message: '{message}'")]
        public static partial void CancellationRequested(this ILogger logger, string message);

        [LoggerMessage(EventId = 60, Level = LogLevel.Debug, Message = "Starting reconnection attempt #{attemptCount}")]
        public static partial void StartingReconnectAttempt(this ILogger logger, int attemptCount);

        [LoggerMessage(EventId = 61, Level = LogLevel.Debug, Message = "Task hub worker exited after {elapsedTimeMs} ms")]
        public static partial void TaskHubWorkerExited(this ILogger logger, long elapsedTimeMs);

        [LoggerMessage(EventId = 62, Level = LogLevel.Debug, Message = "Error in task hub worker, attempt #{reconnectionAttempts}: {exceptionType}: {exceptionMessage}")]
        public static partial void TaskHubWorkerError(this ILogger logger, int reconnectionAttempts, string exceptionType, string exceptionMessage, Exception ex);

        [LoggerMessage(EventId = 63, Level = LogLevel.Debug, Message = "Using provided gRPC channel with target '{target}'")]
        public static partial void GrpcChannelTarget(this ILogger logger, string target);

        [LoggerMessage(EventId = 64, Level = LogLevel.Debug, Message = "Using provided CallInvoker")]
        public static partial void SelectGrpcCallInvoker(this ILogger logger);

        [LoggerMessage(EventId = 65, Level = LogLevel.Debug, Message = "Creating new gRPC channel for address '{address}'")]
        public static partial void CreatingGrpcChannelForAddress(this ILogger logger, string address);

        [LoggerMessage(EventId = 66, Level = LogLevel.Debug, Message = "Shutting down gRPC channel for address '{address}'")]
        public static partial void ShuttingDownGrpcChannel(this ILogger logger, string address);

        [LoggerMessage(EventId = 67, Level = LogLevel.Debug, Message = "Configuring gRPC call with no deadline constraint")]
        public static partial void ConfiguringGrpcCallOptions(this ILogger logger);

        [LoggerMessage(EventId = 68, Level = LogLevel.Debug, Message = "Opening stream connection to get work items")]
        public static partial void OpeningTaskStream(this ILogger logger);

        [LoggerMessage(EventId = 69, Level = LogLevel.Debug, Message = "Received work item of type '{workItemType}' at '{lastActivityTimestamp}'")]
        public static partial void ReceivedWorkItem(this ILogger logger, string workItemType, DateTime lastActivityTimestamp);

        [LoggerMessage(EventId = 70, Level = LogLevel.Debug, Message = "Connection stats: Duration={connectionDuration}, LastActivity={timeSinceLastActivity}, WorkItemsProcessed={workItemsProcessed}")]
        public static partial void ConnectionStats(this ILogger logger, string connectionDuration, string timeSinceLastActivity, int workItemsProcessed);

        [LoggerMessage(EventId = 71, Level = LogLevel.Warning, Message = "Work item stream ended gracefully after {connectionDuration}. This is unusual but not necessarily an error.")]
        public static partial void StreamEndedGracefully(this ILogger logger, string connectionDuration);

        [LoggerMessage(EventId = 72, Level = LogLevel.Warning, Message = "gRPC call cancelled after {connectionDuration}: {status} {statusCode} {message}")]
        public static partial void GrpcCallCancelled(this ILogger logger, string connectionDuration, Status status, StatusCode statusCode, string message);

        [LoggerMessage(EventId = 73, Level = LogLevel.Warning, Message = "Unexpected error in gRPC worker after {connectionDuration}: {exceptionType}: {exceptionMessage}")]
        public static partial void GrpcCallUnexpectedError(this ILogger logger, string connectionDuration, string exceptionType, string exceptionMessage, Exception ex);

        [LoggerMessage(EventId = 74, Level = LogLevel.Debug, Message = "Waiting {delaySeconds} seconds before reconnection attempt #{reconnectAttempt}")]
        public static partial void ReconnectionDelay(this ILogger logger, int delaySeconds, int reconnectAttempt);
    }
}
