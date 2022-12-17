using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            try
            {
                var token = await _authService.LoginAsync(model);
                if (token != null)
                {
                    return Ok(new Response<LoginResponse>
                    {
                        IsSuccess = true,
                        Data = token
                    });
                }
                else
                {
                    return Unauthorized(new Response<LoginResponse>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Wrong UserName or password" }
                    });
                }

            }
            catch (Exception e)
            {
                return Unauthorized(e);
            }


        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {           
            try
            {
                var result = await _authService.RegisterAsync(model);
                return Ok(new Response<Response<RegisterModel>>
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<RegisterModel>
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }



        }
    }
}
