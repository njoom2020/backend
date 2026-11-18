using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using EcommerceApiDataLayer;
using Shared.ItemAdminDTOs;
using Shared.ItemDTOs;

namespace EcommerceApiBusinessLayer;

public class Item
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;
    public ItemDTO IDTO
    {
        get
        {
            return (new ItemDTO(this.Id, this.Name, this.NameAr, this.Desc, this.DescAr,
             this.Image, this.Count, this.Price, this.Discount, this.CategoryId, this.IsFavorite));
        }
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameAr { get; set; }
    public string Desc { get; set; }
    public string DescAr { get; set; }
    public string Image { get; set; }
    public int Count { get; set; }
    public bool Active { get; set; }
    public decimal Price { get; set; }
    public int Discount { get; set; }
    public DateTime Creadet { get; set; }
    public int CategoryId { get; set; } = 0;
    public bool IsFavorite { get; set; } = false;


    public Item(ItemDTO IDto, DateTime? created = null, enMode cMode = enMode.AddNew)
    {
        this.Id = IDto.Id;
        this.Name = IDto.Name;
        this.NameAr = IDto.NameAr;
        this.Desc = IDto.Desc;
        this.DescAr = IDto.DescAr;
        this.Image = IDto.Image;
        this.Count = IDto.Count;
        this.Active = IDto.IsActive;
        this.Price = IDto.Price;
        this.Discount = IDto.Discount;
        this.Creadet = created ?? DateTime.UtcNow;
        this.CategoryId = IDto.CategoryId;
        this.IsFavorite = IDto.IsFavorite;

        Mode = cMode;
    }
    public Item()
    {
        this.Id = -1;
        this.Name = "";
        this.NameAr = "";
        this.Desc = "";
        this.DescAr = "";
        this.Image = "";
        this.Count = -1;
        this.Active = false;
        this.Price = 0;
        this.Discount = 0;
        this.Creadet = DateTime.UtcNow;
        this.CategoryId = -1;
        this.IsFavorite = false;

    }
    public static List<ItemDTO> GetAllItems(string? baseUrl = null, bool isOffers = false)
    {
        var ItemsList = ItemData.GetAllItems(isOffers);
        if (string.IsNullOrWhiteSpace(baseUrl))
            return ItemsList;

        foreach (var item in ItemsList)
            if (!string.IsNullOrWhiteSpace(item.Image))
                item.Image = $"{baseUrl}/api/ItemApi/image/{item.Image}";

        return ItemsList;
    }

    public static List<ItemDTO> GetTopSelling(string? baseUrl = null)
    {
        var itemList = ItemData.GetTopSellingItems();
        if (string.IsNullOrWhiteSpace(baseUrl))
            return itemList;

        foreach (var item in itemList)
            item.Image = $"{baseUrl}/api/ItemApi/image/{item.Image}";

        return itemList;
    }

    public static List<ItemDTO> getAllItemsByCategoryId(int categoryId, string? baseUrl = null)
    {
        var itemsList = ItemData.getAllItemsByCategoryId(categoryId);
        if (string.IsNullOrWhiteSpace(baseUrl))
            return itemsList;

        foreach (var item in itemsList)
            if (!string.IsNullOrWhiteSpace(item.Image))
                item.Image = $"{baseUrl}/api/ItemApi/image/{item.Image}";

        return itemsList;
    }


    public static List<ItemDTO> getItemsByCategoryId(int categoryId, int userId, string baseUrl, int pageNumber = 1, int pageSize = 30)
    {

        return ItemData.getItemsByCategoryIdWithPagination(categoryId, userId, pageNumber, pageSize, baseUrl);

    }

    public static List<ItemDTO> searchItems(int userId, string searchText, string baseUrl, int categoryId = 0)
    {
        return ItemData.searchItems(userId, searchText, baseUrl, categoryId);
    }

    private bool _AddNew()
    {
        this.Id = ItemData.AddNewItem(this.IDTO);
        return this.Id > 0;
    }

    public static Item? findItemByitemId(int itemId)
    {
        ItemDTO itemDto = new ItemDTO();
        itemDto.Id = itemId;
        DateTime created = DateTime.UtcNow;

        bool isFound = ItemData.findItemById(ref itemDto,ref created);
        if(isFound)
            return new Item(itemDto, created, enMode.Update);
        else
            return null;
    }

    private bool _update()
    {
        return ItemData.updateItemInfo(this.IDTO);
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNew())
                {
                    Mode = enMode.Update;
                    return true;
                }
                return false;

            case enMode.Update:
                return _update();

        }
        return false;
    }




    // ------------------------------- admin ________

    public static List<ItemAdminDTO> getAllItemsForAdmin(string? baseUrl = null)
    {
        return ItemData.getAllItems(baseUrl);
    }

    public static ItemInfoAdminDTO? getItemInfoById(int itemId)
    {
        return ItemData.getItemInfo(itemId);
    }

    public static bool deleteItemByItemId(int itemId)
    {
        return ItemData.deleteItem(itemId);
    }



}