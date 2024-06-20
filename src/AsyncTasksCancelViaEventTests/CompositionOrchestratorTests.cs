using AsyncTasksCancelViaEvent;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Abstractions;

namespace AsyncTasksCancelViaEventTests;

public class CompositionOrchestratorTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async Task CompositionOrchestrator_Handle_ReturnViewModelWithPropertiesSet()
    {
        // Arrange
        var handlers = new List<ICompositionRequestHandler<RequestStub, ViewModelStub>>()
        {
            new DescriptionCompositionRequestHandler(5),
            new IdCompositionRequestHandler(),
            new NameCompositionRequestHandler()
        };
        var sut = new CompositionOrchestrator(handlers);
        var cancellationTokenSource = new CancellationTokenSource(10000);

        // Act
        var result = await sut.HandleRequestHandlers(cancellationTokenSource.Token);

        // Assert
        using (new AssertionScope())
        {
            result.ViewModel.Should().NotBeNull();
            result.ViewModel!.Description.Should().NotBeNullOrEmpty();
            result.ViewModel.Id.Should().NotBeEmpty();
            result.ViewModel.Name.Should().NotBeNullOrEmpty();
            result.HttpStatusCode.Should().Be(200);
        }
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(18)]
    [InlineData(19)]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(22)]
    [InlineData(23)]
    [InlineData(24)]
    [InlineData(25)]
    public async Task CompositionOrchestrator_Handle_ReturnResultWithError_WhenCancelledByHandler(int delay)
    {
        // Arrange
        var handlers = new List<ICompositionRequestHandler<RequestStub, ViewModelStub>>()
        {
            new DescriptionCompositionRequestHandler(delay),
            new ErrorCompositionRequestHandler(12, 500),
            new ErrorCompositionRequestHandler(12, 502)
        };

        var sut = new CompositionOrchestrator(handlers);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        var result = await sut.HandleRequestHandlers(cancellationTokenSource.Token);

        // Assert
        _output.WriteLine("Error Code: {0}, msg {1}", result.HttpStatusCode, result.ErrorMessage);
        using (new AssertionScope())
        {
            result.ViewModel.Should().BeNull();
            result.HttpStatusCode.Should().BeInRange(500, 502);
        }
    }

    [Fact]
    public async Task CompositionOrchestrator_Handle_ReturnResult_WhenCancelledExternally()
    {
        // Arrange
        var handlers = new List<ICompositionRequestHandler<RequestStub, ViewModelStub>>()
        {
            new DescriptionCompositionRequestHandler(5),
            new ErrorCompositionRequestHandler(50, 500),
            new ErrorCompositionRequestHandler(5, 500)
        };

        var sut = new CompositionOrchestrator(handlers);

        var cancellationTokenSource = new CancellationTokenSource(5);

        // Act
        var result = await sut.HandleRequestHandlers(cancellationTokenSource.Token);

        // Assert
        using (new AssertionScope())
        {
            result.ViewModel.Should().BeNull();
            result.HttpStatusCode.Should().Be(0);
        }
        _output.WriteLine("Error Code: {0}", result.HttpStatusCode);
    }
}