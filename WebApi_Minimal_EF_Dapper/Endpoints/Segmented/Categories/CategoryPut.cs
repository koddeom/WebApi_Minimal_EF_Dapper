using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.AppDomain.Extensions.ErroDetailedExtension;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Category;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Categories
{
    public class CategoryPut
    {
        public static string Template => "Category/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
        public static Delegate Handle => Action;

        //-----------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona
        
        [SwaggerOperation(Tags = new[] { "Segmented Category" })]
        public static IResult Action([FromRoute] Guid id,
                                                 CategoryRequestDTO categoryRequestDTO,
                                                 HttpContext http,
                                                 ApplicationDbContext dbContext)
        {
            //Usuario fixo, mas  poderia vir de um identity
            string user = "doe joe";

            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return Results.NotFound();
            }

            category.EditInfo(categoryRequestDTO.Name,
                              categoryRequestDTO.Active,
                              user);

            if (!category.IsValid)
            {
                return Results.ValidationProblem(category.Notifications
                                                         .ConvertToErrorDetails());
            }

            dbContext.SaveChanges();

            return Results.Ok();
        }
    }
}