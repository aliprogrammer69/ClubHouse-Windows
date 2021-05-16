using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;

namespace ClubHouse.Domain.Services {
    public interface IEventService {
        Task<object> Get(EventRequest query);
        Task<GetEventListResponse> GetList(GetEventListRequest request);
        Task<object> GetToStart();
        Task<object> GetForUser();
        Task<object> Create(EventRequest model);
        Task<object> Edit(EventRequest model);
        Task<object> Delete(EventRequest model);
    }
}
