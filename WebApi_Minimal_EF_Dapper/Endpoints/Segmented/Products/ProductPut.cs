using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.AppDomain.Extensions.ErroDetailedExtension;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Product;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Products
{
    public class ProductPut
    {
        public static string Template => "Product/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
        public static Delegate Handle => Action;

        //---------------------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona

        [SwaggerOperation(Tags = new[] { "Segmented Product" })]
        public static IResult Action([FromRoute] Guid id,
                                                 ProductRequestDTO productRequestDTO,
                                                 HttpContext http,
                                                 ApplicationDbContext dbContext)
        {
            //Usuario fixo, mas  poderia vir de um identity
            string user = "doe joe";

            //Recupero o produto do banco
            var product = dbContext.Products.FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return Results.NotFound();
            }

            //Recupero a categoria de forma sincrona
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == productRequestDTO.CategoryId);

            if (category == null)
            {
                return Results.NotFound();
            }

            product.EditProduct(productRequestDTO.Name,
                                productRequestDTO.Price,
                                true,
                                category,
                                user
                                );

            if (!product.IsValid)
            {
                return Results.ValidationProblem(product.Notifications.ConvertToErrorDetails());
            }

            dbContext.SaveChanges();

            return Results.Ok();
        }
    }
}