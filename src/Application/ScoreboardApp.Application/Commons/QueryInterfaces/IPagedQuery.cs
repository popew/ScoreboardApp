namespace ScoreboardApp.Application.Commons.Queries
{
    public interface IPaginationQuery
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
