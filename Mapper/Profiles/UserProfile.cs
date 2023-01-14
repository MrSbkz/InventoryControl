using InventoryControl.Data.Entities;
using InventoryControl.Models;
using AutoMapper;

namespace InventoryControl.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, Employee>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName));
    }
}