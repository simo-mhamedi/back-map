using AutoMapper;
using back_map.Auth.Data.Dto;
using back_map.Entity;
using back_map.Entity.Dto;
using JwtWebApiTutorial;

namespace back_map.Business.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterRequest, User>()
                   .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());


            CreateMap<User, RegisterRequest>()
                     .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(ps => ps.MediaFile.MediaUrl));
           
            
            CreateMap<RegisterRequestClient, User>();
            CreateMap<MoreInfo, MoreInfoDto>();
            CreateMap<MoreInfoDto, MoreInfo>();
            CreateMap<AnnouncementDto, Announcement>();
            CreateMap<Announcement, AnnouncementDto>()
       .ForMember(dest => dest.City, opt => opt.MapFrom(ps => ps.Address != null ? ps.Address.City : null))
       .ForMember(dest => dest.Country, opt => opt.MapFrom(ps => ps.Address != null ? ps.Address.Country : null))
       .ForMember(dest => dest.Latitude, opt => opt.MapFrom(ps => ps.Address != null ? ps.Address.Latitude : 0))
       .ForMember(dest => dest.UserPhoto, opt => opt.MapFrom(ps => ps.User.MediaFile.MediaUrl))
       .ForMember(dest => dest.UserName, opt => opt.MapFrom(ps => ps.User.Username))
       .ForMember(dest => dest.Longitude, opt => opt.MapFrom(ps => ps.Address != null ? ps.Address.Longitude : 0))
       .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(ps => ps.Address != null ? ps.Address.FullAddress : null));


            CreateMap<MediaFile, MediaFileDto>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
