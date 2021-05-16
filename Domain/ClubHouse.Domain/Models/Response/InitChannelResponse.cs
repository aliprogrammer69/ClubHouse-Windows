namespace ClubHouse.Domain.Models.Response {
    public class InitChannelResponse : BaseChannelResponse {
        public string Token { get; set; }
        public string Rtm_token { get; set; }
        public string Pubnub_token { get; set; }
        public string Pubnub_origin { get; set; }
        public int Pubnub_heartbeat_value { get; set; }
        public int Pubnub_heartbeat_interval { get; set; }
        public bool Pubnub_enable { get; set; }
        public bool Agora_native_mute { get; set; }
        public string Pubnub_pub_key { get; set; }
        public string Pubnub_sub_key { get; set; }
        public string Agora_app_id { get; set; }
        public string Local_ws_agora_url { get; set; }
    }
}
