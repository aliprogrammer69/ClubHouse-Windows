using System;

namespace ClubHouse.UI.DesktopApp.Models {
    public class UserListToken : IEquatable<UserListToken> {
        public UserLoadingActions Action { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 50;

        public bool Equals(UserListToken other) => other != null &&
                                                   other.Action == Action &&
                                                   other.UserId == UserId;
    }
}
