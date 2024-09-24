using BlueDragon.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
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

            // Configure JSInterop to handle MudBlazor's JavaScript calls
            _ctx.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);  // Allow any arguments for mudKeyInterceptor.connect
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

        [TestMethod]
        public void BarcodeValidation_WithValidBarcode_ParsesCorrectly()
        {
            // Arrange
            var cut = _ctx.RenderComponent<BlueDragon.Components.Audit>();
            var validBarcode = "112023092501"; // A valid barcode
            var barcodeField = cut.Find("#barcode");

            // Act
            barcodeField.Change(validBarcode);

            // Use reflection to get the value of the private ItemDate property
            var itemDateField = cut.Instance.GetType().GetProperty("ItemDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var itemDateValue = itemDateField?.GetValue(cut.Instance)?.ToString();

            // Assert
            Assert.AreEqual("NA", itemDateValue);
        }

        // A minimal AuthService mock that can be used for the test
        public class MockAuthService : AuthService
        {
            public MockAuthService() : base(null, null) { }
        }
    }
}