using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Product;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Products
{
    public class ProductGetAll
    {
        public static string Template => "Product";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //---------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona

        [SwaggerOperation(Tags = new[] { "Segmented Product" })]
        public static IResult Action(ApplicationDbContext dbContext)
        {
            var products = dbContext.Products
                                  .AsNoTracking()
                                  .Include(p => p.Category)
                                  .OrderBy(p => p.Name)
                                  .ToList();

            var productsResponseDTO = products.Select(p => new ProductResponseDTO(
                                                        p.Id,
                                                        p.Name,
                                                        p.Description,
                                                        p.Price,
                                                        p.Active
                                                     ));

            return Results.Ok(productsResponseDTO);
        }
    }
}