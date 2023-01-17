using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.Orders.DTO;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Orders
{
    public class OrderGet
    {
        public static string Template => "/orders/{id}";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //----------------------------------------------------------------------
        //Observacao: Task<IResult> Está trabalhando com uma operacao assincrona

        public static async Task<IResult> Action(Guid id,
                                                 HttpContext http,
                                                 ApplicationDbContext context
                                                )
        {
            //Usuario fixo, mas  poderia vir de um identity
            string userName = "doe joe";

            var order = context.Orders.FirstOrDefault(order => order.Id == id);

            var productsResponseDTO = order.Products.Select(p => new OrderProductDTO(p.Id,
                                                                                     p.Name));

            var orderResponseDTO = new OrderResponseDTO(order.Id,
                                                        userName,
                                                        productsResponseDTO
                                                        );

            return Results.Ok(orderResponseDTO);
        }
    }
}