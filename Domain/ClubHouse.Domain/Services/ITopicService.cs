using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;

namespace ClubHouse.Domain.Services {
    public interface ITopicService {
        Task<object> Get();
        Task<object> Get(long topicId);
        Task<object> GetClubs(TopicPagingRequest request);
        Task<object> GetUsers(TopicPagingRequest request);

    }
}
