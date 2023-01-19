using Swashbuckle.AspNetCore.Annotations;
using WebApi_Minimal_EF_Dapper.AppDomain.Extensions.ErroDetailedExtension;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Domain.Database.Entities.Product;
using WebApi_Minimal_EF_Dapper.Endpoints.DTO.Order;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Segmented.Orders
{
    public class OrderPost
    {
        public static string Template => "Order";
        public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        //----------------------------------------------------------------------
        //Observacao: Task<IResult> Está trabalhando com uma operacao assincrona
        
        [SwaggerOperation(Tags = new[] { "Segmented Order" })]
        public static async Task<IResult> Action(OrderRequestDTO orderRequestDTO,
                                                 HttpContext http,
                                                 ApplicationDbContext dbContext)
        {
            //Usuario fixo, mas  poderia vir de um identity
            var userId = "123456";
            var userName = "Doe Joe Client";

            var products = new List<Product>();

            List<Product> orderProducts = new List<Product>();

            if (orderRequestDTO.ProductListIds.Any())

                orderProducts = dbContext.Products.Where(p => orderRequestDTO.ProductListIds
                                                                           .Contains(p.Id))
                                                                           .ToList();
            if (orderProducts == null)
            {
                return Results.NotFound();
            }

            var order = new Order();

            order.AddOrder(userId, userName, orderProducts);

            if (!order.IsValid)
            {
                return Results.ValidationProblem(order.Notifications.ConvertToErrorDetails());
            }

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/orders/{order.Id}", order.Id);
        }
    }
}