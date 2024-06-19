namespace AsyncTasksCancelViaEvent;

public class Result<TViewModel> 
    where TViewModel : class
{
    private Result(TViewModel? viewModel, int httpStatusCode = 200)
    {
        ViewModel = viewModel;
        HttpStatusCode = httpStatusCode;
    }

    public int HttpStatusCode { get; private set; }
    public TViewModel? ViewModel { get; private set; }

    public static Result<TViewModel> Success(TViewModel result) => new(result);
    public static Result<TViewModel> Failure(int httpStatusCode) => new(null, httpStatusCode);
}