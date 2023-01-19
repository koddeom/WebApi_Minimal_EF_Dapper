namespace WebApi_Minimal_EF_Dapper.Endpoints.DTO.Product
{
    public record ProductResponseDTO(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        bool Active
    );
}