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

	public class Transaction_Details
	{
		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("clerk")]
		public string Clerk { get; set; }
		[JsonPropertyName("terminal")]
		public string Terminal { get; set; }
		[JsonPropertyName("client_ip")]
		public string Client_IP { get; set; }
		[JsonPropertyName("signature")]
		public string Signature { get; set; }
	}

	public class Transaction_Flags
	{
		[JsonPropertyName("allow_partial_approval")]
		public bool Allow_Partial_Approval { get; set; }
		[JsonPropertyName("is_recurring")]
		public bool Is_Recurring { get; set; }
		[JsonPropertyName("is_installment")]
		public bool Is_Installment { get; set; }
		[JsonPropertyName("is_customer_initiated")]
		public bool Is_Customer_Initiated { get; set; }
		[JsonPropertyName("cardholder_present")]
		public bool Cardholder_Present { get; set; }
		[JsonPropertyName("card_present")]
		public bool Card_Present { get; set; }
	}

	public class Terminal
	{
	}

}
