        public CreditResponse Credit(CreditRequest creditRequest)
        {
            string txtContent = JsonSerializer.Serialize(creditRequest, options);
            CreditResponse creditResponse = PostAsync<CreditResponse>(Config.AcceptBlue.Paths.Credit, new StringContent(JsonSerializer.Serialize(creditRequest, options), Encoding.UTF8, "application/json"), "Unable to process the refund, please try again.");
            if (!string.IsNullOrWhiteSpace(creditResponse.Error_Code))
            {
                throw new Exception(creditResponse.Error_Message);
            }
            if (creditResponse.Status_Code != "A")
            {
                throw new Exception(creditResponse.Status);
            }
            return creditResponse;
        }
        
        //Used to Verify and save nonce token by returning a longer term token
		public SourceVerificationResponse SourceVerification(SourceVerificationRequest verifyAndSaveRequest)
		{
            string txtContent = JsonSerializer.Serialize(verifyAndSaveRequest, options);
            var endpoint = Config.AcceptBlue.Paths.Verify; //endpoint for the verify api
            var payload = new StringContent(txtContent, Encoding.UTF8, "application/json");
            SourceVerificationResponse sourceVerificationResponse = PostAsync<SourceVerificationResponse>(endpoint, payload, "Unable to verify! Please try again!"); //calls the api / sends request
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
			SourceChargeResponse sourceChargeResponse = PostAsync<SourceChargeResponse>(endpoint, payload, "Unable to create a charge! Please try again!"); //calls the api / sends request
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


		//Used to make PostRequest
		private T PostAsync<T>(string endpoint, StringContent content, string error = "Something went wrong!")
        {
            HttpResponseMessage responseMessage = null;
            T response;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.AcceptBlue.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Credentials));

                responseMessage = client.PostAsync(endpoint, content).Result;
                if (responseMessage == null || !responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception(error);
                }
                response = JsonSerializer.Deserialize<T>(responseMessage.Content.ReadAsStringAsync().Result, options);
            }
            return response;
        }
