using AsyncTasksCancelViaEvent;
using FluentAssertions;

namespace AsyncTasksCancelViaEventTests
{
    public class CompositionOrchestratorTests
    {
        [Fact]
        public async Task CompositionOrchestrator_Handle_ReturnViewModelWithPropertiesSet()
        {
            var sut = new CompositionOrchestrator();

            var result = await sut.HandleRequestHandlers();

            result.Should().NotBeNull();
        }
    }
}