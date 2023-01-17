namespace WebApi_Minimal_EF_Dapper.Endpoints.Orders.DTO
{
    public record OrderRequestDTO(
        List<Guid> ProductListIds
    );
}