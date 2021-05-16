namespace ClubHouse.Domain.Models.Response {
    public class PubnubResponse {
        public string Action { get; set; }
        public string Channel { get; set; }

        public long User_id { get; set; }
        public ChannelUser User_profile { get; set; }
        public bool Is_on_call { get; set; }

        public long From_user_id { get; set; }
        public string From_name { get; set; }
    }
}
