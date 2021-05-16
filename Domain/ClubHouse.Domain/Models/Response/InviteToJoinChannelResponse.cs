namespace ClubHouse.Domain.Models.Response {
    public class InviteToJoinChannelResponse : BaseResponse {
        public bool Notifications_enabled { get; set; }
        public string Fallback_number_hash { get; set; }
        public string Fallback_message { get; set; }
    }
}
