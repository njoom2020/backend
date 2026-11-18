namespace Shared.ItemImageDTOs;

public class ItemImageDTO
{

 public   ItemImageDTO(
                int r_id,
                int item_id,
                int image_id,
                int imageIndex,
                string image_fileName,
                string image_folderName
                 )
    {

        this.R_id = r_id;
        this.Item_id = item_id;
        this.Image_id = image_id;
        this.ImageIndex = imageIndex;
        this.Image_fileName = image_fileName;
        this.Image_folderName = image_folderName;
    }
    public int R_id { get; set; }
    public int Item_id { get; set; }
    public int Image_id { get; set; }
    public int ImageIndex { get; set; }
    public string Image_fileName { get; set; } = "";
    public string Image_folderName { get; set; } = "";
}
