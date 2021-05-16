using System;
using System.Collections.Generic;
using System.Text;

namespace ClubHouse.Domain.Models.Request {
    public class ClubSearchRequest {
        public string Query { get; set; }
        public bool Cofollows_only { get; set; } = false;
        public bool Following_only { get; set; } = false;
        public bool Followers_only { get; set; } = false;
    }
}
