using BlueDragon.Account;
using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;

namespace BlueDragonTests.Account
{
    [TestClass]
    public class InventoryTests
    {
        private Bunit.TestContext? _ctx;

        [TestInitialize]
        public void Setup()
        {
            _ctx = new Bunit.TestContext();

            // Provide a simple mock/stub for AuthService
            _ctx.Services.AddScoped<IAuthService>(sp => new MockAuthService());

            // Mock the ISnackbar service to avoid null reference exception
            _ctx.Services.AddScoped<ISnackbar>(sp => Mock.Of<ISnackbar>());

            // Register the required MudBlazor services
            _ctx.Services.AddMudServices();

            // Configure JSInterop to ignore the "mudKeyInterceptor.connect" JS call
            _ctx.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        }

        [TestCleanup]
        public void Teardown()
        {
            _ctx!.Dispose();
        }

        [TestMethod]
        public void AuditComponent_EnsureExists()
        {
            // Act: Render the Inventory page
            var cut = _ctx!.RenderComponent<BlueDragon.Account.Inventory>();

            // Assert: Audit component is present
            var componentExists = cut.HasComponent<BlueDragon.Components.Audit>();
            Assert.IsTrue(componentExists);
        }

        // Minimal AuthService mock
        public class MockAuthService : AuthService
        {
            public MockAuthService() : base(default, default) { }
        }
    }
}
