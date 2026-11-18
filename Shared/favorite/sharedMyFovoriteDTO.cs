namespace Shared.MyFavoriteDTO;

public class MyFavoriteDTO
{
    public MyFavoriteDTO() { }
    public MyFavoriteDTO(int favoriteId, int userId, int itemId, string name, string nameAr, string desc, string descAr, string image, int count,
               decimal price, int discount, DateTime created, int categoryId, bool isFavorite, bool active = true)
    {

        FavoriteId = favoriteId;
        UserId = userId;
        ItemId = itemId;
        Name = name;
        NameAr = nameAr;
        Desc = desc;
        DescAr = descAr;
        Image = image;
        Count = count;
        Active = active;
        Price = price;
        Discount = discount;
        Created = created;
        CategoryId = categoryId;
        IsFavorite = isFavorite;

    }
    public int FavoriteId { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string DescAr { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Count { get; set; }
    public bool Active { get; set; } = true;
    public decimal Price { get; set; }
    public int Discount { get; set; }
    public DateTime Created { get; set; }
    public int CategoryId { get; set; }
    public bool IsFavorite { get; set; } 

}