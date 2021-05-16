using System;
using System.Text;

namespace ClubHouse.Domain.Models {
    public class NotificationModel {
        public long Notification_Id { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool Is_unread { get; set; }
        private DateTimeOffset _timeCreated;
        public DateTimeOffset Time_created {
            get => _timeCreated; set {
                _timeCreated = value;
                TimeOffset =  CalculateTimeOffset();
            }
        }
        public string TimeOffset { get; private set; }
        public BaseUserInfo User_profile { get; set; }
        public string Channel { get; set; }

        private string CalculateTimeOffset() {
            int diff = (int)Math.Round((DateTimeOffset.Now - _timeCreated).TotalMinutes);
            if(diff < 60) 
                return $"{diff} M Ago";
            else {
                diff /= 60;
                if (diff < 24)
                    return $"{diff} H Ago";
                else {
                    diff /= 24;
                    if (diff < 7)
                        return $"{diff} D Ago";
                    else
                        return Time_created.ToString("yyyy/MM/dd");
                }
            }

        }


    }
}
