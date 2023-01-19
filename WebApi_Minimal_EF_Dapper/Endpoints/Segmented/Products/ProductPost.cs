using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.AppDomain.Extensions.ErroDetailedExtension;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Domain.Database.Entities.Product;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Product;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Products
{
    public class ProductPost
    {
        public static string Template => "Product";
        public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        //----------------------------------------------------------------------
        //Observacao: Task<IResult> Está trabalhando com uma operacao assincrona

        [SwaggerOperation(Tags = new[] { "Segmented Product" })]
        public static async Task<IResult> Action(ProductRequestDTO productRequestDTO,
                                                 HttpContext http,
                                                 ApplicationDbContext dbContext)
        {
            //Usuario fixo, mas  poderia vir de um identity
            string user = "doe joe";

            //Recupero a categoria de forma sincrona
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == productRequestDTO.CategoryId);

            var product = new Product();

            product.AddProduct(productRequestDTO.Name,
                                productRequestDTO.Description,
                                productRequestDTO.Price,
                                true,
                                category,
                                user
                                );

            if (!product.IsValid)
            {
                return Results.ValidationProblem(product.Notifications.ConvertToErrorDetails());
            }

            await dbContext.Products.AddAsync(product);
            dbContext.SaveChanges();

            return Results.Created($"/products/{product.Id}", product.Id);
        }
    }
}