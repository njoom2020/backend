namespace Shared.SettingImageDTOs;

public class SettingImageDTO
{

 public   SettingImageDTO(
                int r_id,
                int setting_id,
                int image_id,
                int imageIndex,
                string image_fileName,
                string image_folderName,
                string iitle,
                string body
                 )
    {

        this.R_id = r_id;
        this.Setting_id = setting_id;
        this.Image_id = image_id;
        this.ImageIndex = imageIndex;
        this.Image_fileName = image_fileName;
        this.Image_folderName = image_folderName;
        this.Title = iitle;
        this.Body = body;
    }
    public int R_id { get; set; }
    public int Setting_id { get; set; }
    public int Image_id { get; set; }
    public int ImageIndex { get; set; }
    public string Image_fileName { get; set; } = "";
    public string Image_folderName { get; set; } = "";
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
}
