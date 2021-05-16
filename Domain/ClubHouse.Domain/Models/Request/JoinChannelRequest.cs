namespace ClubHouse.Domain.Models.Request {
    public class JoinChannelRequest {
        public string Channel { get; set; }
        public string Attribution_source { get; set; } = "feed";
        public string Attribution_details { get; set; } = "eyJpc19leHBsb3JlIjpmYWxzZSwicmFuayI6MX0="; //base64_json
    }
}
