
using Core.DTOs;
using Core.Entities.Zoom;
using Core.Interfaces.Services;
using Infrastructure.Helper;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ZoomServices
{
    public class ZoomService : IZoomService
    {
        private readonly IZoomAuthService _authService;
     

        public ZoomService(
            IZoomAuthService authService,
            IHttpClientFactory httpClientFactory,
            IOptions<ZoomSettings> settings,
            ILogger<ZoomService> logger)
        {
            _authService = authService;
      
        }

        public async Task<ZoomMeetingResponse> CreateMeetingAsync(
            ZoomMeetingRequest meeting, 
            CancellationToken ct)
        {

            var client = new HttpClient();
            var token = await _authService.GetAccessTokenAsync(ct);
            var request_ = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/v2/users/me/meetings");
            request_.Headers.Add("Accept", "application/json");
            request_.Headers.Add("Authorization", $"Bearer {token}");
            var meetingTime = meeting.StartTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var meetingData = new
            {
                topic = "",
                agenda = "",
                default_password = false,
                settings = new
                {
                    auto_recording = meeting.AutoRecording.ToString().ToLower(),
                    join_before_host = true,
                    jbh_time = 0
                },
                use_pmi = false,
                meeting_authentication = false,
                timezone = "Africa/Cairo",
                start_time = meetingTime,
           

            };

            var jsonString = JsonConvert.SerializeObject(meetingData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request_.Content = content;
            var response = await client.SendAsync(request_);
            var res = await response.Content.ReadAsStringAsync();



            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Zoom API error: {response.StatusCode} - {res}");
            }

            var zoomResponse = JsonConvert.DeserializeObject<ZoomMeetingResponse>(res);
            return zoomResponse!;

        

        }


        public async Task<ZoomMeeting> ScheduleMeetingAsync(
                    Guid instructorId,
                    ZoomMeetingRequest request,
                    CancellationToken ct)
        {
            
            var token = await _authService.GetAccessTokenAsync(ct);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        
            var meetingData = new
            {
                topic = request.Topic ?? "No Topic",
                type = 2,
                start_time = request.StartTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                duration = request.Duration > 0 ? request.Duration : 60,
                timezone = request.Timezone ?? "Africa/Cairo",
                agenda = request.Agenda ?? string.Empty,
                settings = new
                {
                    host_video = true,
                    participant_video = true,
                    join_before_host = false,
                    auto_recording = request.AutoRecording,
                    waiting_room = true
                }
            };

            var json = JsonConvert.SerializeObject(meetingData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

          
            var response = await client.PostAsync("https://api.zoom.us/v2/users/me/meetings", content, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Zoom API error: {response.StatusCode} - {responseBody}");
            }

            var zoomResponse = JsonConvert.DeserializeObject<ZoomMeetingApiResponse>(responseBody);

            if (zoomResponse == null)
                throw new Exception("Failed to parse Zoom meeting response");

            
            var meeting = new ZoomMeeting
            {
                ZoomMeetingId = zoomResponse.Id,
                //InstructorId = instructorId,
                Topic = zoomResponse.Topic ?? string.Empty,
                StartTime = DateTime.Parse(zoomResponse.StartTime),
                Duration = zoomResponse.Duration > 0 ? zoomResponse.Duration : 60,
                JoinUrl = zoomResponse.JoinUrl ?? string.Empty,
                StartUrl = zoomResponse.StartUrl ?? string.Empty,
                Password = zoomResponse.Password ?? string.Empty,
                Status = zoomResponse.Status ?? "waiting"
            };

         
            //_dbContext.ZoomMeetings.Add(meeting);
            //await _dbContext.SaveChangesAsync(ct);

            return meeting;
        }


        public async Task<ZoomMeeting> GetMeetingAsync(long zoomMeetingId, CancellationToken ct)
        {
            
            var token = await _authService.GetAccessTokenAsync(ct);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // 2️⃣ Call Zoom API to get the meeting
            var url = $"https://api.zoom.us/v2/meetings/{zoomMeetingId}";
            var response = await client.GetAsync(url, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Zoom API error: {response.StatusCode} - {responseBody}");
            }

            var zoomResponse = JsonConvert.DeserializeObject<ZoomMeetingApiResponse>(responseBody);
            if (zoomResponse == null)
                throw new Exception("Failed to parse Zoom meeting response");

         
            var meeting = new ZoomMeeting
            {
                ZoomMeetingId = zoomResponse.Id,
                Topic = zoomResponse.Topic ?? string.Empty,
                StartTime = DateTime.Parse(zoomResponse.StartTime),
                Duration = zoomResponse.Duration > 0 ? zoomResponse.Duration : 60,
                JoinUrl = zoomResponse.JoinUrl ?? string.Empty,
                StartUrl = zoomResponse.StartUrl ?? string.Empty,
                Password = zoomResponse.Password ?? string.Empty,
                Status = zoomResponse.Status ?? "waiting"
            };

            return meeting;
        }



    }


}
