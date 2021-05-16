using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class GetClubMemebersResponse : BasePagingResponse {
        public IEnumerable<ClubUserInfo> Users { get; set; }
    }
}
