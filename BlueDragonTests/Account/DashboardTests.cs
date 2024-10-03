using BlueDragon.Account;
using BlueDragonTests.Mocks;
using Moq;
using MudBlazor;

namespace BlueDragonTests.Account
{
    [TestClass]
    public class DashboardTests
    {
        private Dashboard? _dashboard;
        private Mock<ISnackbar>? _mockSnackbar;
        private TestSolutionService? _testSolutionService;

        [TestInitialize]
        public void Setup()
        {
            _mockSnackbar = new Mock<ISnackbar>();

            // Mock Snackbar.Configuration to avoid null reference exception
            var mockSnackbarConfig = new Mock<SnackbarConfiguration>();
            _mockSnackbar.Setup(s => s.Configuration).Returns(mockSnackbarConfig.Object);
            _testSolutionService = new TestSolutionService();
            _dashboard = new Dashboard
            {
                Snackbar = _mockSnackbar.Object,  // Pass in the mocked Snackbar
                SolutionService = _testSolutionService
            };
        }

        [TestMethod]
        public async Task BeginAudit_ShouldCallSnackbarAdd()
        {
            // Act
            await _dashboard!.BeginAudit();

            // Assert
            Assert.IsTrue(_dashboard.AuditInProgress);
            _mockSnackbar!.Verify(s => s.Add(It.IsAny<string>(), It.IsAny<Severity>(), It.IsAny<Action<SnackbarOptions>>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task StopAudit_ShouldCallSnackbarAdd()
        {
            // Act
            await _dashboard!.StopAudit();

            // Assert
            Assert.IsFalse(_dashboard.AuditInProgress);
            _mockSnackbar!.Verify(s => s.Add(It.IsAny<string>(), It.IsAny<Severity>(), It.IsAny<Action<SnackbarOptions>>(), It.IsAny<string>()), Times.Once);
        }
    }
}
