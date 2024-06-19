using System.Security.AccessControl;

namespace AsyncTasksCancelViaEvent
{
    public interface ICompositionRequestHandler<in TRequest, in TViewModel>
    {
        event EventHandler<ErrorEncounteredArgs> ErrorEncountered;

        Task Handle(TRequest request, TViewModel viewModel, CancellationToken token = default);
    }

    public abstract class RequestHandlerBase
    {
        public event EventHandler<ErrorEncounteredArgs>? ErrorEncountered;

        protected virtual void OnErrorEncountered(ErrorEncounteredArgs e)
        {
            ErrorEncountered?.Invoke(this, e);
        }
    }
    
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

    public class DescriptionCompositionRequestHandler : RequestHandlerBase, ICompositionRequestHandler<RequestStub, ViewModelStub>
    {
        public async Task Handle(RequestStub requestStub, ViewModelStub viewModelStub, CancellationToken token = default)
        {
            if (!token.IsCancellationRequested)
                await Task.Delay(10, token);

            if (!token.IsCancellationRequested)
                viewModelStub.Description = "Description set by DescriptionCompositionRequestHandler";
        }
    }

    public class ErrorCompositionRequestHandler(int delay, int httpStatusCode)
        : RequestHandlerBase, ICompositionRequestHandler<RequestStub, ViewModelStub>
    {
        public async Task Handle(RequestStub requestStub, ViewModelStub viewModelStub, CancellationToken token = default)
        {
            if (!token.IsCancellationRequested)
                await Task.Delay(delay, token);

            OnErrorEncountered(new ErrorEncounteredArgs(httpStatusCode));
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
}
