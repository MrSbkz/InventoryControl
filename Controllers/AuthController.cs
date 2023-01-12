using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            try
            {
                var token = await _authService.LoginAsync(model);

                return Ok(new Response<LoginResponse>
                {
                    IsSuccess = true,
                    Data = token
                });
            }

            catch (Exception e)
            {
                return BadRequest(new Response<LoginResponse>
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }
    }
}