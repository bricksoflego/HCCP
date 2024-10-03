using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using BlueDragonTests.Mocks;

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

            // Create required Stubs & Mocks
            _ctx.Services.AddScoped<IAuthService>(sp => new MockAuthService());
            _ctx.Services.AddScoped<ISnackbar>(sp => Mock.Of<ISnackbar>());
            _ctx.Services.AddMudServices();
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
            // Arrange: Render the Inventory page
            var cut = _ctx!.RenderComponent<BlueDragon.Account.Inventory>();
            var componentExists = cut.HasComponent<BlueDragon.Components.Audit>();

            // Assert: Audit component is present
            Assert.IsTrue(componentExists);
        }
    }
}
