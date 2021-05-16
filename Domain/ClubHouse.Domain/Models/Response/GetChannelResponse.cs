using System;
using System.Collections.Generic;
using System.Text;

namespace ClubHouse.Domain.Models.Response {
    public class GetChannelResponse : BaseChannelResponse {
        public bool Should_leave { get; set; }
    }
}
