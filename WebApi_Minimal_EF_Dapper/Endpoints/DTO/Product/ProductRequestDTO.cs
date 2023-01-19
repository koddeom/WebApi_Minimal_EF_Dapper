namespace WebApi_Minimal_EF_Dapper.Endpoints.DTO.Product
{
    public record ProductRequestDTO(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        Guid CategoryId
    );
}