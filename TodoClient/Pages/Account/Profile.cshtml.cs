using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace TodoClient.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public UserModel Profile { get; set; }

        public void OnGet()
        {
            Profile = new UserModel
            {
                Name = User.Identity.Name,
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            };
        }

        public class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
