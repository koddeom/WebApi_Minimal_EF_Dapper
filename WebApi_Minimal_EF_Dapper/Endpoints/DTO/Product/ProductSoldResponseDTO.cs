namespace WebApi_Minimal_EF_Dapper.Endpoints.DTO.Product
{
    public record ProductSoldResponseDTO(
         Guid Id,
         string Name,
         int Amount
        );
}