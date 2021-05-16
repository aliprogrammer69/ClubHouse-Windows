using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Extensions;
using ClubHouse.UI.DesktopApp.Handler;

using Prism.Commands;
using Prism.Regions;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class EventsViewModel : NavigationAwareViewModel {
        private readonly IEventService _eventService;
        private readonly IMessageService _messageService;
        private readonly IRegionManager _regionManager;
        private readonly RoomManagerService _roomManagerService;
        private readonly GetEventListRequest _request = new GetEventListRequest();
        public EventsViewModel(IEventService eventService,
                               IMessageService messageService,
                               IRegionManager regionManager,
                               RoomManagerService roomManagerService):base(roomManagerService) {
            _eventService = eventService;
            _messageService = messageService;
            _regionManager = regionManager;
            _roomManagerService = roomManagerService;
            InitializeCollection();
            GetEvents();
        }

        #region INavigationAware Members
        public override bool IsNavigationTarget(NavigationContext navigationContext) => true;
        #endregion

        public ObservableCollection<EventModel> AllEvents { get; protected set; }
        public ICollectionView EventsView { get; protected set; }

        private EventModel _selectedItem;
        public EventModel SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

        private bool _fetchingEvents;
        public bool FetchingEvents { get => _fetchingEvents; set => SetProperty(ref _fetchingEvents, value); }

        #region Commands
        public DelegateCommand<long?> ShowProfileCommand { get => new(ShowProfile); }
        public DelegateCommand CopyUrlCommand { get => new(CopyUrl); }
        public DelegateCommand JoinRoomCommand { get => new(JoinRoom); }
        public DelegateCommand CreateEventCommand { get => new(CreateEvent); }
        public DelegateCommand CloseCommand { get => new(Close); }

        private void ShowProfile(long? userId) {
            _regionManager.ShowProfile(Regions.EventsProfileContainerRegionName, userId ?? 0);
        }

        private void CopyUrl() {
            Clipboard.SetText(_selectedItem.Url);
            _messageService.Show("Event link is now in your clipboard. press CTRL + V any where you want.");
        }

        private async void JoinRoom() {
            InitChannelResponse response = await _roomManagerService.JoinRoom(_selectedItem.Channel);
            if (!response.Success) {
                _messageService.Show($"Failed to join the room. {response.Error_message}");
                return;
            }

            _regionManager.ShowRoom(response);
        }

        private void CreateEvent() {

        }

        private void Close() {
            NavigationService.Close();
        }
        #endregion

        #region Private Methods
        private void InitializeCollection() {
            AllEvents = new ObservableCollection<EventModel>();
            EventsView = CollectionViewSource.GetDefaultView(AllEvents);
            EventsView.SortDescriptions.Add(new SortDescription(nameof(EventModel.Time_start), ListSortDirection.Ascending));
        }

        private async void GetEvents() {
            FetchingEvents = true;
            GetEventListResponse events = await _eventService.GetList(_request);
            if (!events.Success) {
                _messageService.Show($"Failed to get events. {events.Error_message}");
                return;
            }

            IEnumerable<EventModel> newEvents = events.Events.Where(e => !AllEvents.Any(ae => ae.Event_id == e.Event_id));
            AllEvents.AddRange(newEvents);
            EventsView.Refresh();
            FetchingEvents = false;

            _request.TotalCount = events.Count;
            _request.NextPageCount = events.Next;
            _request.PreviousPageCount = events.Previous;
        }

        private void FetchNextPage() {
            if (_request.NextPageCount > 0) {
                _request.Page++;
                GetEvents();
            }
        }


        #endregion
    }
}
