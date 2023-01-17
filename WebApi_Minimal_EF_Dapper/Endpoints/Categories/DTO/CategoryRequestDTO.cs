namespace WebApi_Minimal_EF_Dapper.Endpoints.Categories.DTO
{
    public record CategoryRequestDTO(
        String Name,
        bool Active
    );
}