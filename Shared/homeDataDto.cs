using Shared.CategoryDTOs;
using Shared.ItemDTOs;
using Shared.SettingDTOs;

public class HomeDataDTO
{
    public List<ItemDTO> offerItems { get; set; } = new List<ItemDTO>();
    public List<CategoryDTO> categories { get; set; } = new List<CategoryDTO>();
    public List<ItemDTO> topSelling { get; set; } = new List<ItemDTO>();
    public SettingDTO lastSetting { get; set; } = new SettingDTO();
}