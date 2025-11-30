using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{ 
    public class ZoomWebhookPayload
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = default!;

        [JsonPropertyName("payload")]
        public ZoomWebhookPayloadData Payload { get; set; } = default!;
    }

    public class ZoomWebhookPayloadData
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; } = default!;

        [JsonPropertyName("object")]
        public ZoomWebhookObject Object { get; set; } = default!;
    }

    public class ZoomWebhookObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; } = default!;

        [JsonPropertyName("topic")]
        public string Topic { get; set; } = default!;

        [JsonPropertyName("host_id")]
        public string HostId { get; set; } = default!;

        [JsonPropertyName("recording_files")]
        public List<ZoomRecordingFile> RecordingFiles { get; set; } = new();
    }

    public class ZoomRecordingFile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("recording_start")]
        public DateTime RecordingStart { get; set; }

        [JsonPropertyName("recording_end")]
        public DateTime RecordingEnd { get; set; }

        [JsonPropertyName("file_type")]
        public string FileType { get; set; } = default!;

        [JsonPropertyName("file_size")]
        public long FileSize { get; set; }

        [JsonPropertyName("download_url")]
        public string DownloadUrl { get; set; } = default!;
    }

}
