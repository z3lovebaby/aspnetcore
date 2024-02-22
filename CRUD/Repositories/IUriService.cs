using CRUD.Filter;

namespace CRUD.Repositories
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
