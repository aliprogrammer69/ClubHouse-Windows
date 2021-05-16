namespace ClubHouse.Domain.Models {
    public class ClubUserInfo : BaseUserInfo {
        public bool Is_admin { get; set; }
        public bool Is_member { get; set; }
        public bool Is_follower { get; set; }
        public bool Is_pending_accept { get; set; }
        public bool Is_pending_approval { get; set; }
    }
}
