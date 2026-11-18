using EcommerceApiDataLayer;
using Shared.FavoriteDTO;
using Shared.ItemDTOs;
using Shared.MyFavoriteDTO;

namespace EcommerceApiBusinessLayer;

public class Favorite
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;


    public FavoriteDTO FDTO
    {
        get
        {
            return new FavoriteDTO(this.Id, this.UserId, this.ItemId);
        }
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }

    private Favorite(FavoriteDTO fTdo, enMode cMode = enMode.AddNew)
    {
        this.Id = fTdo.Id;
        this.UserId = fTdo.UserId;
        this.ItemId = fTdo.ItemId;

        Mode = cMode;
    }
    public Favorite()
    {

    }

    private bool _addNew()
    {
        this.Id = FavoriteData.addNewFavorite(this.UserId, this.ItemId);
        return this.Id != -1;
    }

    public bool save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                Mode = enMode.Update;
                return _addNew();

        }
        return false;
    }

    public static bool deleteFavorite(int userId, int itemId)
    {
        return FavoriteData.deleteFavorite(userId, itemId);
    }

    public static List<MyFavoriteDTO> getUserFavoriteItems(int userId, string baseUrl, int pageNumber, int pageSize = 20)
    {
        return FavoriteData.getUserFavoriteItems(userId, baseUrl, pageNumber, pageSize);
    }
     public static List<ItemDTO> SearchInUserFavoriteItems(int userId, string searchText, string baseUrl)
    {
        return FavoriteData.SearchInUserFavoriteItems(userId, searchText, baseUrl);
    }
}