using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.Categories.DTO;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Categories
{
    public class CategoryGet
    {
        public static string Template => "/Categorys/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //-----------------------------------------------------------------------
        //Observacao: IResult está trabalhando com uma operacao sincrona

        public static IResult Action([FromRoute] Guid id, ApplicationDbContext dbContext)
        {
            var Categorys = dbContext.Categories
                                     .AsNoTracking()
                                     .ToList();

            var categoryResponseDTO = Categorys.Select(p => new CategoryResponseDTO(
                                                        p.Id,
                                                        p.Name,
                                                        p.Active
                                                     ));

            return Results.Ok(categoryResponseDTO);
        }
    }
}