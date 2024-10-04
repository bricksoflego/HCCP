using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System.Security.Claims;

namespace BlueDragonTests.Mocks
{
    #region MOCKS
    public class MockAuthService : AuthService
    {
        public MockAuthService() : base(default, default) { }
    }

    public class MockUserService : UserService
    {
        public MockUserService() : base(default, default, default) { }

        public override Task<List<ApplicationUser>> GetUserList()
        {
            // Return a mocked list of users
            return Task.FromResult(new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "User1", Email = "user1@example.com" },
            new ApplicationUser { UserName = "User2", Email = "user2@example.com" }
        });
        }

        public override Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            // Return mocked roles for the user
            return Task.FromResult<IList<string>>(new List<string>
        {
            "Admin",
            "Manager",
            "User"
        });
        }
    }

    public class MockRoleService : RoleService
    {
        public MockRoleService() : base(default) { }

        public override Task<List<IdentityRole>> GetRoleListAsync()
        {
            // Return a mocked list of IdentityRoles
            return Task.FromResult(new List<IdentityRole>
        {
            new IdentityRole { Name = "Admin" },
            new IdentityRole { Name = "Manager" },
            new IdentityRole { Name = "User" }
        });
        }
    }

    public class MockBrandNameService : BrandNameService
    {
        public MockBrandNameService() : base(default) { }

        public override Task<List<LuBrandName>> GetBrandNames()
        {
            // Return a mocked list of brand names
            return Task.FromResult(new List<LuBrandName>
            {
                new LuBrandName { Id = 1, Name="Demo A" },
                new LuBrandName { Id = 2, Name="Demo B" }
            });
        }
    }

    public class MockCableService : CableService
    {
        public MockCableService() : base(default) { }
    }

    public class MockCableTypeService : CableTypeService
    {
        public MockCableTypeService() : base(default) { }

        public override Task<List<LuCableType>> GetCableTypes()
        {
            // Return a mocked list of cable types
            return Task.FromResult(new List<LuCableType>
        {
            new LuCableType{ Id = 1, Name = "USB" },
            new LuCableType{ Id = 2, Name = "HDMI" }
        });
        }
    }

    public class MockHardwareService : HardwareService
    {
        public MockHardwareService() : base(default) { }
    }

    public class MockComponentService : EComponentService
    {
        public MockComponentService() : base(default) { }
    }

    public class MockPeripheralService : PeripheralService
    {
        public MockPeripheralService() : base(default) { }
    }

    public class TestSolutionService : ISolutionService
    {
        public bool UpsertCalled { get; private set; }

        public Task Upsert(string key, bool value)
        {
            UpsertCalled = true;
            return Task.CompletedTask;
        }

        public Task<List<SolutionSetting>> GetSolutionSetings()
        {
            return Task.FromResult(new List<SolutionSetting>());
        }
    }

    public class MockLayoutWithPopover : ComponentBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // Render the MudPopoverProvider as part of the layout
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.AddContent(1, ChildContent);  // Render child components (like Lookups)
            builder.CloseComponent();
        }
    }

    public class MockAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());

        public MockAuthenticationStateProvider(bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                // Default to "Admin" role for authenticated users
                SetAuthenticatedUser("AdminUser", "Admin");
            }
            else
            {
                SetUnauthenticatedUser();
            }
        }

        public void SetAuthenticatedUser(string username, string role)
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }, "mockAuthenticationType"));

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public void SetUnauthenticatedUser()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_user));
        }
    }
    #endregion
}
