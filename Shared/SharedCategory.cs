
namespace Shared.CategoryDTOs;
public class CategoryDTO
{
    public CategoryDTO(int id, string name, string name_ar, string? image = null)
    {
        this.Id = id;
        this.Name = name;
        this.Name_ar = name_ar;
        this.image = image;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Name_ar { get; set; }
    public string? image {get; set;} = "";

}