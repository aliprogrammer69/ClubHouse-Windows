namespace ClubHouse.Domain.Models.Request {
    public class UserSearchRequest {
        public string Query { get; set; }
        public bool Cofollows_only { get; set; } = false;
        public bool Following_only { get; set; } = false;
        public bool Followers_only { get; set; } = false;
    }
}
