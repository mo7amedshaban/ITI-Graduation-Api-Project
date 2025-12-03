using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Zoom.Commands.RemoveZoomMeeting
{
    public class DeleteZoomMeetingCommandValidator : AbstractValidator<DeleteZoomMeetingCommand>
    {
        public DeleteZoomMeetingCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required")
                .MaximumLength(100).WithMessage("User ID cannot exceed 100 characters");

        }

     
    }
}
