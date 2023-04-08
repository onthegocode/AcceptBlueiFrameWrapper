using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SO_Virtual_Terminal.Data;
using SO_Virtual_Terminal.Data.Models;
using SO_Virtual_Terminal.Models;
using SO_Virtual_Terminal.Models.AcceptBlue;
using SO_Virtual_Terminal.Services;
using SO_Virtual_Terminal.Utilities;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SO_Virtual_Terminal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly CloverClient CloverClient;
        private readonly POSHttpClient POSClient;
        private readonly IAcceptBlueService AcceptBlue;

        public TransactionsController(ApplicationDbContext dbContext, CloverClient cloverClient, POSHttpClient posClient, IAcceptBlueService acceptBlue)
        {
            DbContext = dbContext;
            CloverClient = cloverClient;
            POSClient = posClient;
            AcceptBlue = acceptBlue;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<PaymentResponse>> GetPayment([FromBody()] PaymentRequest paymentRequest = null)
        {
            PaymentResponse paymentResponse = null;

            if (string.IsNullOrWhiteSpace(paymentRequest.LocationId) || string.IsNullOrWhiteSpace(paymentRequest.PaymentId)) { return NotFound(); }

            Location location = DbContext.Locations
                .Include(o => o.LocationSettings)
                .SingleOrDefault(c => c.Id == paymentRequest.LocationId);

            if (location == null) { return NotFound(); }

            Clover.Payment payment = await CloverClient.GetPayment(location.LocationSettings.ApiKey, location.CloverMerchantId, paymentRequest.PaymentId);

            if (payment == null) { return NotFound(); }

            paymentResponse = new PaymentResponse
            {
                Amount = (decimal)payment.Amount / 100,
                PaymentId = payment.Id,
                OrderId = payment.Order?.id,
                ReferenceId = payment.CardTransaction?.ReferenceId,
                Type = payment.CardTransaction?.Type,
                EntryMethod = payment.CardTransaction?.EntryType,
                Result = payment.Result,
                State = payment.CardTransaction.State,
                CardHolderName = payment.CardTransaction?.CardholderName,
                AuthCode = payment.CardTransaction?.AuthCode,
                CardType = payment.CardTransaction?.CardType,
                First6 = payment.CardTransaction?.First6,
                Last4 = payment.CardTransaction?.Last4,
                Terminal = payment.Device?.Id.ToString(),
                Date = DateTimeOffset.FromUnixTimeMilliseconds(payment.CreatedTime).Date.ToString(),
                Time = DateTimeOffset.FromUnixTimeMilliseconds(payment.CreatedTime).TimeOfDay.ToString(),
            };

            return Ok(paymentResponse);
        }

       /* //Refund
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<RefundResponse>> Refund([FromBody()] RefundRequest refundRequest = null)
        {
            RefundResponse refundResponse = null;
            Location location;
            Transaction paymentTransaction;
            POS.InvoiceRefundInformation invoiceRefund;
            Transaction transaction;

            try
            {
                // Ensure we have a valid request
                if (string.IsNullOrWhiteSpace(refundRequest.LocationId) || string.IsNullOrWhiteSpace(refundRequest.PaymentId) || refundRequest.Amount <= 0 || string.IsNullOrWhiteSpace(refundRequest.CardNumber) || refundRequest.ExpirationMonth < 1 || refundRequest.ExpirationMonth > 12 || refundRequest.ExpirationYear <= 0) { return BadRequest(new { status = "Error", message = "Invalid request, please verify and try again." }); }

                // Retrieve the Location information along with the LocationSettings
                location = DbContext.Locations
                    .Include(o => o.LocationSettings)
                    .SingleOrDefault(c => c.Id == refundRequest.LocationId);
                if (location == null) { return BadRequest(new { status = "Error", message = "Unable to retrieve Location, please verify and try again." }); }

                // Retrieve the original Payment Transaction
                paymentTransaction = DbContext.Transactions.SingleOrDefault(c => c.LocationId == refundRequest.LocationId && c.PaymentId == refundRequest.PaymentId && !c.IsRefund);
                if (paymentTransaction == null) { return BadRequest(new { status = "Error", message = "Unable to retrieve original transaction, please verify and try again." }); }
                if (refundRequest.Amount > paymentTransaction.Amount) { return BadRequest(new { status = "Error", message = "Refund Amount cannot be greater than original transaction amount" }); }

                // Retrieve the Invoice from Zeus (includes the required RequestId to record the refund back to Zeus)
                // First attempt to retrieve the Invoice as a "Pre-Sale" invoice as we do not know the original invoice type
                invoiceRefund = await POSClient.GetInvoiceRefund("PreSale", location.Id, paymentTransaction.InvoiceId, paymentTransaction.OrderId, paymentTransaction.PaymentId);
                if (invoiceRefund == null)
                {
                    // If we did not get a "Pre-Sale" invoice, attempt to retrieve the Invoice as a "Order" invoice
                    invoiceRefund = await POSClient.GetInvoiceRefund("Order", location.Id, paymentTransaction.InvoiceId, paymentTransaction.OrderId, paymentTransaction.PaymentId);
                }
                if (invoiceRefund == null) { return BadRequest(new { status = "Error", message = "Unable to retrieve Invoice, please verify and try again." }); }

                if (refundRequest.Amount > invoiceRefund.EligibleRefundAmount) { return BadRequest(new { status = "Error", message = "Refund Amount cannot be greater than Eligible Refund Amount" }); }

                *//****************************************************************************************************
                 ****************************************************************************************************
                 ** This is the Clover refund coding, it can be re-implemented once the permissions are corrected. **
                 ****************************************************************************************************
                 ****************************************************************************************************//*
                // Process the Refund with Clover
                //Clover.ECommRefundResponse cloverRefundResponse = await CloverClient.Refund(location.LocationSettings.ApiKey, refundRequest.PaymentId, (long)(refundRequest.Amount * 100), string.Empty);

                // Ensure that the Refund was Successfully processed
                //if (cloverRefundResponse == null) { return NotFound(); }
                //if (cloverRefundResponse.Status.ToLower() != "succeeded") { return BadRequest(new { status = "Error", message = "Unable to process refund, please verify and try again." }); }

                // Record the Refund in the transactions table
                //transaction = new Transaction
                //{
                //    RequestId = invoiceRefund.RequestId,
                //    LocationId = location.Id,
                //    SerialNumber = "Virtual",
                //    InvoiceId = invoiceRefund.InvoiceId,
                //    Method = "Card",
                //    IsRefund = true,
                //    Amount = (decimal)cloverRefundResponse.Amount / 100,
                //    SurchargeAmount = decimal.Parse("0.00"),
                //    OrderId = paymentTransaction.OrderId,
                //    PaymentId = paymentTransaction.PaymentId,
                //    ReferenceId = cloverRefundResponse.Id,
                //    Type = string.Empty,
                //    EntryMethod = string.Empty,
                //    Result = "SUCCESS",
                //    State = string.Empty,
                //    CreatedTime = cloverRefundResponse.Created,
                //    AuthCode = string.Empty,
                //    CardType = string.Empty,
                //    First6 = string.Empty,
                //    Last4 = string.Empty,
                //    ExpirationDate = string.Empty,
                //    CardholderName = string.Empty,
                //    Token = string.Empty,
                //    Created = DateTimeOffset.FromUnixTimeMilliseconds(cloverRefundResponse.Created).DateTime,
                //    PromoCode = string.Empty,
                //    ResponseCode = string.Empty,
                //    ResponseText = string.Empty,
                //    OFPLanguage = string.Empty,
                //    AVSCode = string.Empty,
                //    CVV2Result = string.Empty,
                //    DeclineReason = string.Empty,
                //    Signature = string.Empty,
                //    FullRefund = paymentTransaction.Amount == (decimal)cloverRefundResponse.Amount / 100 ? true : false,
                //};

                //DbContext.Transactions.Add(transaction);
                //await DbContext.SaveChangesAsync();

                // Send the Refund to Zeus
                //await POSClient.PostCardRefund(new POS.CardRefundRequest(transaction));

                //refundResponse = new RefundResponse
                //{
                //    Id = cloverRefundResponse.Id,
                //};
                *//****************************************************************************************************
                 ****************************************************************************************************
                 ** This is the Clover refund coding, it can be re-implemented once the permissions are corrected. **
                 ****************************************************************************************************
                 ****************************************************************************************************/
               
                /* Accept Blue refund processing *//*
                Models.AcceptBlue.CreditRequest creditRequest = new Models.AcceptBlue.CreditRequest
                {
                    Amount = refundRequest.Amount,
                    Card = refundRequest.CardNumber,
                    Expiry_Month = refundRequest.ExpirationMonth,
                    Expiry_Year = refundRequest.ExpirationYear,
                };
                Models.AcceptBlue.CreditResponse creditResponse = AcceptBlue.Credit(creditRequest);

                // Record the Refund in the transactions table
                transaction = new Transaction
                {
                    RequestId = invoiceRefund.RequestId,
                    LocationId = location.Id,
                    SerialNumber = "Virtual",
                    InvoiceId = invoiceRefund.InvoiceId,
                    Method = "Card",
                    IsRefund = true,
                    Amount = creditRequest.Amount,
                    SurchargeAmount = decimal.Parse("0.00"),
                    OrderId = paymentTransaction.OrderId,
                    PaymentId = paymentTransaction.PaymentId,
                    ReferenceId = creditResponse.Reference_Number.ToString(),
                    Type = string.Empty,
                    EntryMethod = string.Empty,
                    Result = "SUCCESS",
                    State = string.Empty,
                    CreatedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    AuthCode = string.Empty,
                    CardType = creditResponse.Card_Type,
                    First6 = string.Empty,
                    Last4 = creditResponse.Last_4,
                    ExpirationDate = string.Empty,
                    CardholderName = string.Empty,
                    Token = string.Empty,
                    Created = DateTime.UtcNow,
                    PromoCode = string.Empty,
                    ResponseCode = string.Empty,
                    ResponseText = string.Empty,
                    OFPLanguage = string.Empty,
                    AVSCode = string.Empty,
                    CVV2Result = string.Empty,
                    DeclineReason = string.Empty,
                    Signature = string.Empty,
                    FullRefund = paymentTransaction.Amount == creditRequest.Amount ? true : false,
                };

                DbContext.Transactions.Add(transaction);
                await DbContext.SaveChangesAsync();

                // Send the Refund to Zeus
                await POSClient.PostCardRefund(new POS.CardRefundRequest(transaction));

                refundResponse = new RefundResponse
                {
                    Id = creditResponse.Reference_Number.ToString(),
                };
            }

            catch (Exception ex)
            {
                return BadRequest(new { status = "Error", message = ex.Message });
            }

            return Ok(refundResponse);
        }*/

        //AcceptBlue

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
                    Name = verificationRequest.Name,
                    Avs_Address = verificationRequest.Avs_Address,
					Avs_Zip = verificationRequest.Avs_Zip,
					Software = verificationRequest.Software,
					Save_Card = true, //must be true to save
					Source = verificationRequest.Source, // required
					Expiry_Month = verificationRequest.Expiry_Month, // required
					Expiry_Year = verificationRequest.Expiry_Year, // required
				};

				Models.AcceptBlue.SourceVerificationResponse sourceVerificationResponse = AcceptBlue.SourceVerification(sourceVerificationRequest); //calls the SourceVerification Method in the AcceptBlueService

				//Captures the response given to us by the SourceVerification method 
				verificationResponse = new VerificationResponse
				{
					Status = sourceVerificationResponse.Status == "Approved" ? "Success" : sourceVerificationResponse.Status,
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
					Avs_Address = chargeRequest.Avs_Address, 
					Avs_Zip = chargeRequest.Avs_Zip,
					CVV2 = chargeRequest.CVV2, 
					Expiry_Month = chargeRequest.Expiry_Month, 
					Expiry_Year = chargeRequest.Expiry_Year, 
					Source = "tkn-" + chargeRequest.Token, // required
					Software = chargeRequest.Software,
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
    }

    public class PaymentRequest
    {
        public string LocationId { get; set; }

        public string PaymentId { get; set; }
    }

    public class PaymentResponse
    {
        public decimal Amount { get; set; }

        public string PaymentId { get; set; }

        public string OrderId { get; set; }

        public string ReferenceId { get; set; }

        public string Type { get; set; }

        public string EntryMethod { get; set; }

        public string Result { get; set; }

        public string State { get; set; }

        public string CardHolderName { get; set; }

        public string AuthCode { get; set; }

        public string CardType { get; set; }

        public string First6 { get; set; }

        public string Last4 { get; set; }

        public string Terminal { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }
    }

    public class RefundRequest
    {
        public string LocationId { get; set; }

        public string PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }
    }

    public class RefundResponse
    {
        public string Id { get; set; }
    }
    public class VerificationRequest
    {
        public string Source { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Software { get; set; } = string.Empty;
        public string Avs_Address { get; set; } = string.Empty;
        public string Avs_Zip { get; set; } = string.Empty;
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
        public string Avs_Address { get; set; } = string.Empty;
        public string Avs_Zip { get; set; } = string.Empty;
        public int Expiry_Month { get; set; } = DateTime.Now.Month + 1;
        public int Expiry_Year { get; set; } = DateTime.Now.Year + 1;
        public string Software { get; set; } = string.Empty;
        public string CVV2 { get; set; } = null; //default null as using string.empty causes error
		public string Token { get; set; }
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
}
