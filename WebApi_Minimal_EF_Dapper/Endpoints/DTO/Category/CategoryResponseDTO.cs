namespace WebApi_Minimal_EF_Dapper.Endpoints.DTO.Category
{
    public record CategoryResponseDTO(
     Guid Id,
     string Name,
     bool Active
    );

    //public class CategoryResponseDTO
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public bool Active { get; set; }
    //}
}