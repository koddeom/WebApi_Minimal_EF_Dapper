using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.AppDomain.Extensions.ErroDetailedExtension;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Domain.Database.Entities.Product;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Category;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Categories
{
    public class CategoryPost
    {
        public static string Template => "Category";
        public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        //----------------------------------------------------------------------
        //Observacao: Task<IResult> Está trabalhando com uma operacao assincrona
        
        [SwaggerOperation(Tags = new[] { "Segmented Category" })]
        public static async Task<IResult> Action(CategoryRequestDTO categoryRequestDTO,
                                                 HttpContext http,
                                                 ApplicationDbContext dbContext)
        {
            //Usuario fixo, mas  poderia vir de um identity
            string user = "doe joe";

            var category = new Category();

            category.AddCategory(categoryRequestDTO.Name,
                                  user);

            if (!category.IsValid)
            {
                return Results.ValidationProblem(category.Notifications.ConvertToErrorDetails());
            }

            await dbContext.Categories.AddAsync(category);
            dbContext.SaveChanges();

            return Results.Created($"/categories/{category.Id}", category.Id);
        }
    }
}