using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
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
        [Route("getusers")]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                var result = await _userService.GetUsersAsync();

                return Ok(new Response<IList<UserDTO>>()
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
        [Route("getbyuser")]
        public async Task<IActionResult> GetByUserAsync([FromQuery] UserDTO dto)
        {
            try
            {
                var result = await _userService.GetByUserAsync(dto);
                if (result != null)
                {
                    return Ok(new Response<User>()
                    {
                        IsSuccess = true,
                        Data = result
                    });
                }

                return BadRequest(new Response<string>()
                {
                    IsSuccess = false,
                    Errors = new List<string> { result!.ToString() }
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
        [Route("adduser")]
        public async Task<IActionResult> AddUSerAsync(RegisterModel model)
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
        [Route("update")]
        public async Task<IActionResult> UpdateUserAsync(UserDTO dto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(dto);

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
        [Route("reset-password")]
        public async Task<IActionResult> ResetPasswordUserAsync(User model, string lastPassword, string newPassword)
        {
            try
            {
                var result = await _userService.ResetPasswordAsync(model, lastPassword, newPassword);

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
        [Route("delete")]
        public async Task<IActionResult> DeleteUserAsync(UserDTO dto)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(dto);

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