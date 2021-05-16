using Prism.Ioc;

namespace ClubHouse.Bootstrapper.Unity.Extensions {
    public static class PrismUnityConllectionExtension {
        public static IContainerRegistry AddServices(this IContainerRegistry services,
                                                     IContanerRegisteryConfigurationManager configurationManager = null) {
            configurationManager = configurationManager ?? new PrismUnityContainerConfigurationManager();
            configurationManager.RegisterRepositories(services)
                                .RegisterServices(services)
                                .RegisterUtils(services);
            return services;
        }
    }
}
