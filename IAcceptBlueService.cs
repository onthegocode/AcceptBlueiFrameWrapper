using SO_Virtual_Terminal.Models.AcceptBlue;

namespace SO_Virtual_Terminal.Services
{
    public interface IAcceptBlueService
    {
        CreditResponse Credit(CreditRequest creditRequest);
        SourceVerificationResponse SourceVerification(SourceVerificationRequest verifyAndSaveRequest);
        SourceChargeResponse SourceCharge(SourceChargeRequest sourceChargeRequest);
    }
}
