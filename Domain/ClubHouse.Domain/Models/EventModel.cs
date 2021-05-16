using System;
using System.Collections.Generic;
using System.Text;

namespace ClubHouse.Domain.Models {
    public class EventModel {
        public ClubModel Club { get; set; }
        public string Url { get; set; }
        public string Channel { get; set; }
        public bool Club_is_member { get; set; }
        public bool Club_is_follower { get; set; }
        public bool Is_expired { get; set; }
        public IEnumerable<BaseUserInfo> Hosts { get; set; }

        public long Event_id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private DateTimeOffset _timeStart;
        public DateTimeOffset Time_start {
            get => _timeStart; set {
                _timeStart = value;
                CalcTimeOffset();
            }
        }
        public string TimeOffset { get; private set; }
        public bool Is_member_only { get; set; }

        private void CalcTimeOffset() {
            int dayDiff = (int)Math.Round((_timeStart - DateTimeOffset.Now).TotalDays);
            StringBuilder result = new StringBuilder();
            if (dayDiff == 0)
                result.Append("Today");
            else if (dayDiff == 1)
                result.Append("Tomorrow");
            else
                result.Append($"{dayDiff} Days Later");
            result.Append($" {Time_start.ToString("HH:mm")}");
            TimeOffset = result.ToString();
        }
    }
}
