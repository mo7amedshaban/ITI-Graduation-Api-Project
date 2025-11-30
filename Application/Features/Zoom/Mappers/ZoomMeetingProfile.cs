using Application.Features.Zoom.Commands.CreateZoomMeeting;
using AutoMapper;
using Core.DTOs;
using Core.Entities.Zoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Zoom.Mappers
{
    public class ZoomMeetingProfile : Profile
    {
        
        public ZoomMeetingProfile() {

            // Command → Request
            CreateMap<CreateZoomMeetingCommand, ZoomMeetingRequest>()
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                // Map other properties as needed
                ;

            // Response → Entity
            CreateMap<ZoomMeetingResponse, ZoomMeeting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                //.ForMember(dest => dest.HostId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.JoinUrl, opt => opt.MapFrom(src => src.JoinUrl))
                // Map other properties as needed
                ;

            // Entity → Command (separate mapping, avoid ReverseMap for complex scenarios)
            CreateMap<ZoomMeeting, CreateZoomMeetingCommand>()
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                // Map other properties as needed
                ;

            // Response → ApiResponse
            CreateMap<ZoomMeetingResponse, ZoomMeetingApiResponse>();
            CreateMap<ZoomMeetingApiResponse, ZoomMeetingResponse>();




        }
    }
}
