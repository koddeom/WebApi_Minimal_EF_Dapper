using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.Business;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Products
{
    public class ProductSoldGet
    {
        public static string Template => "/products/sold";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //----------------------------------------------------------------------
        //Observacao: Task<IResult> Está trabalhando com uma operacao assincrona

        public static async Task<IResult> Action(ServiceAllProductsSold serviceAllProductsSold)
        {
            var result = await serviceAllProductsSold.Execute();

            return Results.Ok(result);
        }
    }
}