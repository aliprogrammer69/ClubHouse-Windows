using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class BaseChannelResponse : BaseResponse {
        public string Channel_id { get; set; }
        public string Channel { get; set; }
        public string Topic { get; set; }
        public string Url { get; set; }

        public IEnumerable<ChannelRule> Rules { get; set; }
        public bool Is_private { get; set; }
        public bool Is_social_mode { get; set; }
        public bool Is_handraise_enabled { get; set; }
        public int Handraise_permission { get; set; }

        public ClubModel Club { get; set; }
        public string Club_id { get; set; }
        public string Club_name { get; set; }
        public bool Is_club_member { get; set; }
        public bool Is_club_admin { get; set; }
        public bool Is_club_follower { get; set; }
        public bool Is_club_pending_accept { get; set; }

        public int Creator_user_profile_id { get; set; }
        public IEnumerable<ChannelUser> Users { get; set; }

        public string[] Feature_flags { get; set; }
        public object Welcome_for_user_profile { get; set; }

    }
}
