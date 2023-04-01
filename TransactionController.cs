//Source Verification
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<VerificationResponse>> SourceVerification([FromBody()] VerificationRequest verificationRequest = null)
        {
            VerificationResponse verificationResponse = null;

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
            Models.AcceptBlue.SourceVerificationResponse sourceVerificationResponse = AcceptBlue.SourceVerification(sourceVerificationRequest);

            verificationResponse = new VerificationResponse
            {
                Card_Ref = sourceVerificationResponse.Card_Ref,
            };

			// (To-do): Save the Token in the customer profile *

			await DbContext.SaveChangesAsync();

			return Ok(verificationResponse);
        }

        //Source Charge
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ChargeResponse>> SourceCharge([FromBody()] ChargeRequest chargeRequest = null)
        {

            //(To-do): Use Token from saved customer profile to charge

            ChargeResponse chargeResponse = null;
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

            Models.AcceptBlue.SourceChargeResponse sourceChargeResponse = AcceptBlue.SourceCharge(sourceChargeRequest);
			chargeResponse = new ChargeResponse
            {
                Status = sourceChargeResponse.Status,
            };

			// (To-do): Record the Charge in the transactions table *
            

			await DbContext.SaveChangesAsync();

			return Ok(chargeResponse);
        }
