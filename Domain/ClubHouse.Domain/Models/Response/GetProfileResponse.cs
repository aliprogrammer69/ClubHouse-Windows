namespace ClubHouse.Domain.Models.Response {
    public class GetProfileResponse : BaseResponse {
        public int Num_invites { get; set; }
        public bool Has_unread_notifications { get; set; }
        public string[] Following_ids { get; set; }
        public string[] Blocked_ids { get; set; }
        public BaseUserInfo User_profile { get; set; }

        public string Access_token { get; set; }
        public int Actionable_notifications_count { get; set; }
        public object Email { get; set; }
        public string[] Feature_flags { get; set; }
        public bool Is_admin { get; set; }
        public bool Notifications_enabled { get; set; }
        public string Refresh_token { get; set; }
    }

}
