using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class GetClubResponse : BaseResponse {
        public bool Is_admin { get; set; }
        public bool Is_member { get; set; }
        public bool Is_follower { get; set; }
        public ClubModel Club { get; set; }

        public bool Is_pending_accept { get; set; }
        public bool Is_pending_approval { get; set; }
        public object Added_by_user_profile { get; set; }
        public object[] Member_user_ids { get; set; }
        public int Num_invites { get; set; }
        public object Invite_link { get; set; }
        public IEnumerable<TopicModel> Topics { get; set; }
    }
}
