namespace ClubHouse.Domain.Models.Response {
    public class CheckWaitListStatusResponse : BaseResponse {
        public Analytics_Properties Analytics_properties { get; set; }
        public ClubModel Club { get; set; }
        public bool Enable_twitter { get; set; }
        public BaseUserInfo Invited_by_user_profile { get; set; }
        public bool Is_onboarding { get; set; }
        public bool Is_waitlisted { get; set; }
        public int Num_preselect_follows { get; set; }

        public class Analytics_Properties {
            public string SignUpDay { get; set; }
            public string SignUpWeek { get; set; }
        }
    }
}
