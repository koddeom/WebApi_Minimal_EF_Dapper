using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.Domain.Database;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Products
{
    public class ProductDelete
    {
        public static string Template => "Product/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
        public static Delegate Handle => Action;

        //-----------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona
        
        [SwaggerOperation(Tags = new[] { "Segmented Product" })]
        public static IResult Action([FromRoute] Guid id, ApplicationDbContext dbContext)
        {
            //Recupero o produto do banco
            var product = dbContext.Products.FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return Results.NotFound();
            }

            dbContext.Products.Remove(product);
            dbContext.SaveChanges();

            return Results.Ok();
        }
    }
}