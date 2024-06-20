namespace AsyncTasksCancelViaEvent;

public class CompositionOrchestrator(List<ICompositionRequestHandler<RequestStub, ViewModelStub>> handlers)
{
    private CancellationTokenSource? _cancellationTokenSource;
    private int? _httpStatusErrorCodeEncountered;

    public async Task<Result<ViewModelStub>> HandleRequestHandlers(CancellationToken token = default)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        var request = new RequestStub();
        var viewModel = new ViewModelStub();

        // Register event handlers
        handlers
            .ForEach(h=> h.ErrorEncountered += RequestHandler_ErrorEncountered);

        // build list of tasks
        var tasks = handlers
            .Select(h => h.Handle(request, viewModel, _cancellationTokenSource.Token));

        // await all tasks
        try
        {
            await Task.WhenAll(tasks);

            return _httpStatusErrorCodeEncountered.HasValue
                ? Result<ViewModelStub>.Failure(_httpStatusErrorCodeEncountered.Value)
                : Result<ViewModelStub>.Success(viewModel);
        }
        catch (OperationCanceledException exception)
        {
            return
                _httpStatusErrorCodeEncountered.HasValue
                    ? Result<ViewModelStub>.Failure(_httpStatusErrorCodeEncountered.Value, exception.Message)
                    : Result<ViewModelStub>.Failure(0, exception.StackTrace);
        }
        finally
        {
            _cancellationTokenSource.Dispose();
        }
    }

    private void RequestHandler_ErrorEncountered(object? sender, ErrorEncounteredArgs e)
    {
        _cancellationTokenSource?.Cancel();
        _httpStatusErrorCodeEncountered = e.HttpStatusErrorCode;
    }
}