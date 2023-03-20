using InventoryControl.Helper;
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
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        [Route("list")]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> GetDevicesAsync(
            string? searchString,
            bool showDecommissionDevice,
            bool showUnassignedDevices,
            int? currentPage = 1,
            int? pageSize = 20)
        {
            try
            {
                var result = await _deviceService.GetDevicesAsync(
                    searchString,
                    showDecommissionDevice,
                    showUnassignedDevices,
                    currentPage!.Value, pageSize!.Value);

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
                var result = await _deviceService.GetDeviceAsync(deviceId);
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
        [Route("device-history")]
        [Authorize]
        public async Task<IActionResult> GetDeviceHistoryAsync(int deviceId)
        {
            try
            {
                var result = await _deviceService.GetDeviceHistoryAsync(deviceId);
                return Ok(new Response<IList<DeviceHistoryDto>>()
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
                var result = await _deviceService.GetEmployeesAsync();
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
        public async Task<FileResult> GetQrCodeAsync(int deviceId)
        {
            var result = await _deviceService.GetQrCodeAsync(deviceId);

            return File(result.Path, result.Type, result.Name);
        }

        [HttpPost]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> AddDeviceAsync(AddDeviceModel model)
        {
            try
            {
                var result = await _deviceService.AddDeviceAsync(model);

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

        [HttpPost]
        [Route("inventory")]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> InventoryAsync(int id)
        {
            try
            {
                var userName = HttpContextHelper.GetUserFromContext(HttpContext);
                var result = await _deviceService.InventoryAsync(id, userName);

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
                var result = await _deviceService.UpdateDeviceAsync(model);

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

        [HttpDelete]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> DecommissDeviceAsync(int deviceId)
        {
            try
            {
                var userName = HttpContextHelper.GetUserFromContext(HttpContext);
                var result = await _deviceService.DecommissDeviceAsync(deviceId, userName);

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
    }
}