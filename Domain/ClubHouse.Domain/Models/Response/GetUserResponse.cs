namespace ClubHouse.Domain.Models.Response {
    public class GetUserResponse : BaseResponse {
        public UserInfo User_profile { get; set; }
    }
}
