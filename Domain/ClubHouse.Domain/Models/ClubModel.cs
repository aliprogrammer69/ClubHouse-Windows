namespace ClubHouse.Domain.Models {
    public class ClubModel {
        public long Club_id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo_url { get; set; }
        public int Num_members { get; set; }
        public int Num_followers { get; set; }

        public bool Is_follow_allowed { get; set; }
        public bool Is_membership_private { get; set; }
        public bool Is_community { get; set; }

        public bool Enable_private { get; set; }
        public int Num_online { get; set; }
        public ClubRule[] Rules { get; set; }
        public string Url { get; set; }

        public bool Is_member { get; set; }
        public bool Is_follower { get; set; }

        public class ClubRule {
            public string Desc { get; set; }
            public string Title { get; set; }
        }
    }
}
