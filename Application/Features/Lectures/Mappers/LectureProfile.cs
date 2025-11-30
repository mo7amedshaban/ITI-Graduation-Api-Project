using Application.Features.Lectures.Dtos;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Mappers
{
    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            //CreateMap<Lecture, LectureDto>()
            //.ForMember(dest => dest.ZoomMeeting, opt => opt.MapFrom(src => src.ZoomMeeting))
            //.ForMember(dest => dest.ZoomRecording, opt => opt.MapFrom(src => src.ZoomRecording));

            //CreateMap<ZoomMeeting, ZoomMeetingDto>()
            //    .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic))
            //    .ForMember(dest => dest.JoinUrl, opt => opt.MapFrom(src => src.JoinUrl))
            //    .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime));
            CreateMap<Lecture, LectureDto>()
            .ForMember(dest => dest.ZoomMeeting, opt => opt.MapFrom(src => src.ZoomMeeting))
            .ForMember(dest => dest.ZoomRecording, opt => opt.MapFrom(src => src.ZoomRecording));
         

            CreateMap<ZoomRecording, ZoomRecordingDto>()
                .ForMember(dest => dest.RecordingUrl, opt => opt.MapFrom(src => src.FileUrl));

            CreateMap<ZoomRecording, ZoomRecordingDto>();


            CreateMap<ZoomMeeting, ZoomMeetingDto>()
         .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartTime, DateTimeKind.Utc)));

        }
    }
}
