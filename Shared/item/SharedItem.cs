namespace Shared.ItemDTOs;

public class ItemDTO
{
    public ItemDTO()
    {

        this.Id = -1;
        this.Name = "";
        this.NameAr = "";
        this.Desc = "";
        this.DescAr = "";
        this.Image = "";
        this.Count = -1;
        this.Price = 0;
        this.Discount = 0;
        this.CategoryId = -1;
        this.IsFavorite = false;
        this.IsActive = false;


    }

    public ItemDTO(int id, string name, string nameAr, string desc,
         string descAr, string image, int count, decimal price, int discount,
         int categoryId = 0,
         bool isFavorite = false,
         bool isActive = true
         )
    {
        this.Id = id;
        this.Name = name;
        this.NameAr = nameAr;
        this.Desc = desc;
        this.DescAr = descAr;
        this.Image = image;
        this.Count = count;
        this.Price = price;
        this.Discount = discount;
        this.CategoryId = categoryId;
        this.IsFavorite = isFavorite;
        this.IsActive = isActive;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string NameAr { get; set; }
    public string Desc { get; set; } = "";
    public string DescAr { get; set; } = "";
    public string Image { get; set; } = "noImage.png";
    public int Count { get; set; }
    public decimal Price { get; set; }
    public int Discount { get; set; } = 0;
    public int CategoryId { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool IsActive { get; set; } = true;


}



