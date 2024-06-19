namespace AsyncTasksCancelViaEvent;

public class CompositionOrchestrator
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<ICompositionRequestHandler<RequestStub, ViewModelStub>> _handlers;
    private int? _httpStatusErrorCodeEncountered;

    public CompositionOrchestrator(List<ICompositionRequestHandler<RequestStub, ViewModelStub>> handlers)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _handlers = handlers;
    }

    public async Task<Result<ViewModelStub>> HandleRequestHandlers()
    {
        var request = new RequestStub();
        var viewModel = new ViewModelStub();

        // Register event handlers
        _handlers
            .ForEach(h=> h.ErrorEncountered += RequestHandler_ErrorEncountered);

        // build list of tasks
        var tasks = _handlers
            .Select(h => h.Handle(request, viewModel, _cancellationTokenSource.Token));

        // await all tasks
        await Task.WhenAll(tasks);

        return _httpStatusErrorCodeEncountered.HasValue ? 
            Result<ViewModelStub>.Failure(_httpStatusErrorCodeEncountered.Value) : 
            Result<ViewModelStub>.Success(viewModel);
    }

    private void RequestHandler_ErrorEncountered(object? sender, ErrorEncounteredArgs e)
    {
        _cancellationTokenSource.Cancel();
        _httpStatusErrorCodeEncountered = e.HttpStatusErrorCode;
    }
}