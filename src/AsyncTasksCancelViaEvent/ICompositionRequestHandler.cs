namespace AsyncTasksCancelViaEvent;

public interface ICompositionRequestHandler<in TRequest, in TViewModel>
{
    event EventHandler<ErrorEncounteredArgs> ErrorEncountered;

    Task Handle(TRequest request, TViewModel viewModel, CancellationToken token = default);
}