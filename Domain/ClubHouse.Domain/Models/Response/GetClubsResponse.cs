using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class GetClubsResponse : BaseResponse {
        public IEnumerable<ClubModel> Clubs { get; set; }
    }
}
