using InventoryControl.Data.Entities;
using InventoryControl.Models;
using AutoMapper;

namespace InventoryControl.Mapper.Profile;

public class UserProfile : AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
    }
}
            
            