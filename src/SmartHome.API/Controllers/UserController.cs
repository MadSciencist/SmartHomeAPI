using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Dto;
using SmartHome.API.Security.Token;
using SmartHome.Core.Domain.User;
using SmartHome.Core.Utils;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        // TODO create userService to cleanup this mess
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IPasswordValidator<AppUser> _passwordValidator;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenBuilder tokenBuilder, IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenBuilder = tokenBuilder;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // only admin or user itself can access
            if (ClaimsPrincipalHelper.IsUserAdmin(User) || ClaimsPrincipalHelper.HasUserClaimedIdentifier(User, id))
                return Ok(user); // TODO dedicated DTO to hide sensitive data

            return Forbid();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login, string redirect)
        {
            var user = await _userManager.FindByNameAsync(login.Login);
            if (user == null) return NotFound();

            if (!user.IsActive) return BadRequest();

            await _signInManager.SignOutAsync(); // terminate existing session

            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
            if (!signInResult.Succeeded) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiring) = _tokenBuilder.Build(user, roles);

            return Ok(new { access = new { token, expires = expiring }, redirect });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register, string redirect)
        {
            if (await _userManager.FindByNameAsync(register.Login) != null)
            {
                ModelState.AddModelError("", "User with that login already exists.");
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                Email = register.Email,
                UserName = register.Login,
                IsActive = false
            };

            var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, register.Password);
            if (!passwordValidationResult.Succeeded) return BadRequest(passwordValidationResult.Errors);

            var createResult = await _userManager.CreateAsync(user, register.Password);
            if (!createResult.Succeeded) return BadRequest(createResult.Errors);

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!addToRoleResult.Succeeded) return BadRequest(addToRoleResult.Errors);

            var signInResult = await _signInManager.PasswordSignInAsync(user, register.Password, false, false);
            if (!signInResult.Succeeded) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiring) = _tokenBuilder.Build(user, roles);

            return CreatedAtAction(nameof(Register), "", new { access = new { token, expires = expiring }, redirect });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // only resource owner can modify it
            if (!ClaimsPrincipalHelper.HasUserClaimedIdentifier(User, id))
                return Forbid();

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded) return BadRequest(deleteResult.Errors);

            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] RegisterDto updatedModel, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // only resource owner can modify it
            if (!ClaimsPrincipalHelper.HasUserClaimedIdentifier(User, id))
                return Forbid();

            var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, updatedModel.Password);
            if (!passwordValidationResult.Succeeded) return BadRequest(passwordValidationResult.Errors);

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updatedModel.Password);
            user.Email = updatedModel.Email;

            var userValidationResult = await _userValidator.ValidateAsync(_userManager, user);
            if (!userValidationResult.Succeeded) return BadRequest(userValidationResult.Errors);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

            return Ok();
        }
    }
}
