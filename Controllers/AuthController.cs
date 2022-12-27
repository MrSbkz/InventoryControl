using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [Route("controller")]
    [ApiController]
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

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);

                if (result.IsSuccess == false)
                {
                    return BadRequest(new Response<RegisterResponse>
                    {
                        IsSuccess = false,
                        Errors = result.Data.ToList()
                    });
                }

                return Ok(new Response<string>
                {
                    IsSuccess = true,
                    Data = result.Data.FirstOrDefault()
                });
            }

            catch (Exception e)
            {
                return BadRequest(new Response<RegisterResponse>
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }
    }
}
