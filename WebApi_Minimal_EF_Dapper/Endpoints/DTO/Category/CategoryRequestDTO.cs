namespace WebApi_Minimal_EF_Dapper.Endpoints.DTO.Category
{
    public record CategoryRequestDTO(
        string Name,
        bool Active
    );
}