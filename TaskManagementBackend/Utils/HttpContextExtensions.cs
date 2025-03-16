using Microsoft.EntityFrameworkCore;

namespace TaskManagementBackend.Utils
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Append("cantidad-total-registros", count.ToString());
        }
    }
}
