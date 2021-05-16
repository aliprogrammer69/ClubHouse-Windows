using System;
using System.Collections.Generic;

namespace ClubHouse.Domain.Models {
    public class UserInfo : BaseUserInfo {
        public string Twitter { get; set; }
        public string TwitterUrl { get => !string.IsNullOrEmpty(Twitter) ? $"https://twitter.com/{Twitter}" : null; }
        public string Instagram { get; set; }
        public string InstagramUrl { get => !string.IsNullOrEmpty(Instagram) ? $"https://instagram.com/{Instagram}" : null; }
        public int Num_followers { get; set; }
        public int Num_following { get; set; }
        public bool Follows_me { get; set; }
        public int Mutual_follows_count { get; set; }
        public IEnumerable<ClubModel> Clubs { get; set; }

        public object Displayname { get; set; }
        public DateTime Time_created { get; set; }
        public bool Is_blocked_by_network { get; set; }
        public object[] Mutual_follows { get; set; }
        public int Notification_type { get; set; } // 3: not follow. 2: followed
        public bool Following { get => Notification_type == 2; }
        public BaseUserInfo Invited_by_user_profile { get; set; }
        public object Invited_by_club { get; set; }
        public string Url { get; set; }
        public bool Can_receive_direct_payment { get; set; }
        public float Direct_payment_fee_rate { get; set; }
        public float Direct_payment_fee_fixed { get; set; }
    }
}
