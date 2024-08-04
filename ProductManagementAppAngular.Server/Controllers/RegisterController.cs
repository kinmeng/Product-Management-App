using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAppAngular.Models;

namespace ProductManagementAppAngular.Server.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public RegisterController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;


        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PreferredName = model.PreferredName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Registration successful" });
                }
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Registration failed: {errors}", string.Join(",", errors));
                return BadRequest(new { message = "Registration failed", errors = result.Errors });
            }

            return BadRequest(ModelState);
        }
    }

    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string PreferredName { get; set; }

    }
}

