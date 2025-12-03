
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{

    public class ZoomMeetingResponse
    {

        [JsonProperty("uuid")]
        public string Uuid { get; set; } = string.Empty;

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("host_id")]
        public string HostId { get; set; } = string.Empty;

        [JsonProperty("topic")]
        public string Topic { get; set; } = string.Empty;

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("start_time")]
        public string StartTime { get; set; } = string.Empty;

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; } = string.Empty;

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonProperty("start_url")]
        public string StartUrl { get; set; } = string.Empty;

        [JsonProperty("join_url")]
        public string JoinUrl { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;




        public ZoomMeetingResponse()
        {

        }
    }

    public class ZoomMeetingRequest
    {

       
        public string userId { get; set; }
        public string Topic { get; set; }
        public DateTime StartTime { get; set; }  
        public int Duration { get; set; }    
        public string Timezone { get; set; }
        public string Agenda { get; set; }
        public string AutoRecording { get; set; } 
        public bool PreSchedule { get; set; }

        public Guid HostId { get; set; }
    }


    public record ZoomRecordingResponse(
        string Id,
        string MeetingId,
        string DownloadUrl,
        string FileType,
        long FileSize,
        int Duration,
        DateTime RecordingStart,
        DateTime RecordingEnd);
    public class ZoomListMeetingsResponse
    {
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public List<ZoomMeetingItem> Meetings { get; set; } = new();
    }

    public class ZoomMeetingItem
    {
        public string Uuid { get; set; } = string.Empty;
        public long Id { get; set; }                 // Meeting Id
        public string HostId { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public int Type { get; set; }                // 1 = Instant, 2 = Scheduled etc.
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }            // in minutes
        public string Timezone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string JoinUrl { get; set; } = string.Empty;
    }


    //API Response DTOs
    public class ZoomMeetingApiResponse
    {
        public long Id { get; set; }
        public string Topic { get; set; } = default!;
        public string StartTime { get; set; } = default!;
        public int Duration { get; set; }
        public string JoinUrl { get; set; } = default!;
        public string StartUrl { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Status { get; set; } = default!;
    }

    public class ZoomRecordingsApiResponse
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("recording_files")]
        public List<RecordingFile> RecordingFiles { get; set; } = new();

        public class RecordingFile
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("recording_start")]
            public string RecordingStart { get; set; }

            [JsonPropertyName("recording_end")]
            public string RecordingEnd { get; set; }

            [JsonPropertyName("file_type")]
            public string FileType { get; set; }

            [JsonPropertyName("file_size")]
            public long FileSize { get; set; }

            [JsonPropertyName("download_url")]
            public string DownloadUrl { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("duration")]
            public int Duration { get; set; }
        }
    }



    public class ZoomTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;
    }
}
