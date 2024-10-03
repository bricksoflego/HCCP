using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using BlueDragonTests.Mocks;

namespace BlueDragonTests.Pages
{
    [TestClass]
    public class IndexTests
    {
        private Bunit.TestContext? _ctx;

        [TestInitialize]
        public void Setup()
        {
            _ctx = new Bunit.TestContext();

            // Provide a simple mock/stub for AuthService that does nothing
            _ctx.Services.AddScoped<IAuthService>(sp => new MockAuthService());
        }

        [TestCleanup]
        public void Teardown()
        {
            _ctx!.Dispose();
        }

        [TestMethod]
        public void IndexPageRendersCorrectly()
        {
            // Arrange: Render the Index component
            var cut = _ctx!.RenderComponent<BlueDragon.Pages.Index>();

            // Act: Search for the element to check
            var section = cut.Find("h5.mud-typography-h5");

            // Assert: Verify the presence of specific content
            // Have to break up in sections (getting the first h5) for evaluation
            section.MarkupMatches(@"
                <h5 class=""mud-typography mud-typography-h5"">
                    Hardware, Cables, Components, and Peripherals (HCCP)<br>
                    is an Inventory Management System created for the fictious company Blue Dragon.
                </h5>");
        }
    }
}