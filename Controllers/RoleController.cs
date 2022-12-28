using InventoryControl.Helper;
using InventoryControl.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class RoleController : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}
