using agorartc;

using ClubHouse.Business.Handlers;
using ClubHouse.Business.Services;
using ClubHouse.Business.Services.Common;
using ClubHouse.Common.Configurations;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

using Prism.Ioc;

using Unity;

namespace ClubHouse.Bootstrapper.Unity {
    public class PrismUnityContainerConfigurationManager : IContanerRegisteryConfigurationManager {
        public IContanerRegisteryConfigurationManager RegisterRepositories(IContainerRegistry service) {
            return this;
        }

        public IContanerRegisteryConfigurationManager RegisterServices(IContainerRegistry service) {
            service.RegisterSingleton<IAccountService, AccountService>()
                   .RegisterSingleton<IAuthService, AuthService>()
                   .RegisterSingleton<IChannelService, ChannelService>()
                   .RegisterSingleton<IClubService, ClubService>()
                   .RegisterSingleton<IEventService, EventService>()
                   .RegisterSingleton<IProfileService, ProfileService>()
                   .RegisterSingleton<ITopicService, TopicService>()
                   .RegisterSingleton<IUserService, UserService>()
                   .Register<AuthConfiguration>(p => p.Resolve<IAccountService>().CurrentConfig);
            return this;
        }

        public IContanerRegisteryConfigurationManager RegisterUtils(IContainerRegistry service) {
            service.RegisterSingleton<ISerializer>(p => new NewtonSoftSerializer())
                   .RegisterSingleton<ClubHouseHttpClientHandler>()
                   .RegisterSingleton<IHttpClient>(p => new NetHttpClient(p.Resolve<ISerializer>(), p.Resolve<ClubHouseHttpClientHandler>()))
                   .Register<AgoraRtcEngine>(p => AgoraRtcEngine.CreateRtcEngine());
            return this;
        }
    }
}
