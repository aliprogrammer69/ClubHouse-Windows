using ClubHouse.Domain.Models.Response;

using System.Threading.Tasks;

namespace ClubHouse.Domain.Services {
    public interface IAuthService {
        Task<StartPhoneNumberAuthResponse> StartPhoneNumberAuth(string phoneNumber);
        Task<BaseResponse> CallPhoneNumberAuth(string phoneNumber);
        Task<object> ResendPhoneNumberAuth(string phoneNumber);
        Task<CompletePhoneNumberAuthResponse> CompletePhoneNumberAuth(string phoneNumber, string verificationCode);
        Task<object> CheckForUpdate(bool isTestflight = false);
        void Logout();
    }
}
