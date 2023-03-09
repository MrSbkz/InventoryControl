using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryControl.Models;

public class UserInfoDto
{
    public UserDto User { get; set; }

    public Page<DeviceDto> Devices { get; set; }
}