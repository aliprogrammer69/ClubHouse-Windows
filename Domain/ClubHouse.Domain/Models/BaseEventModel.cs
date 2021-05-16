using System;
using System.Collections.Generic;
using System.Text;

namespace ClubHouse.Domain.Models {
    public class BaseEventModel {
        public long? Event_id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Is_member_only { get; set; } = false;
    }
}
