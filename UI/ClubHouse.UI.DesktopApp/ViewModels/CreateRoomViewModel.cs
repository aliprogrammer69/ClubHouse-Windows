using System.Collections.ObjectModel;
using System.Linq;
using ClubHouse.Common;
using ClubHouse.Domain;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Models;
using ClubHouse.UI.DesktopApp.Views;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class CreateRoomViewModel : BaseViewModel {
        private readonly IChannelService _channelService;
        private readonly IRegionManager _regionManager;
        private readonly RoomManagerService _roomManagerService;
        private readonly IMessageService _messageService;
        private string topic;
        private ObservableCollection<CreateChannelTargetModel> targets;
        private CreateChannelTargetModel selectedTarget;

        public CreateRoomViewModel(IChannelService channelService,
                                   IRegionManager regionManager,
                                   RoomManagerService roomManagerService,
                                   IMessageService messageService) {
            _channelService = channelService;
            _regionManager = regionManager;
            _roomManagerService = roomManagerService;
            _messageService = messageService;

            SetTargets();
        }

        public string Topic { get => topic; set => SetProperty(ref topic, value); }
        public ObservableCollection<CreateChannelTargetModel> Targets { get => targets; set => SetProperty(ref targets, value); }
        public CreateChannelTargetModel SelectedTarget { get => selectedTarget; set => SetProperty(ref selectedTarget, value); }

        public DelegateCommand CreateRoomCommand { get => new(CreateRoom); }

        public async void SetTargets() {
            var apiResult = await _channelService.GetCreateChannelTargets();

            Targets = new ObservableCollection<CreateChannelTargetModel>(new CreateChannelTargetModel[] {
                new CreateChannelTargetModel() {
                    Name = "Open",
                    Description = "Start a room open to everyone",
                    PhotoUrl = "../Images/open.png",
                    IsPrivate = false,
                    IsSocialable = true,
                },
                new CreateChannelTargetModel() {
                    Name = "Social",
                    Description = "Start a room with people I follow",
                    PhotoUrl = "../Images/social.png",
                    IsPrivate = false,
                    IsSocialable = false,
                },
                new CreateChannelTargetModel() {
                    Name = "Closed",
                    Description = "Start a room for people I choose",
                    PhotoUrl = "../Images/close.png",
                    IsPrivate = true,
                    IsSocialable = false,
                }
            });

            if (apiResult?.Events != null) {
                //TODO: add events
            }

            if (apiResult?.Clubs != null) {
                Targets.AddRange(apiResult.Clubs.Select(c => new CreateChannelTargetModel() {
                    Name = c.Name,
                    Description = c.Description,
                    EntityId = c.Club_id,
                    EntityType = EntityType.Club,
                    IsPrivate = false,
                    IsSocialable = true,
                    PhotoUrl = c.Photo_url
                }));
            }
        }

        public async void CreateRoom() {
            if(SelectedTarget == null) {
                return;
            }

            await _roomManagerService.LeaveRoom();
            var apiResult = await _channelService.Create(new CreateChannelRequest() {
                Topic = Topic,
                Is_private = SelectedTarget.IsPrivate,
                Is_social_mode = SelectedTarget.IsSocialable,
                Club_id = SelectedTarget.EntityType == EntityType.Club && SelectedTarget.EntityId != null ? SelectedTarget.EntityId.Value : null,
                Event_id = SelectedTarget.EntityType == EntityType.Event && SelectedTarget.EntityId != null ? SelectedTarget.EntityId.Value : null
            });

            if (apiResult?.Success == true) {
                _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(RoomView), new NavigationParameters() {
                    { Consts.ChannelResponseNavigationParameterKey, apiResult }
                });
                DialogHost.Close(null, null);
                return;
            }
            else {
                _messageService.Show(apiResult?.Error_message ?? "Failed to create room");
            }
        }
    }
}
