using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Handler;
using MaterialDesignThemes.Wpf;
using Prism.Commands;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class JoinAsSpeakerViewModel : BaseViewModel {
        private readonly RoomManagerService _roomManagerService;
        private readonly IChannelService _channelService;
        private readonly IMessageService _messageService;
        private string fromName;

        public JoinAsSpeakerViewModel(RoomManagerService roomManagerService,
                                      IChannelService channelService,
                                      IMessageService messageService) {
            _roomManagerService = roomManagerService;
            _channelService = channelService;
            _messageService = messageService;
        }

        public string FromName { get => fromName; set => SetProperty(ref fromName, value); }
        public long FromUserId { get; set; }

        public DelegateCommand RejectCommand { get => new(Reject); }
        public DelegateCommand AcceptCommand { get => new(Accept); }

        public async void Reject() {
            if (string.IsNullOrEmpty(_roomManagerService.ChannelInfo?.Channel)) {
                DialogHost.Close(null, false);
                return;
            }
            
            var apiResult = await _channelService.RejectSpeakerInvite(new ChannelUserRequest {
                Channel = _roomManagerService.ChannelInfo.Channel,
                User_id = FromUserId
            });
            if(apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? "Error in reject invitation");
            }
            DialogHost.Close(null, false);
        }

        public async void Accept() {
            if (string.IsNullOrEmpty(_roomManagerService.ChannelInfo?.Channel)) {
                DialogHost.Close(null, false);
                return;
            }

            var apiResult = await _channelService.AcceptSpeakerInvite(new ChannelUserRequest {
                Channel = _roomManagerService.ChannelInfo.Channel,
                User_id = FromUserId
            });
            if (apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? "Error in accept invitation");
            }
            else {
                _roomManagerService.Rejoin();
                DialogHost.Close(null, true);
            }
        }
    }
}
