namespace ClubHouse.Domain.Models.Request {
    public class PagingRequest {
        public int Page_size { get; set; } = 50;
        public int Page { get; set; } = 1;
        public int TotalCount { get; set; }
        public int NextPageCount { get; set; }
        public int PreviousPageCount { get; set; }
        public override string ToString() {
            return $"page_size={Page_size}&page={Page}";
        }
    }
}
