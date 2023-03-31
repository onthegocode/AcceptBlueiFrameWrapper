using System;
using System.Text.Json.Serialization;

namespace SO_Virtual_Terminal.Models.AcceptBlue
{

    public class SourceVerificationResponse
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("status_code")]
        public string Status_Code { get; set; }
        [JsonPropertyName("error_message")]
        public string Error_Message { get; set; }
        [JsonPropertyName("error_code")]
        public string Error_Code { get; set; }
        [JsonPropertyName("error_details")]
        public string Error_Details { get; set; }
        [JsonPropertyName("avs_result")]
        public string Avs_Result { get; set; }
        [JsonPropertyName("avs_result_code")]
        public string Avs_Result_Code { get; set; }
        [JsonPropertyName("cvv2_result")]
        public string CVV2_Result { get; set; }
        [JsonPropertyName("cvv2_result_code")]
        public string CVV2_Result_Code { get; set; }
        [JsonPropertyName("card_type")]
        public string Card_Type { get; set; }
        [JsonPropertyName("last_4")]
        public string Last_4 { get; set; }
        [JsonPropertyName("card_ref")]
        public string Card_Ref { get; set; }
    }
}
