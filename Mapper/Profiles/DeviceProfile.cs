using AutoMapper;
using InventoryControl.Models;
using Device = InventoryControl.Data.Entities.Device;

namespace InventoryControl.Mapper.Profiles;

public class DeviceProfile: Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(x => x.RegisterDate))
            .ForMember(dest => dest.DecommissionDate, opt => opt.MapFrom(x => x.DecommissionDate));
    }
}