namespace ClubHouse.Domain.Models.Common {
    public class HttpResponse<T> : HttpResponse {
        public T Body { get; set; }
    }
}
