using AutoMapper;
using InventoryControl.Data.Entities;
using InventoryControl.Models;

namespace InventoryControl.Mapper.Profiles;

public class DeviceProfile: Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDto>()
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(x => x.User));
        CreateMap<User, Employee>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName));
    }
}