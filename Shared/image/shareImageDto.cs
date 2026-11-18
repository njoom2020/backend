namespace Shared.ImageDTOs;
public class ImageDTO
{
    public ImageDTO(int image_id, string image_fileName,string image_folderName)
    {
        this.Image_id = image_id;
        this.Image_fileName = image_fileName;
        this.Image_folderName = image_folderName;
    }
    public int Image_id {get; set;}
    public string Image_fileName {get;set;} = "";
    public string Image_folderName {get;set;} = "";

}
