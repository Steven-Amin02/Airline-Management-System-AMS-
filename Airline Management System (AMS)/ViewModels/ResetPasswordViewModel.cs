namespace Airline_Management_System__AMS_.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}
