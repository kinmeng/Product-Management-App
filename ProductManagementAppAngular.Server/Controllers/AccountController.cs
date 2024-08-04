using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAppAngular.Models;

namespace ProductManagementAppAngular.Server.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            if (model == null)
            {
                _logger.LogError("Login model is null.");
                return BadRequest(new { message = "Login model is null." });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("Login successful for user: {Username}", model.Username);
                return Ok(new { message = "Login successful", redirectUrl = "/products" });
                }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out: {Username}", model.Username);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Account locked out." });
            }

            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User account not allowed to sign in: {Username}", model.Username);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Account not allowed." });
            }

            if (result.RequiresTwoFactor)
            {
                _logger.LogWarning("Two-factor authentication required for user: {Username}", model.Username);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Two-factor authentication required." });
            }

            _logger.LogWarning("Login failed for user: {Username}. Failure reason: {FailureReason}", model.Username, "Invalid credentials");
            return Unauthorized(new { message = "Invalid username or password." });
        }
    }

}


public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}