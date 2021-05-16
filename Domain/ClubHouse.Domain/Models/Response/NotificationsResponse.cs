using System.Collections.Generic;

namespace ClubHouse.Domain.Models.Response {
    public class NotificationsResponse : BasePagingResponse {
        public IEnumerable<NotificationModel> Notifications { get; set; }
    }
}
