using InventoryControl.Helper;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("list")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsersAsync(
            string? searchString,
            bool showInactiveUsers,
            int? currentPage = 1,
            int? pageSize = 20)
        {
            try
            {
                var result = await _userService.GetUsersAsync(
                    searchString,
                    showInactiveUsers,
                    currentPage!.Value,
                    pageSize!.Value);

                return Ok(new Response<Page<UserDto>>()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<string>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            try
            {
                var userName = HttpContextHelper.GetUserFromContext(HttpContext);
                var result = await _userService.GetUserAsync(userName);
                return Ok(new Response<UserDto>()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<string>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUserAsync(RegisterModel model)
        {
            try
            {
                var result = await _userService.AddUserAsync(model);

                if (result.IsSuccess == false)
                {
                    return BadRequest(new Response<RegisterResponse>
                    {
                        IsSuccess = false,
                        Errors = result.Data!.ToList()
                    });
                }

                return Ok(new Response<string>
                {
                    IsSuccess = true,
                    Data = result.Data!.FirstOrDefault()
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

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserModel model)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(model);

                return Ok(new Response<string>()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<string>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUserAsync(string userName)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userName);

                return Ok(new Response<string>()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<string>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("restore")]
        public async Task<IActionResult> RestoreUserAsync(string userName)
        {
            try
            {
                var result = await _userService.RestoreUserAsync(userName);

                return Ok(new Response<string>()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response<string>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                });
            }
        }
    }
}