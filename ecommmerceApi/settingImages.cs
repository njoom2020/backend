using EcommerceApiDataLayer;
using Shared.ImageDTOs;
using Shared.SettingImageDTOs;

namespace EcommerceApiBusinessLayer;

public class SettingImage
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;



    public int R_id { get; set; }
    public int Image_id { get; set; }
    public int Setting_id { get; set; }
    public int ImageIndex { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";


    public SettingImage(
                            int r_id,
                            int image_id,
                            int setting_id,
                            int imageIndex,
                            string title,
                            string body,
                            enMode cMode = enMode.AddNew
                            )
    {
        this.R_id = r_id;
        this.Image_id = image_id;
        this.Setting_id = setting_id;
        this.ImageIndex = imageIndex;
        this.Title = title;
        this.Body = body;
    }

    public static List<SettingImageDTO> getAllImageForSetting( int settingId, string baseUrl)
    {
        return SettingImageData.getAllSettingImages(settingId, baseUrl);
    }

}