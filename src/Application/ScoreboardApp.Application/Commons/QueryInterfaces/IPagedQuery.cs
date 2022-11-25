namespace ScoreboardApp.Application.Commons.Queries
{
    public interface IPagedQuery
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
