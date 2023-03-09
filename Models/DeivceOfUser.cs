using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryControl.Models;

public class DeivceOfUser
{
    public UserDto User { get; set; }

    public Page<DeviceDto> Device { get; set; }
}