using EcommerceApiDataLayer;
using Shared.UserCartDTO;

namespace EcommerceApiBusinessLayer;

public class Cart
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public int Carts_id { get; set; }
    public int Carts_userId { get; set; }
    public int Carts_itemId { get; set; }
    public int Carts_quantity { get; set; }

    public string Items_name { get; set; }
    public string Items_name_ar { get; set; }
    public string Items_image { get; set; }
    public decimal Items_price { get; set; }
    public int Items_discount { get; set; }
    public int Items_count { get; set; }
    public bool Items_active { get; set; }
    public bool Is_favorite { get; set; }
    public DateTime Carts_created { get; set; }

    public Cart()
    {
        Carts_id = -1;
        Carts_userId = -1;
        Carts_itemId = -1;
        Carts_quantity = -1;
        Items_name = "";
        Items_name_ar = "";
        Items_image = "";
        Items_price = 0;
        Items_discount = 0;
        Items_count = 0;
        Items_active = false;
        Is_favorite = false;
        Carts_created = DateTime.UtcNow;

        Mode = enMode.AddNew;
    }

    public Cart(int carts_id, int carts_userId, int carts_itemId, int carts_quantity, string items_name, 
    string items_name_ar, string items_image, decimal items_price,int items_discount,int items_count,
    bool items_active, bool is_favorite , DateTime carts_created, enMode cMode = enMode.AddNew)
    {
        Carts_id = carts_id;
        Carts_userId = carts_userId;
        Carts_itemId = carts_itemId;
        Carts_quantity = carts_quantity;
        Items_name = items_name;
        Items_name_ar = items_name_ar;
        Items_image = items_image;
        Items_price = items_price;
        Items_discount = items_discount;
        Items_count = items_count;
        Items_active = items_active;
        Is_favorite = is_favorite;
        Carts_created = carts_created;

        Mode = cMode;

    }

    public static List<UserCartDTO> getUserCarts(int userId, string baseUrl)
    {
        return CartData.getUserCarts(userId, baseUrl);
    }

    public static ManageCartResultDto addSubtractQuantityAndAddNewOrUpdateOrDelete(CartDataRequestDto requestDto)
    {
        return CartData.addSubtractQuantityAndAddNewOrUpdateOrDelete(requestDto);
    }

    public static bool addNewUpateCarts(int userId, int itemId, int quantity)
    {
        return CartData.addUpdateCarts(userId, itemId, quantity);
    }

}
