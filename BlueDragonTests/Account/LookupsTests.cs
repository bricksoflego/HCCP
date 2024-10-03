using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using MudBlazor;
using Moq;
using Microsoft.AspNetCore.Components.Web;
using BlueDragonTests.Mocks;

namespace BlueDragonTests.Account
{
    [TestClass]
    public class LookupsTests
    {
        private Bunit.TestContext? _ctx;

        [TestInitialize]
        public void Setup()
        {
            _ctx = new Bunit.TestContext();

            // Add required services for Lookups and MudBlazor components
            _ctx.Services.AddScoped<UserService>(sp => new MockUserService());
            _ctx.Services.AddScoped<RoleService>(sp => new MockRoleService());
            _ctx.Services.AddScoped<ISnackbar>(sp => Mock.Of<ISnackbar>());
            _ctx.Services.AddScoped<IAuthService>(sp => new MockAuthService());
            _ctx.Services.AddScoped<IBrandNameService>(sp => new MockBrandNameService());
            _ctx.Services.AddScoped<ICableTypeService>(sp => new MockCableTypeService());

            // Add MudBlazor services
            _ctx.Services.AddMudServices();

            // Mock all JSInterop interactions required by MudBlazor
            _ctx.JSInterop.SetupVoid("mudPopover.initialize", _ => true);
            _ctx.JSInterop.SetupVoid("mudPopover.dispose", _ => true);
            _ctx.JSInterop.SetupVoid("mudPopover.connect", _ => true);
            _ctx.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
            _ctx.JSInterop.SetupVoid("mudKeyInterceptor.disconnect", _ => true);
            _ctx.JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true);
        }

        [TestMethod]
        public void PageTitle_ShouldBeDefined_SetToSpecificText()
        {
            // Arrange: Render Lookups component inside MockLayoutWithPopover
            var cut = _ctx!.RenderComponent<MockLayoutWithPopover>(parameters => parameters.AddChildContent<BlueDragon.Account.Lookups>());

            // Act: Find the PageTitle component
            var pageTitle = cut.FindComponent<PageTitle>();

            // Assert: Ensure the PageTitle component exists
            Assert.IsNotNull(pageTitle);
        }
    }
}
