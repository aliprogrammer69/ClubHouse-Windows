using System;
using System.Collections.Generic;
using System.Text;

namespace ClubHouse.Domain.Models.Response {
    public class BasePagingResponse : BaseResponse {
        public int Count { get; set; }
        public int Next { get; set; }
        public int Previous { get; set; }
    }
}
