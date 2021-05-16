using ClubHouse.Common;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

using System.Threading.Tasks;

namespace ClubHouse.Business.Services {
    public class AuthService : IAuthService {
        private readonly IHttpClient _httpClient;
        private readonly IAccountService _accountService;

        public AuthService(IHttpClient httpClient, IAccountService accountService) {
            _httpClient = httpClient;
            _accountService = accountService;
        }

        public Task<StartPhoneNumberAuthResponse> StartPhoneNumberAuth(string phoneNumber) =>
            _httpClient.PostAsync<object, StartPhoneNumberAuthResponse>($"{APIConsts.API_URL}/start_phone_number_auth", new {
                phone_number = phoneNumber
            });

        public Task<BaseResponse> CallPhoneNumberAuth(string phoneNumber) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/call_phone_number_auth", new {
                phone_number = phoneNumber
            });

        public Task<object> ResendPhoneNumberAuth(string phoneNumber) =>
            _httpClient.PostAsync<object, object>($"{APIConsts.API_URL}/resend_phone_number_auth", new {
                phone_number = phoneNumber
            });

        public async Task<CompletePhoneNumberAuthResponse> CompletePhoneNumberAuth(string phoneNumber, string verificationCode) {
            var response = await _httpClient.PostAsync<object, CompletePhoneNumberAuthResponse>($"{APIConsts.API_URL}/complete_phone_number_auth", new {
                phone_number = phoneNumber,
                verification_code = verificationCode
            });
            if (!string.IsNullOrEmpty(response?.Auth_token)) {
                var config = _accountService.CurrentConfig;
                config.UserToken = response.Auth_token;
                config.UserId = response.User_profile.User_id;
                config.RefereshToken = response.Refresh_token;
                _accountService.Set(config);
            }
            return response;
        }

        public Task<object> CheckForUpdate(bool isTestflight = false) =>
            _httpClient.GetAsync<object>($"{APIConsts.API_URL}/check_for_update?is_testflight={(isTestflight ? "1" : "0")}");

        public void Logout() {
            _accountService.CurrentConfig.UserToken = null;
            _accountService.CurrentConfig.UserId = null;
            _accountService.CurrentConfig.RefereshToken = null;
            _accountService.Set(_accountService.CurrentConfig);
        }
    }
}
