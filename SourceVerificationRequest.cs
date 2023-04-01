using System.Text.Json.Serialization;

namespace SO_Virtual_Terminal.Models.AcceptBlue
{


	public class SourceVerificationRequest
	{

		[JsonPropertyName("avs_address")]
		public string Avs_Address { get; set; }
		[JsonPropertyName("avs_zip")]
		public string Avs_Zip { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("software")]
		public string Software { get; set; }
		[JsonPropertyName("save_card")]
		public bool Save_Card { get; set; }
		[JsonPropertyName("source")]
		public string Source { get; set; }
		[JsonPropertyName("expiry_month")]
		public int Expiry_Month { get; set; }
		[JsonPropertyName("expiry_year")]
		public int Expiry_Year { get; set; }
	}
}
