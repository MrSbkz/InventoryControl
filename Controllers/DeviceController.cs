using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public async Task GetDeviceAsync()
        {
            
        }
    }
}

