using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using przepisy.DTO.Auth;
using przepisy.Models;

namespace przepisy.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRoleDTO dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound("Użytkownik nie istnieej");

            if (!await roleManager.RoleExistsAsync(dto.Role)) return BadRequest("Rola nie istnieje");

            var result = await userManager.AddToRoleAsync(user, dto.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Rola nadana");
        }
    }
}
