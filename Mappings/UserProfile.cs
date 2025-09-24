
using AutoMapper;
using UsersCRUD.Models;
using UsersCRUD.DTOs.User;

namespace UsersCRUD.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDTO>();
        CreateMap<CreateUserDTO, User>().ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}