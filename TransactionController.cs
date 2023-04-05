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
                    Name = !(verificationRequest.Name is null) ? verificationRequest.Name : "",
                    Avs_Address = !(verificationRequest.Avs_Address is null) ? verificationRequest.Avs_Address : "",
		    Avs_Zip = !(verificationRequest.Avs_Zip is null) ? verificationRequest.Avs_Zip : "",
		    Software = !(verificationRequest.Software is null) ? verificationRequest.Software : "",
		    Save_Card = true, //must be true to save
		    Source = verificationRequest.Source, // required
		    Expiry_Month = verificationRequest.Expiry_Month, // required
		    Expiry_Year = verificationRequest.Expiry_Year, // required
		};

		Models.AcceptBlue.SourceVerificationResponse sourceVerificationResponse = AcceptBlue.SourceVerification(sourceVerificationRequest); //calls the SourceVerification Method in the AcceptBlueService

		//Captures the response given to us by the SourceVerification method 
		verificationResponse = new VerificationResponse
		{
			Status = sourceVerificationResponse.Status,
			Token = sourceVerificationResponse.Card_Ref,
			Card_Type = sourceVerificationResponse.Card_Type,
			Last_4 = sourceVerificationResponse.Last_4,
			Expiry_Month = verificationRequest.Expiry_Month, //used to return the date then entered
			Expiry_Year = verificationRequest.Expiry_Year, //used to return the date then entered
			Name = verificationRequest.Name,
			Avs_Address = verificationRequest.Avs_Address,
			Avs_Zip = verificationRequest.Avs_Zip,
			Software = verificationRequest.Software,
			Error_Message = sourceVerificationResponse.Error_Message,
			Error_Details = sourceVerificationResponse.Error_Details,
		};

		// (To-do): Save the Token in the customer profile *

		await DbContext.SaveChangesAsync();
	}
	catch (Exception ex)
	{
		return BadRequest(new { status = "Error", message = ex.Message });
	}

	return Ok(verificationResponse); //returns the response to the success in ajax
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
	Amount = chargeRequest.Amount, // required
	Avs_Address = !(chargeRequest.Avs_Address is null) ? chargeRequest.Avs_Address : "", //allow for the field to be used but default to null or string.empty
	Avs_Zip = !(chargeRequest.Avs_Zip is null) ? chargeRequest.Avs_Zip : "",//allow for the field to be used but default to null or string.empty
	CVV2 = !(chargeRequest.CVV2 is null) ? chargeRequest.CVV2 : "123",
	Expiry_Month = chargeRequest.Expiry_Month != 0 ? chargeRequest.Expiry_Month : DateTime.Now.Month + 1,
	Expiry_Year = chargeRequest.Expiry_Year != 0 ? chargeRequest.Expiry_Year : DateTime.Now.Year + 1,
	Source = "tkn-" + chargeRequest.Source, //make it customer profile token -> required
	Software = !(chargeRequest.Software is null) ? chargeRequest.Software : "",//allow for the field to be used but default to null or string.empty
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
		Token = sourceChargeResponse.Card_Ref,
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
        public string Name { get; set; }
        public string Software { get; set; }
	public string Avs_Address { get; set; }
        public string Avs_Zip { get; set; }
	public int Expiry_Month { get; set; }
        public int Expiry_Year { get; set; }
    }
    public class VerificationResponse
    {
	public string Status { get; set; }
	public string Token { get; set; }
	public string Card_Type { get; set; }
	public string Last_4 { get; set; }
	public int Expiry_Month { get; set; }
	public int Expiry_Year { get; set; }
        public string Name { get; set; }
        public string Avs_Address { get; set; }
        public string Avs_Zip { get; set; }
        public string Software { get; set; }
	public string Error_Message { get; set; }
	public string Error_Details { get; set; }

	}
    public class ChargeRequest
    {
        public decimal Amount { get; set; }
        public string Avs_Address { get; set; }
        public string Avs_Zip { get; set; }
        public int Expiry_Month { get; set; }
        public int Expiry_Year { get; set; }
        public string Software { get; set; }
	public string CVV2 { get; set; }
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
	public string Token { get; set; }
}
