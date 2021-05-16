using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Request {
    public class CreateChannelRequest {
        public string Topic { get; set; }
        public bool Is_private { get; set; }
        public bool Is_social_mode { get; set; }
        public IEnumerable<long> User_ids { get; set; }
        public long? Club_id { get; set; }
        public long? Event_id { get; set; }
    }
}
