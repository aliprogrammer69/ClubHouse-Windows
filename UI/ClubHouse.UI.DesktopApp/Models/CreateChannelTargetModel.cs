using ClubHouse.Domain;

namespace ClubHouse.UI.DesktopApp.Models {
    public class CreateChannelTargetModel {
        public long? EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsSocialable { get; set; }
    }
}
