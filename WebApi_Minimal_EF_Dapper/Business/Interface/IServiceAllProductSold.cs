using WebApi_Minimal_EF_Dapper.Business.Models;

namespace WebApi_Minimal_EF_Dapper.Business.Interface
{
    public interface IServiceAllProductSold
    {
        Task<IEnumerable<ProductSold>> Execute();
    }
}