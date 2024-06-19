namespace AsyncTasksCancelViaEvent;

public class ErrorEncounteredArgs(int httpStatusErrorCode) : EventArgs
{
    public int HttpStatusErrorCode { get; } = httpStatusErrorCode;
}