namespace ClubHouse.Domain.Models.Request {
    public class GetEventListRequest : PagingRequest {
        public bool Is_filtered { get; set; } = true;

        public override string ToString() {
            return $"is_filtered={Is_filtered}&page_size={Page_size}&page={Page}";
        }
    }
}
