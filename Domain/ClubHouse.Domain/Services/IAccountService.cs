
using ClubHouse.Common.Configurations;

namespace ClubHouse.Domain.Services {
    public interface IAccountService {
        bool Authenticated { get; }

        AuthConfiguration CurrentConfig { get; }

        void Set(AuthConfiguration config);

        AuthConfiguration Reset();
    }
}
