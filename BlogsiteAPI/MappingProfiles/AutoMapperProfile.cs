using AutoMapper;
using BlogsiteAPI.DataTransferObjects;
using BlogsiteDomain.Entities.Account;

namespace BlogsiteAPI.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUserSignupDTO, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress));
        }
    }
}