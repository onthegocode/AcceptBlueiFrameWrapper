        //Used to Verify and save nonce token by returning a longer term token
	public SourceVerificationResponse SourceVerification(SourceVerificationRequest verifyAndSaveRequest)
	{
            string txtContent = JsonSerializer.Serialize(verifyAndSaveRequest, options);
            var endpoint = Config.AcceptBlue.Paths.Verify; //endpoint for the verify api
            var payload = new StringContent(txtContent, Encoding.UTF8, "application/json");
            SourceVerificationResponse sourceVerificationResponse = PostAsync<SourceVerificationResponse>(endpoint, payload); //calls the api / sends request
            if (!string.IsNullOrWhiteSpace(sourceVerificationResponse.Error_Code))
            {
                throw new Exception(sourceVerificationResponse.Error_Message);
            }
            if (sourceVerificationResponse.Status_Code != "A")
            {
                throw new Exception(sourceVerificationResponse.Status);
            }
            return sourceVerificationResponse; //returns the response given to us by the Accept.blue api
        }

	//Used to Charge card based on saved Token
	public SourceChargeResponse SourceCharge(SourceChargeRequest sourceChargeRequest)
	{
		string txtContent = JsonSerializer.Serialize(sourceChargeRequest, options);
		var endpoint = Config.AcceptBlue.Paths.Charge; //endpoint for the charge api
		var payload = new StringContent(txtContent, Encoding.UTF8, "application/json");
		SourceChargeResponse sourceChargeResponse = PostAsync<SourceChargeResponse>(endpoint, payload); //calls the api / sends request
		if (!string.IsNullOrWhiteSpace(sourceChargeResponse.Error_Code))
		{
			throw new Exception(sourceChargeResponse.Error_Message);
		}
		if (sourceChargeResponse.Status_Code != "A")
		{
			throw new Exception(sourceChargeResponse.Status);
		}
		return sourceChargeResponse; //returns the response given to us by the Accept.blue api
	}
