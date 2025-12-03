using Core.Common.Results;
using Core.DTOs;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Zoom.Queries.GetZoomMeetingsQuery
{
    public record GetZoomMeetingsQuery(long meetingId) : IRequest<Result<List<ZoomMeetingResponse>>>;
 
       

}
