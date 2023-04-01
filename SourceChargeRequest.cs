using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SO_Virtual_Terminal.Models.AcceptBlue
{
	public class SourceChargeRequest
	{
		[JsonPropertyName("amount")]
		public float Amount { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("ignore_duplicates")]
		public bool Ignore_Duplicates { get; set; }
		[JsonPropertyName("software")]
		public string Software { get; set; }
		[JsonPropertyName("avs_address")]
		public string Avs_Address { get; set; }
		[JsonPropertyName("avs_zip")]
		public string Avs_Zip { get; set; }
		/// <summary>Used to preset source charge credit card expiry month by 1 month ahead of the current. Example: 10 => 11</summary>
		[JsonPropertyName("expiry_month")]
		public int Expiry_Month { get; set; } = DateTime.Now.Month + 1;
		/// <summary>Used to preset source charge credit card expiry year by 1 year ahead of the current. Example: 2022 => 2023</summary>
		[JsonPropertyName("expiry_year")]
		public int Expiry_Year { get; set; } = DateTime.Now.Year + 1;
		/// <summary>Used to preset source charge credit cvv as its not needed for a source charge using a token</summary>
		[JsonPropertyName("cvv2")]
		public string CVV2 { get; set; } = "123";
		[JsonPropertyName("source")]
		public string Source { get; set; }
		[JsonPropertyName("capture")]
		public bool Capture { get; set; }
		[JsonPropertyName("save_card")]
		public bool Save_Card { get; set; }
	}

	public class Amount_Details
	{
		[JsonPropertyName("tax")]
		public int Tax { get; set; }
		[JsonPropertyName("surcharge")]
		public int Surcharge { get; set; }
		[JsonPropertyName("shipping")]
		public int Shipping { get; set; }
		[JsonPropertyName("tip")]
		public int Tip { get; set; }
		[JsonPropertyName("discount")]
		public int Discount { get; set; }
	}


	public class Billing_Info
	{
		[JsonPropertyName("first_name")]
		public string First_Name { get; set; }
		[JsonPropertyName("last_name")]
		public string Last_Name { get; set; }
		[JsonPropertyName("street")]
		public string Street { get; set; }
		[JsonPropertyName("street2")]
		public string Street2 { get; set; }
		[JsonPropertyName("state")]
		public string State { get; set; }
		[JsonPropertyName("city")]
		public string City { get; set; }
		[JsonPropertyName("zip")]
		public string Zip { get; set; }
		[JsonPropertyName("country")]
		public string Country { get; set; }
		[JsonPropertyName("phone")]
		public string Phone { get; set; }
	}

	public class Shipping_Info
	{
		[JsonPropertyName("first_name")]
		public string First_Name { get; set; }
		[JsonPropertyName("last_name")]
		public string Last_Name { get; set; }
		[JsonPropertyName("Street")]
		public string Street { get; set; }
		[JsonPropertyName("street2")]
		public string Street2 { get; set; }
		[JsonPropertyName("state")]
		public string State { get; set; }
		[JsonPropertyName("city")]
		public string City { get; set; }
		[JsonPropertyName("zip")]
		public string Zip { get; set; }
		[JsonPropertyName("country")]
		public string Country { get; set; }
		[JsonPropertyName("phone")]
		public string Phone { get; set; }
	}

	public class Custom_Fields
	{
		[JsonPropertyName("custom1")]
		public string Custom1 { get; set; }
		[JsonPropertyName("custom2")]
		public string Custom2 { get; set; }
		[JsonPropertyName("custom3")]
		public string Custom3 { get; set; }
		[JsonPropertyName("custom4")]
		public string Custom4 { get; set; }
		[JsonPropertyName("custom5")]
		public string Custom5 { get; set; }
		[JsonPropertyName("custom6")]
		public string Custom6 { get; set; }
		[JsonPropertyName("custom7")]
		public string Custom7 { get; set; }
		[JsonPropertyName("custom8")]
		public string Custom8 { get; set; }
		[JsonPropertyName("custom9")]
		public string Custom9 { get; set; }
		[JsonPropertyName("custom10")]
		public string Custom10 { get; set; }
		[JsonPropertyName("custom11")]
		public string Custom11 { get; set; }
		[JsonPropertyName("custom12")]
		public string Custom12 { get; set; }
		[JsonPropertyName("custom13")]
		public string Custom13 { get; set; }
		[JsonPropertyName("custom14")]
		public string Custom14 { get; set; }
		[JsonPropertyName("custom15")]
		public string Custom15 { get; set; }
		[JsonPropertyName("custom16")]
		public string Custom16 { get; set; }
		[JsonPropertyName("custom17")]
		public string Custom17 { get; set; }
		[JsonPropertyName("custom18")]
		public string Custom18 { get; set; }
		[JsonPropertyName("custom19")]
		public string Custom19 { get; set; }
		[JsonPropertyName("custom20")]
		public string Custom20 { get; set; }
	}

	public class Customer
	{
		[JsonPropertyName("send_receipt")]
		public bool Send_Receipt { get; set; }
		[JsonPropertyName("email")]
		public string Email { get; set; }
		[JsonPropertyName("fax")]
		public string Fax { get; set; }
		[JsonPropertyName("identifier")]
		public string Identifier { get; set; }
		[JsonPropertyName("customer_id")]
		public int Customer_Id { get; set; }
	}

}
