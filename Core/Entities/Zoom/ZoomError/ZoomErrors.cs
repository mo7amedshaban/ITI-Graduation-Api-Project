
using Core.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ZoomServices.Errors
{
    public static class ZoomErrors
    {
        public static readonly Error MeetingNotFound = Error.NotFound(
            "Zoom.MeetingNotFound",
            "Zoom meeting not found or access denied");

        public static readonly Error InvalidMeetingData = Error.Validation(
            "Zoom.InvalidMeetingData",
            "Invalid meeting data provided");

        public static readonly Error ZoomApiUnavailable = Error.Failure(
            "Zoom.ApiUnavailable",
            "Zoom API is temporarily unavailable");

        public static readonly Error AuthenticationFailed = Error.Unauthorized(
            "Zoom.AuthenticationFailed",
            "Failed to authenticate with Zoom API");

        public static readonly Error RateLimitExceeded = Error.Conflict(
            "Zoom.RateLimitExceeded",
            "Zoom API rate limit exceeded");

        public static readonly Error RecordingNotFound = Error.NotFound(
            "Zoom.RecordingNotFound",
            "Recording not found for the specified meeting");

        public static Error ApiError(string details) => Error.Failure(
            "Zoom.ApiError",
            $"Zoom API error: {details}");

        public static Error HttpError(HttpStatusCode statusCode) => Error.Failure(
            "Zoom.HttpError",
            $"Zoom API returned HTTP {(int)statusCode}: {statusCode}");
    }




}
