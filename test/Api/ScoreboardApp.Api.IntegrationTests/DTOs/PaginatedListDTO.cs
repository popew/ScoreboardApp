
namespace ScoreboardApp.Api.IntegrationTests.DTOs
{
    public class PaginatedListDTO<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
