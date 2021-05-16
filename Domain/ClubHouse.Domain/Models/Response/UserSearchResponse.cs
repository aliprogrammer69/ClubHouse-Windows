using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class UserSearchResponse : BasePagingResponse {
        public string Query_id { get; set; }
        public IEnumerable<Models.BaseUserInfo> Users { get; set; }
    }
}
