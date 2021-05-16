using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class GetEventListResponse : BasePagingResponse {
        public IEnumerable<EventModel> Events { get; set; }
    }
    
}
