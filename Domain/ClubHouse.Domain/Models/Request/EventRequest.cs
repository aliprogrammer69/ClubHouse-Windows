using System;
using System.Collections.Generic;
using System.Text;

namespace ClubHouse.Domain.Models.Request {
    public class EventRequest : BaseEventModel {
        public long? Time_start_epoch { get; set; }
        public IEnumerable<long> User_ids { get; set; }
        public long? Club_id { get; set; }
        public string Event_hashid { get; set; }
    }
}
