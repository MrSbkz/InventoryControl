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
        [Authorize(Roles = "admin,accountant")]
        public async Task<IActionResult> GetDeviceAsync(int? currentPage = 1, int? pageSize = 20)
        {
            try
            {
                var result = await _device.GetDeviceListAsync(currentPage!.Value, pageSize!.Value);

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

        [HttpPost]
        [Authorize(Roles = "accountant")]
        public async Task<IActionResult> AddDeviceAsync(RegisterDeviceModel model)
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
    }
}