namespace ClubHouse.Domain.Models.Response {
    public class CreateChannelTargetsResponse: BaseResponse {
        public object[] Events { get; set; }
        public ClubModel[] Clubs { get; set; }
    }

}
