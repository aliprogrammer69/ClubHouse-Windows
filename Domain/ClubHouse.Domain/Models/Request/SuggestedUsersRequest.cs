namespace ClubHouse.Domain.Models.Request {
    public class SuggestedUsersRequest : PagingRequest {
        public bool In_onboarding { get; set; } = true;

        public override string ToString() {
            return $"in_onboarding={In_onboarding}&page_size={Page_size}&page={Page}";
        }
    }
}
