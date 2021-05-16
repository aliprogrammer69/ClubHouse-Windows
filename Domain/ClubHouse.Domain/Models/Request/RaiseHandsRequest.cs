namespace ClubHouse.Domain.Models.Request {
    public class RaiseHandsRequest {
        public string Channel { get; set; }
        public bool Raise_hands { get; set; } = true;
        public bool Unraise_hands { get; set; } = false;
    }
}
