using Application.Features.Lectures.Dtos;
using AutoMapper;
using Core.Entities.Zoom;
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
           
            CreateMap<Core.Entities.Courses.Lecture, LectureDto>()
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
