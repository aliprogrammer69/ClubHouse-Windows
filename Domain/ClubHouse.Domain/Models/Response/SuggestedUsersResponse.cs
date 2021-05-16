using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class SuggestedUsersResponse : BasePagingResponse {
        public IEnumerable<Models.BaseUserInfo> Users { get; set; }
    }
}
