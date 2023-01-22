﻿using InventoryControl.Helper;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _device;

        public DeviceController(IDeviceService device)
        {
            _device = device;
        }

        [HttpGet]
        [Route("list")]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> GetDevicesAsync(int? currentPage = 1, int? pageSize = 20)
        {
            try
            {
                var result = await _device.GetDevicesAsync(currentPage!.Value, pageSize!.Value);

                return Ok(new Response<Page<DeviceDto>>()
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
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> GetDeviceAsync(int deviceId)
        {
            try
            {
                var result = await _device.GetDeviceAsync(deviceId);
                return Ok(new Response<DeviceDto>()
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
        [Route("employees")]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            try
            {
                var result = await _device.GetEmployeesAsync();
                return Ok(new Response<IList<Employee>>()
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
        [Route("qr-code")]
        [Authorize(Roles = "accountant")]
        public async Task<FileResult> GetDeviceByQrAsync(int deviceId)
        {
            var result = await _device.GetQrCodeAsync(deviceId);

            return File(result.Path, result.Type, result.Name);
        }

        [HttpPost]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> AddDeviceAsync(AddDeviceModel model)
        {
            try
            {
                var result = await _device.AddDeviceAsync(model);

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

        [HttpPost]
        [Route("inventory")]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> InventoryAsync(int id)
        {
            try
            {
                var userName = HttpContextHelper.GetUserFromContext(HttpContext);
                var result = await _device.InventoryAsync(id, userName);

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
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> UpdateDeviceAsync(UpdateDeviceModel model)
        {
            try
            {
                var result = await _device.UpdateDeviceAsync(model);

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
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> DecommissDeviceAsync(int deviceId)
        {
            try
            {
                var result = await _device.DecommissDeviceAsync(deviceId);

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