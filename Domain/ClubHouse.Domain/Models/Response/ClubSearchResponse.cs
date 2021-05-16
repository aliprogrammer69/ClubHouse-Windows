namespace ClubHouse.Domain.Models.Response {

    public class ClubSearchResponse : BasePagingResponse {
        public ClubModel[] Clubs { get; set; }
        public string Query_id { get; set; }
    }

}
