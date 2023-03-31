//Used to Verify and save nonce token by returning a longer term token
		public SourceVerificationResponse SourceVerification(SourceVerificationRequest verifyAndSaveRequest)
		{
            string txtContent = JsonSerializer.Serialize(verifyAndSaveRequest, options);
            var endpoint = Config.AcceptBlue.Paths.Verify;
            var payload = new StringContent(txtContent, Encoding.UTF8, "application/json");
            SourceVerificationResponse sourceVerificationResponse = PostAsync<SourceVerificationResponse>(endpoint, payload);
            if (!string.IsNullOrWhiteSpace(sourceVerificationResponse.Error_Code))
            {
                throw new Exception(sourceVerificationResponse.Error_Message);
            }
            if (sourceVerificationResponse.Status_Code != "A")
            {
                throw new Exception(sourceVerificationResponse.Status);
            }
            return sourceVerificationResponse;
        }
