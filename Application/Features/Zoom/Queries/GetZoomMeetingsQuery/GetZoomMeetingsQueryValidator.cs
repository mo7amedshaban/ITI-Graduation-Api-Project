using FluentValidation;
using System;


namespace Application.Features.Zoom.Queries.GetZoomMeetingsQuery
{
   

    public class GetZoomMeetingsQueryValidator : AbstractValidator<GetZoomMeetingsQuery>
    {
        public GetZoomMeetingsQueryValidator()
        {
            RuleFor(x => x.meetingId)
                .NotEmpty().WithMessage("UserId is required.");
                
        }

       
    }

}
