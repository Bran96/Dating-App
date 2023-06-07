using AutoMapper;
using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Extensions;

namespace DatingAppApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        // The idea behind AutoMapper is, is that it takes away the writing of tedious code from us.
        // We need to tell AutoMapper what we want to go from and what we want to go to in the constructor
        public AutoMapperProfiles()
        {
            // Create a Map here. First where we want to go from and where we want to go to
            // The idea of AutoMapper is, that its gonna compare the properties by their name and their type
            CreateMap<AppUser, MemberDto>()
                .ForMember(destination => destination.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(destination => destination.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
        }
    }
}
