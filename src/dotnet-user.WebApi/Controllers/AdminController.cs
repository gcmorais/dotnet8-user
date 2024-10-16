using dotnet_user.WebApi.DTOs;
using dotnet_user_api.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_user.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }

            return Ok(users);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Email == "masteradmin@example.com")
            {
                return BadRequest("Cannot delete the Master Admin.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "MasterAdmin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var user = new User(model.Email, model.Email, model.FullName, model.DateOfBirth);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok($"Admin user '{user.UserName}' created successfully.");
        }

        [Authorize(Roles = "MasterAdmin")]
        [HttpPost("{userId}/assign-role")]
        public async Task<IActionResult> AssignRoleToUser(string userId, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest($"Role '{role}' does not exist.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok($"User '{user.UserName}' assigned to role '{role}'.");
            }

            return BadRequest(result.Errors);
        }
    }

}
