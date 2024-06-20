namespace AsyncTasksCancelViaEvent;

public class Result<TViewModel> 
    where TViewModel : class
{
    private Result(TViewModel? viewModel, int httpStatusCode = 200, string? errorMessage = null)
    {
        ViewModel = viewModel;
        HttpStatusCode = httpStatusCode;
        ErrorMessage = errorMessage;
    }

    public int HttpStatusCode { get; private set; }
    public string? ErrorMessage { get; }
    public TViewModel? ViewModel { get; private set; }

    public static Result<TViewModel> Success(TViewModel result) => new(result);
    public static Result<TViewModel> Failure(int httpStatusCode, string? exceptionMessage = null) => new(null, httpStatusCode, exceptionMessage);
}