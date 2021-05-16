namespace ClubHouse.Domain.Models {
    public class ChannelModel {
        public long Channel_id { get; set; }
        public string Channel { get; set; }
        public string Topic { get; set; }
        public string Url { get; set; }

        public bool Is_private { get; set; }
        public bool Is_social_mode { get; set; }
        public bool Has_blocked_speakers { get; set; }
        public bool Is_explore_channel { get; set; }

        public ClubModel Club { get; set; }
        public long? Club_id { get; set; }
        public string Club_name { get; set; }

        public long Creator_user_profile_id { get; set; }
        public int Num_speakers { get; set; }
        public int Num_all { get; set; }
        public int Num_other { get; set; }
        public ChannelUser[] Users { get; set; }

        public string[] Feature_flags { get; set; }
    }
}
