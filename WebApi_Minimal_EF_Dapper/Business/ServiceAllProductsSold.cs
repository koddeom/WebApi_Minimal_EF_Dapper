using Dapper;
using Microsoft.Data.SqlClient;
using WebApi_Minimal_EF_Dapper.Business.Models;

namespace WebApi_Minimal_EF_Dapper.Business
{
    public class ServiceAllProductsSold
    {
        public ServiceAllProductsSold(IConfiguration configuration)
        {
            //Necessario para recuperar as configuracoes de conexao para
            //usar com Dapper
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        public async Task<IEnumerable<ProductSold>> Execute()
        {
            var db = new SqlConnection(configuration["Database:SQlServer"]);

            var query = @" SELECT A.ID,
                              B.NAME,
                              COUNT(*) AMOUNT
                         FROM ORDERS O
                        INNER JOIN ORDERPRODUCTS A ON
                              O.ID = A.ORDERSID
                        INNER JOIN PRODUCTS B ON
                              B.OD = A.PRODUCTSID
                        GROUP BY P.ID, P.NAME
                        ORDER BY AMOUNT DESC";

            return await db.QueryAsync<ProductSold>(query);
        }
    }
}