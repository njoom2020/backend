using Shared.ImageDTOs;
using Shared.SettingDTOs;
using Shared.SettingImageDTOs;

namespace EcommerceApiBusinessLayer;

public class Image
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public ImageDTO IDTO
    {
        get
        {
            return (new ImageDTO(this.Image_id, this.Image_fileName, this.Image_folderName));
        }
    }

    public int Image_id { get; set; }
    public string Image_fileName { get; set; } = "";
    public string Image_folderName { get; set; } = "";


    public Image(ImageDTO imageDto, enMode cMode = enMode.AddNew)
    {
        this.Image_id = imageDto.Image_id;
        this.Image_fileName = imageDto.Image_fileName;
        this.Image_folderName = imageDto.Image_folderName;

        Mode = cMode;
    }


    public static void DeleteOldImageFromStoreg(string imageName, string folderName)
    {
        if (string.IsNullOrWhiteSpace(imageName)) return;
        try
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string folderPath = Path.Combine(homePath,"AppImages",folderName);
            string fullPath = Path.Combine(folderPath, imageName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.Message}");
        }


    }

}