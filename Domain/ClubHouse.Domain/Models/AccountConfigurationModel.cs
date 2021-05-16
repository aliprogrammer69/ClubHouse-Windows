using System;
using ClubHouse.Common;

namespace ClubHouse.Domain.Models {
    public class AccountConfigurationModel {
        public string UserDevice { get; set; } = Guid.NewGuid().ToString().ToUpper();
        public long? UserId { get; set; }
        public string UserToken { get; set; }
        public string Cookie { get; set; } = $"__cfduid={Utils.GetRandomHexNumber(42)}{Utils.random.Next(1, 9)}";
        public string RefereshToken { get; set; }
    }
}
