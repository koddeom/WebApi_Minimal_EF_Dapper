using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Category;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Categories
{
    public class CategoryGetAll
    {
        public static string Template => "Category";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //-----------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona
        
        [SwaggerOperation(Tags = new[] { "Segmented Category" })]
        public static IResult Action(ApplicationDbContext dbContext)
        {
            var categories = dbContext.Categories
                                      .AsNoTracking()
                                      .ToList();

            var categoriesResponseDTO = categories.Select(c => new CategoryResponseDTO
            (
                c.Id,
                c.Name,
                c.Active
            ));

            return Results.Ok(categoriesResponseDTO);
        }
    }
}