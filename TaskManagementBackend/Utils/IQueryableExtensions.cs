using Microsoft.CodeAnalysis.CSharp.Syntax;
using TaskManagementBackend.DTOs;

namespace TaskManagementBackend.Utils
{
    public static class IQueryableExtensions
    {
       public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                .Take(pagination.RecordsPerPage);
        }
    }
}
