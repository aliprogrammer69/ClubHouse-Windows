using System;

namespace ClubHouse.Domain.Models {
    public class ChannelUser : BaseUserInfo {
        public int Skintone { get; set; }
        public bool Is_new { get; set; }
        public bool Is_speaker { get; set; }
        public bool Is_moderator { get; set; }
        public bool Is_muted { get; set; }
        public bool Is_speaking { get; set; }
        public bool Raise_hands { get; set; }
        public DateTimeOffset? Time_joined_as_speaker { get; set; }
        public bool Is_followed_by_speaker { get; set; }
        public bool Is_invited_as_speaker { get; set; }
    }
}
