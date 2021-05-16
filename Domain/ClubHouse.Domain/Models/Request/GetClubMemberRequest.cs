namespace ClubHouse.Domain.Models.Request {
    public class GetClubMemberRequest : PagingRequest {
        public long ClubId { get; set; }
        public bool ReturnFollowers { get; set; } = false;
        public bool ReturnMembers { get; set; } = true;

        public override string ToString() => $"club_id={ClubId}&return_followers={ReturnFollowers}&return_members={ReturnMembers}&page_size={Page_size}&page={Page}";
    }
}
