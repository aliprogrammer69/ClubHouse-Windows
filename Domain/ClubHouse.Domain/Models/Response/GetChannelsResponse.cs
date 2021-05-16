using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class GetChannelsResponse : BaseResponse {
        public IEnumerable<ChannelModel> Channels { get; set; }
    }
}
