namespace ClubHouse.Domain.Models {
    public class BaseUserInfo {
        public long User_id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string First_name { get; set; }
        public string Photo_url { get; set; }
        public string ThumbnailPhotoUrl { get => string.IsNullOrEmpty(Photo_url) || Photo_url.EndsWith("_thumbnail_250x250") ? Photo_url : $"{Photo_url}_thumbnail_250x250"; }
        public string Bio { get; set; }
    }
}
