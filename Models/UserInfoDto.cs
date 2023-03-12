using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryControl.Models;

public class UserInfoDto
{
    public UserDto User { get; set; }

    public IList<DeviceDto> Devices { get; set; }
}