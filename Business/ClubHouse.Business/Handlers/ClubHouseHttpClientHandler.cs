using ClubHouse.Common;
using ClubHouse.Common.Configurations;
using ClubHouse.Domain.Services;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ClubHouse.Business.Handlers {
    public class ClubHouseHttpClientHandler : HttpClientHandler {
        private readonly IAccountService _accountService;

        public ClubHouseHttpClientHandler(IAccountService accountService) {
            _accountService = accountService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            AuthConfiguration config = _accountService.CurrentConfig;
            if (config == null)
                config = _accountService.Reset();
            request.Headers.Add("CH-Languages", "en-US,fa-IR");
            request.Headers.Add("CH-Locale", "en-US");
            request.Headers.Add("CH-AppBuild", APIConsts.API_BUILD_ID);
            request.Headers.Add("CH-AppVersion", APIConsts.API_BUILD_VERSION);
            request.Headers.Add("User-Agent", APIConsts.API_UA);
            request.Headers.Add("Cookie", config.Cookie);
            request.Headers.Add("CH-UserID", config.UserId.ToString() ?? "(null)");
            if (!string.IsNullOrEmpty(config.UserToken))
                request.Headers.Add("Authorization", $"Token {config.UserToken}");
            request.Headers.Add("CH-DeviceId", $"Token {config.UserDevice}");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
