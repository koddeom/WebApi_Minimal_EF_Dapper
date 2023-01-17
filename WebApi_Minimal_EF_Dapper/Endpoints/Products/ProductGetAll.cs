using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.Products.DTO;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Products
{
    public class ProductGetAll
    {
        public static string Template => "/products";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //---------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona

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