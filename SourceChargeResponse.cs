using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SO_Virtual_Terminal.Models.AcceptBlue
{
	public class SourceChargeResponse
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
		[JsonPropertyName("auth_amount")]
		public float Auth_Amount { get; set; }
		[JsonPropertyName("auth_code")]
		public string Auth_Code { get; set; }
		[JsonPropertyName("reference_number")]
		public int Reference_Number { get; set; }
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
