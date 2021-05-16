using System;
using ClubHouse.Domain.Models;

using Prism.Commands;
using Prism.Mvvm;

namespace ClubHouse.UI.DesktopApp.Models {
    public class BindableChannelUser : BindableBase {
        private readonly BaseUserInfo userInfo;
        private bool is_speaker;
        private bool is_moderator;
        private bool is_muted;
        private bool is_speaking;
        private bool raise_hands;
        private bool is_invited_as_speaker;
        private int skintone;
        private bool is_followed_by_speaker;
        private DateTimeOffset? time_joined_as_speaker;
        private bool is_new;
        private bool is_on_call;

        public BindableChannelUser(BaseUserInfo userInfo) {
            this.userInfo = userInfo;
        }

        public BindableChannelUser(ChannelUser channelUser) {
            this.userInfo = channelUser;
            SetProperties(channelUser);
            Is_muted = channelUser.Is_muted;
            Is_speaking = channelUser.Is_speaking;
        }

        public long User_id => userInfo.User_id;
        public string Username => userInfo.Username;
        public string Name => userInfo.Name;
        public string FirstName => userInfo.First_name;
        public string Photo_url => userInfo.Photo_url;
        public string ThumbnailPhotoUrl => userInfo.ThumbnailPhotoUrl;
        public string Bio => userInfo.Bio;

        public int Skintone { get => skintone; set => SetProperty(ref skintone, value); }
        public bool Is_new { get => is_new; set => SetProperty(ref is_new, value); }
        public bool Is_speaker { get => is_speaker; set => SetProperty(ref is_speaker, value); }
        public bool Is_moderator { get => is_moderator; set => SetProperty(ref is_moderator, value); }
        public bool Is_muted { get => is_muted; set => SetProperty(ref is_muted, value); }
        public bool Is_speaking { get => is_speaking; set => SetProperty(ref is_speaking, value); }
        public bool Raise_hands { get => raise_hands; set => SetProperty(ref raise_hands, value); }
        public bool Is_invited_as_speaker { get => is_invited_as_speaker; set => SetProperty(ref is_invited_as_speaker, value); }
        public bool Is_followed_by_speaker { get => is_followed_by_speaker; set => SetProperty(ref is_followed_by_speaker, value); }
        public bool Is_on_call { get => is_on_call; set => SetProperty(ref is_on_call, value); }
        public DateTimeOffset? Time_joined_as_speaker { get => time_joined_as_speaker; set => SetProperty(ref time_joined_as_speaker, value); }

        public void SetProperties(ChannelUser channelUser) {
            Skintone = channelUser.Skintone;
            Is_speaker = channelUser.Is_speaker;
            Is_moderator = channelUser.Is_moderator;
            Raise_hands = channelUser.Raise_hands;
            Is_invited_as_speaker = channelUser.Is_invited_as_speaker;
            Is_new = channelUser.Is_new;
            Time_joined_as_speaker = channelUser.Time_joined_as_speaker;
            Is_followed_by_speaker = channelUser.Is_followed_by_speaker;
        }

        public override bool Equals(object obj) {
            if (obj is ChannelUser user) {
                return user.Skintone == Skintone &&
                    user.Is_speaker == Is_speaker &&
                    user.Is_moderator == Is_moderator &&
                    user.Raise_hands == Raise_hands &&
                    user.Is_invited_as_speaker == Is_invited_as_speaker &&
                    user.Is_new == Is_new &&
                    user.Time_joined_as_speaker == Time_joined_as_speaker &&
                    user.Is_followed_by_speaker == Is_followed_by_speaker;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
