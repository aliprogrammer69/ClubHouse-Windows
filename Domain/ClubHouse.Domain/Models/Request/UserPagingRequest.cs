namespace ClubHouse.Domain.Models.Request {
    public class UserPagingRequest : PagingRequest {
        public long User_id { get; set; }

        public override string ToString() {
            return $"user_id={User_id}&page_size={Page_size}&page={Page}";
        }
    }
}
