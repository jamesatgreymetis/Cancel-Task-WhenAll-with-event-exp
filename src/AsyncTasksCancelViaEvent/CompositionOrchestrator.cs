namespace AsyncTasksCancelViaEvent;

public class CompositionOrchestrator
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<ICompositionRequestHandler<RequestStub, ViewModelStub>> _listHandlers;
    private int? _httpStatusErrorCodeEncountered;

    public CompositionOrchestrator()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _listHandlers = new List<ICompositionRequestHandler<RequestStub, ViewModelStub>>()
        {
            new DescriptionCompositionRequestHandler(),
            new IdCompositionRequestHandler(),
            new NameCompositionRequestHandler()
        };
    }

    public async Task<Result<ViewModelStub>> HandleRequestHandlers()
    {
        var request = new RequestStub();
        var viewModel = new ViewModelStub();

        // Register event handlers
        _listHandlers
            .ForEach(h=> h.ErrorEncountered += RequestHandler_ErrorEncountered);

        // build list of tasks
        var tasks = _listHandlers
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