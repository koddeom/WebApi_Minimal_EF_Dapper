using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.Products.DTO;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Products
{
    public class ProductGet
    {
        public static string Template => "/products/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //-----------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona

        public static IResult Action([FromRoute] Guid id, ApplicationDbContext dbContext)
        {
            var products = dbContext.Products
                                   .Include(p => p.Category)
                                   .Where(p => p.Id == id)
                                   .OrderBy(p => p.Name)
                                   .ToList();

            var productResponseDTO = products.Select(p => new ProductResponseDTO(
                                                        p.Id,
                                                        p.Name,
                                                        p.Description,
                                                        p.Price,
                                                        p.Active
                                                     ));

            return Results.Ok(productResponseDTO);
        }
    }
}