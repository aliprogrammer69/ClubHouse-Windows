namespace ClubHouse.Domain.Models.Response {
    public class CompletePhoneNumberAuthResponse : BaseResponse {
        public string Auth_token { get; set; }
        public string Refresh_token { get; set; }
        public bool Is_waitlisted { get; set; }
        public bool Is_onboarding { get; set; }
        public Models.BaseUserInfo User_profile { get; set; }
    }
}
