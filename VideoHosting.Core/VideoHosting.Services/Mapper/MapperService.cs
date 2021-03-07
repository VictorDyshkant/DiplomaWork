using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using VideoHosting.Abstractions.Dto;
using VideoHosting.Domain.Entities;

namespace VideoHosting.Services.Mapper
{
    public class MapperService : Profile
    {
        public MapperService(IConfiguration configuration)
        {
            CreateMap<User, UserDto>()
               .ForMember(c => c.Id, x => x.MapFrom(c => c.Id))
               .ForMember(c => c.Name, x => x.MapFrom(c => c.Name))
               .ForMember(c => c.Surname, x => x.MapFrom(c => c.Surname))
               .ForMember(c => c.Sex, x => x.MapFrom(c => c.Sex))
               .ForMember(c => c.Group, x => x.MapFrom(c => c.Group))
               .ForMember(c => c.Faculty, x => x.MapFrom(c => c.Faculty))
               .ForMember(c => c.DateOfCreation, x => x.MapFrom(c => c.DateOfCreation))
               .ForMember(c => c.Subscribers, x => x.MapFrom(c => c.Subscribers.Count))
               .ForMember(c => c.Subscriptions, x => x.MapFrom(c => c.Subscriptions.Count))
               .ForMember(c => c.PhotoPath, x => x.MapFrom(c => c.PhotoPath != null ? c.PhotoPath + configuration.GetSection("Settings:UserPhoto").Value : null));

            CreateMap<UserDto, User>()
                .ForMember(c => c.Name, x => x.MapFrom(c => c.Name))
                .ForMember(c => c.Surname, x => x.MapFrom(c => c.Surname))
                .ForMember(c => c.Sex, x => x.MapFrom(c => c.Sex))
                .ForMember(c => c.Group, x => x.MapFrom(c => c.Group))
                .ForMember(c => c.Faculty, x => x.MapFrom(c => c.Faculty))
                .ForMember(c => c.DateOfCreation, x => x.MapFrom(c => DateTime.Now))
                .ForMember(c => c.Subscribers, x => x.MapFrom(c => new List<UserUser>()))
                .ForMember(c => c.Subscriptions, x => x.MapFrom(c => new List<UserUser>()));

            CreateMap<User, UserLoginDto>()
                .ForMember(c => c.Id, x => x.MapFrom(c => c.Id))
                .ForMember(c => c.Email, x => x.MapFrom(c => c.Email))
                .ForMember(c => c.PhoneNumber, x => x.MapFrom(c => c.PhoneNumber));


            CreateMap<Commentary, CommentaryDto>()
                .ForMember(c => c.Id, x => x.MapFrom(p => p.Id))
                .ForMember(c => c.Content, x => x.MapFrom(p => p.Content))
                .ForMember(c => c.DayOfCreation, x => x.MapFrom(p => p.DayOfCreation))
                .ForMember(c => c.UserId, x => x.MapFrom(p => p.User.Id))
                .ForMember(c => c.VideoId, x => x.MapFrom(p => p.Video.Id))
                .ReverseMap();

            CreateMap<Video, VideoDto>()
                .ForMember(v => v.Id, x => x.MapFrom(p => p.Id))
                .ForMember(v => v.Name, x => x.MapFrom(p => p.Name))
                .ForMember(v => v.UserId, x => x.MapFrom(p => p.User.Id))
                .ForMember(v => v.PhotoPath, x => x.MapFrom(p => p.PhotoPath != null ? p.PhotoPath + configuration.GetSection("Settings:VideoPhoto").Value : null))
                .ForMember(v => v.VideoPath, x => x.MapFrom(p => p.VideoPath != null ? p.PhotoPath + configuration.GetSection("Settings:Video").Value : null))
                .ForMember(v => v.Views, x => x.MapFrom(p => p.Views))
                .ForMember(v => v.Likes, x => x.MapFrom(p => p.Likes.Count))
                .ForMember(v => v.Dislikes, x => x.MapFrom(p => p.Dislikes.Count))
                .ForMember(v => v.DayOfCreation, x => x.MapFrom(p => p.DayOfCreation))
                .ReverseMap(); ;

        }
    }
}
