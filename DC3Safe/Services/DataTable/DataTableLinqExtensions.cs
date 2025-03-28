using DC3Safe.Models;
using Microsoft.EntityFrameworkCore;

namespace DC3Safe.Services.DataTable
{
    public static class DataTableLinqExtensions
    {
        public static async Task<DataTableResult<T>> ToDataTableAsync<T>(this IQueryable<T> query,
            DataTableRequest request, int recordsTotal) where T : class
        {
            var result = new DataTableResult<T>();
            result.draw = request.draw;
            result.recordsTotal = recordsTotal;
            result.recordsFiltered = await query.CountAsync();
            result.data = await query.Skip(request.start)
                .Take(request.length).ToListAsync();

            return result;
        }
    }
}
