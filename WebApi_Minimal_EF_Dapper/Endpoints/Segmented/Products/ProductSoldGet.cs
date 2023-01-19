using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.Business;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Products
{
    public class ProductSoldGet
    {
        public static string Template => "Product/sold";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //----------------------------------------------------------------------
        //Observacao: Task<IResult> Está trabalhando com uma operacao assincrona

        [SwaggerOperation(Tags = new[] { "Segmented Product" })]
        public static async Task<IResult> Action(ServiceAllProductsSold serviceAllProductsSold)
        {
            var result = await serviceAllProductsSold.Execute();

            return Results.Ok(result);
        }
    }
}