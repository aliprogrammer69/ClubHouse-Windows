namespace ClubHouse.Domain.Models.Request {
    public class TopicPagingRequest : PagingRequest {
        public TopicPagingRequest() {
            Page_size = 25;
        }

        public long Topic_id { get; set; }
    }
}
