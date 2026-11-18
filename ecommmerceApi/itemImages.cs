using EcommerceApiDataLayer;
using Shared.ImageDTOs;
using Shared.ItemImageDTOs;
using Shared.SettingImageDTOs;

namespace EcommerceApiBusinessLayer;

public class ItemImage
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;



    public int R_id { get; set; }
    public int Item_id { get; set; }
    public int Image_id { get; set; }
    public int ImageIndex { get; set; }


    public ItemImage(
                            int r_id,
                            int image_id,
                            int item_id,
                            int imageIndex,
                            enMode cMode = enMode.AddNew
                            )
    {
        this.R_id = r_id;
        this.Item_id = item_id;
        this.Image_id = image_id;
        this.ImageIndex = imageIndex;
    }

    public static List<ItemImageDTO> getAllImagesForItem(int itemId, string baseUrl)
    {
        return ItemImageData.getAllItemImages(itemId, baseUrl);
    }

}