using AsyncTasksCancelViaEvent;

namespace AsyncTasksCancelViaEventTests
{
    public class IdCompositionRequestHandler : RequestHandlerBase, ICompositionRequestHandler<RequestStub, ViewModelStub>
    {
        public async Task Handle(RequestStub requestStub, ViewModelStub viewModelStub, CancellationToken token = default)
        {
            if (!token.IsCancellationRequested)
                await Task.Delay(5, token);

            if (!token.IsCancellationRequested)
                viewModelStub.Id = Guid.NewGuid();
        }
    }

    public class DescriptionCompositionRequestHandler(int delay) : RequestHandlerBase, ICompositionRequestHandler<RequestStub, ViewModelStub>
    {
        public async Task Handle(RequestStub requestStub, ViewModelStub viewModelStub, CancellationToken token = default)
        {
            if (!token.IsCancellationRequested)
                await Task.Delay(delay, token);

            if (!token.IsCancellationRequested)
                viewModelStub.Description = "Description set by DescriptionCompositionRequestHandler";
        }
    }

    public class NameCompositionRequestHandler : RequestHandlerBase, ICompositionRequestHandler<RequestStub, ViewModelStub>
    {
        public async Task Handle(RequestStub requestStub, ViewModelStub viewModelStub, CancellationToken token = default)
        {
            if (!token.IsCancellationRequested)
                await Task.Delay(15, token);

            if (!token.IsCancellationRequested)
                viewModelStub.Name = "Name set by NameCompositionRequestHandler";
        }
    }

    public class ErrorCompositionRequestHandler(int delay, int httpStatusCode)
        : RequestHandlerBase, ICompositionRequestHandler<RequestStub, ViewModelStub>
    {
        public async Task Handle(RequestStub requestStub, ViewModelStub viewModelStub, CancellationToken token = default)
        {
            if (!token.IsCancellationRequested)
                await Task.Delay(delay, token);

            if (!token.IsCancellationRequested)
                OnErrorEncountered(new ErrorEncounteredArgs(httpStatusCode));
        }
    }
}
