using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Prism.Mvvm;

namespace ClubHouse.UI.DesktopApp.Models {
    public class RoomUserCollections : BindableBase, IList<BindableChannelUser> {
        private string searchQuery;
        private string currentSearchQuery;

        public RoomUserCollections() {
            InitializeCollections();
        }

        public string SearchQuery { get => searchQuery; set => SetProperty(ref searchQuery, value); }

        public ObservableCollection<BindableChannelUser> AllItems { get; set; }
        public ICollectionView SpeakersView { get; set; }
        public ICollectionView FollowedBySpeakersView { get; set; }
        public ICollectionView OthersView { get; set; }

        public BindableChannelUser this[int index] { get => AllItems[index]; set => AllItems[index] = value; }

        public int Count => AllItems.Count;

        public bool IsReadOnly => false;

        public void Add(BindableChannelUser item) => AllItems.Add(item);
        public void AddRange(IEnumerable<BindableChannelUser> items) => AllItems.AddRange(items);
        public void Clear() => InitializeCollections();
        public bool Contains(BindableChannelUser item) => AllItems.Contains(item);
        public void CopyTo(BindableChannelUser[] array, int arrayIndex) => AllItems.CopyTo(array, arrayIndex);
        public IEnumerator<BindableChannelUser> GetEnumerator() => AllItems.GetEnumerator();
        public int IndexOf(BindableChannelUser item) => AllItems.IndexOf(item);
        public void Insert(int index, BindableChannelUser item) => AllItems.Insert(index, item);
        public bool Remove(BindableChannelUser item) => AllItems.Remove(item);
        public void RemoveAt(int index) => RemoveAt(index);
        public BindableChannelUser Find(long userId) {
            //for(int i = 0; i < AllItems.Count; i++) {
            //    var item = AllItems[i];
            //    if (item?.User_id == userId)
            //        return item;
            //}
            //return null;
            return AllItems?.ToList().FirstOrDefault(u => u.User_id == userId);
        }

        IEnumerator IEnumerable.GetEnumerator() => AllItems.GetEnumerator();

        public void Search() {
            currentSearchQuery = SearchQuery;
            RefereshView();
        }

        public void RefereshView(bool refereshSpeakers = true, bool refereshFollowedBySpeakers = true, bool refereshOthers = true) {
            if (refereshSpeakers)
                SpeakersView.Refresh();
            if (refereshFollowedBySpeakers)
                FollowedBySpeakersView.Refresh();
            if (refereshOthers)
                OthersView.Refresh();
        }

        private bool MatchQuery(BindableChannelUser item) {
            return string.IsNullOrEmpty(currentSearchQuery) || (item?.Name?.Contains(currentSearchQuery,StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private void InitializeCollections() {
            AllItems = new ObservableCollection<BindableChannelUser>();

            var speakerSource = new CollectionViewSource() {
                Source = AllItems
            };
            SpeakersView = speakerSource.View;
            SpeakersView.Filter = (u) => u is BindableChannelUser user && user.Is_speaker && MatchQuery(user);

            var followedBySpeakerSource = new CollectionViewSource() {
                Source = AllItems
            };
            FollowedBySpeakersView = followedBySpeakerSource.View;
            FollowedBySpeakersView.Filter = (u) => u is BindableChannelUser user && !user.Is_speaker && user.Is_followed_by_speaker && MatchQuery(user);

            var othersSource = new CollectionViewSource() {
                Source = AllItems
            };
            OthersView = othersSource.View;
            OthersView.Filter = (u) => u is BindableChannelUser user && !user.Is_speaker && !user.Is_followed_by_speaker && MatchQuery(user);
        }
    }
}
