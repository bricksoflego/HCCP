using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Services;

namespace BlueDragonTests.Components
{
    [TestClass]
    public class AuditTests
    {
        private Bunit.TestContext _ctx;

        [TestInitialize]
        public void Setup()
        {
            _ctx = new Bunit.TestContext();

            // Provide a simple mock/stub for AuthService that does nothing
            _ctx.Services.AddScoped<AuthService>(sp => new MockAuthService());

            // Register the required MudBlazor services
            _ctx.Services.AddMudBlazorSnackbar();

            // Register localization services
            _ctx.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Register other MudBlazor services
            _ctx.Services.AddMudServices();
        }

        [TestCleanup]
        public void Teardown()
        {
            _ctx.Dispose();
        }

        [TestMethod]
        public void IndexPageRendersCorrectly()
        {
            // Act: Render the Index component
            var cut = _ctx.RenderComponent<BlueDragon.Components.Audit>();

            // Assert: Verify the presence of specific content
        }

        // A minimal AuthService mock that can be used for the test
        public class MockAuthService : AuthService
        {
            public MockAuthService() : base(null, null) { }
        }
    }
}