using Prism.Ioc;

namespace ClubHouse.Bootstrapper.Unity {
    public interface IContanerRegisteryConfigurationManager {
        IContanerRegisteryConfigurationManager RegisterRepositories(IContainerRegistry service);
        IContanerRegisteryConfigurationManager RegisterServices(IContainerRegistry service);
        IContanerRegisteryConfigurationManager RegisterUtils(IContainerRegistry service);
    }
}
