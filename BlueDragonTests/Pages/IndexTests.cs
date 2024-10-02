using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;

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
            // Act: Render the Index component
            var cut = _ctx!.RenderComponent<BlueDragon.Pages.Index>();

            // Assert: Verify the presence of specific content
            // Have to break up in sections (getting the first h5) for evaluation
            var section = cut.Find("h5.mud-typography-h5");
            section.MarkupMatches(@"
                <h5 class=""mud-typography mud-typography-h5"">
                    Hardware, Cables, Components, and Peripherals (HCCP)<br>
                    is an Inventory Management System created for the fictious company Blue Dragon.
                </h5>");
        }

        // A minimal AuthService mock that can be used for the test
        public class MockAuthService : AuthService
        {
            public MockAuthService() : base(default, default) { }
        }
    }
}