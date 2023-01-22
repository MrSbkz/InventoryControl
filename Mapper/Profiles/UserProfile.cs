using InventoryControl.Data.Entities;
using InventoryControl.Models;
using AutoMapper;

namespace InventoryControl.Mapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        
    }
}