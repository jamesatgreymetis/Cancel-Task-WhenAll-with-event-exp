using AsyncTasksCancelViaEvent;
using FluentAssertions;
using Xunit.Abstractions;

namespace AsyncTasksCancelViaEventTests
{
    public class CompositionOrchestratorTests(ITestOutputHelper output)
    {
        private readonly ITestOutputHelper _output = output;

        [Fact]
        public async Task CompositionOrchestrator_Handle_ReturnViewModelWithPropertiesSet()
        {
            // Arrange
            var handlers = new List<ICompositionRequestHandler<RequestStub, ViewModelStub>>()
            {
                new DescriptionCompositionRequestHandler(),
                new IdCompositionRequestHandler(),
                new NameCompositionRequestHandler()
            };
            var sut = new CompositionOrchestrator(handlers);

            // Act
            var result = await sut.HandleRequestHandlers();

            // Assert
            result.ViewModel.Should().NotBeNull();
            result.HttpStatusCode.Should().Be(200);
        }

        [Fact]
        public async Task CompositionOrchestrator_Handle_ReturnResultWithError()
        {
            // Arrange
            var handlers = new List<ICompositionRequestHandler<RequestStub, ViewModelStub>>()
            {
                new DescriptionCompositionRequestHandler(),
                new IdCompositionRequestHandler(),
                new NameCompositionRequestHandler(),
                new ErrorCompositionRequestHandler(10, 500),
                new ErrorCompositionRequestHandler(10, 502)
            };

            var sut = new CompositionOrchestrator(handlers);

            // Act
            var result = await sut.HandleRequestHandlers();

            // Assert
            result.ViewModel.Should().BeNull();
            result.HttpStatusCode.Should().BeGreaterThan(499);
            _output.WriteLine("Error Code: {0}", result.HttpStatusCode);
        }
    }
}