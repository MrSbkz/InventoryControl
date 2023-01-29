using InventoryControl.Helper;
using InventoryControl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("user/list")]
        [Authorize]
        [ProducesResponseType(typeof(IList<string>), 200)]
        public IActionResult GetRoles()
        {
            try
            {
                return Ok(new Response<IList<string>>
                {
                    IsSuccess = true,
                    Data = HttpContextHelper.GetRoleFromContext(HttpContext)
                });
            }

            catch (Exception e)
            {
                return BadRequest(new Response<IList<string>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }

        [HttpGet]
        [Route("list")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IList<string>), 200)]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            try
            {
                return Ok(new Response<List<string>>
                {
                    IsSuccess = true,
                    Data = await _roleManager.Roles.Select(x => x.Name).ToListAsync()
                });
            }

            catch (Exception e)
            {
                return BadRequest(new Response<IList<string>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }
    }
}