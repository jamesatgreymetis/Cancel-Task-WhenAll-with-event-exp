namespace AsyncTasksCancelViaEvent;

public abstract class RequestHandlerBase
{
    public event EventHandler<ErrorEncounteredArgs>? ErrorEncountered;

    protected virtual void OnErrorEncountered(ErrorEncounteredArgs e)
    {
        ErrorEncountered?.Invoke(this, e);
    }
}