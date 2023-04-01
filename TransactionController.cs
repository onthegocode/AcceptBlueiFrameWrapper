        //Source Verification
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<VerificationResponse>> SourceVerification([FromBody()] VerificationRequest verificationRequest = null)
        {
            VerificationResponse verificationResponse = null; //used as a default

            try
            {
		//Creates the request that will be later converted to JSON and used to call the Accept.blue api
		Models.AcceptBlue.SourceVerificationRequest sourceVerificationRequest = new Models.AcceptBlue.SourceVerificationRequest
		{
			Avs_Address = string.Empty,
			Avs_Zip = string.Empty,
			Name = "bob",
			Software = string.Empty,
			Save_Card = true,
			Source = verificationRequest.Source,
			Expiry_Month = verificationRequest.Expiry_Month,
			Expiry_Year = verificationRequest.Expiry_Year,
		};

		Models.AcceptBlue.SourceVerificationResponse sourceVerificationResponse = AcceptBlue.SourceVerification(sourceVerificationRequest); //calls the SourceVerification Method in the AcceptBlueService

		//Captures the response given to us by the SourceVerification method 
		verificationResponse = new VerificationResponse
		{
			Status = sourceVerificationResponse.Status,
			Error_Message = sourceVerificationResponse.Error_Message,
			Error_Details = sourceVerificationResponse.Error_Details,
			Card_Type = sourceVerificationResponse.Card_Type,
			Last_4 = sourceVerificationResponse.Last_4,
			Card_Ref = sourceVerificationResponse.Card_Ref,
		};

		// (To-do): Save the Token in the customer profile *

		await DbContext.SaveChangesAsync();
	}
	catch (Exception ex)
	{
		return BadRequest(new { status = "Error", message = ex.Message });
	}

	return Ok(verificationResponse);
        }

        //Source Charge
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ChargeResponse>> SourceCharge([FromBody()] ChargeRequest chargeRequest = null)
        {
		ChargeResponse chargeResponse = null; //used as a default

		try
		    {
			//(To-do): Use Token from saved customer profile to charge

			//Creates the request that will be later converted to JSON and used to call the Accept.blue api
			Models.AcceptBlue.SourceChargeRequest sourceChargeRequest = new Models.AcceptBlue.SourceChargeRequest
			{
				Amount = 35.63f,
				Avs_Address = string.Empty,
				Avs_Zip = string.Empty,
				Name = "bob",
				CVV2 = "123",
				Expiry_Month = DateTime.Now.Month + 1,
				Expiry_Year = DateTime.Now.Year + 1,
				Source = "tkn-" + chargeRequest.Source, //make it customer profile token
				Software = string.Empty,
				Capture = false,
				Save_Card = false,

			};

			Models.AcceptBlue.SourceChargeResponse sourceChargeResponse = AcceptBlue.SourceCharge(sourceChargeRequest); //calls the SourceCharge Method in the AcceptBlueService

			//Captures the response given to us by the SourceCharge Method
			chargeResponse = new ChargeResponse
			{
				Status = sourceChargeResponse.Status,
				Status_Code = sourceChargeResponse.Status_Code,
				Error_Message = sourceChargeResponse.Error_Message,
				Error_Code = sourceChargeResponse.Error_Code,
				Error_Details = sourceChargeResponse.Error_Details,
				Auth_Amount = sourceChargeResponse.Auth_Amount,
				Auth_Code = sourceChargeResponse.Auth_Code,
				Reference_Number = sourceChargeResponse.Reference_Number,
				Card_Type = sourceChargeResponse.Card_Type,
				Last_4 = sourceChargeResponse.Last_4,
				Card_Ref = sourceChargeResponse.Card_Ref,
			};

			// (To-do): Record the Charge in the transactions table *


			await DbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = "Error", message = ex.Message });
		}


		return Ok(chargeResponse);
        }

    public class VerificationRequest
    {
        public string Source { get; set; }
        public int Expiry_Month { get; set; }
        public int Expiry_Year { get; set; }
    }
    public class VerificationResponse
    {
	public string Status { get; set; }
	public string Error_Message { get; set; }
	public string Error_Details { get; set; }
	public string Card_Type { get; set; }
	public string Last_4 { get; set; }
	public string Card_Ref { get; set; }
    }
    public class ChargeRequest
    {
        public string Source { get; set; }
    }
    public class ChargeResponse
    {
	public string Status { get; set; }
	public string Status_Code { get; set; }
	public string Error_Message { get; set; }
	public string Error_Code { get; set; }
	public string Error_Details { get; set; }
	public float Auth_Amount { get; set; }
	public string Auth_Code { get; set; }
	public int Reference_Number { get; set; }
	public string Card_Type { get; set; }
	public string Last_4 { get; set; }
	public string Card_Ref { get; set; }
}
