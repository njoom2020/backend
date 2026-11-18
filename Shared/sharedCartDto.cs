

namespace Shared.UserCartDTO
{
    public class UserCartDTO
    {
        public UserCartDTO() { }
        public UserCartDTO(int cartId, int userId, int itemId, string itemName, string itemNameAr, string itemImage, decimal itemPrice,
        int itemDiscount, int itemCount, int cartQuantity, bool isFavorite, bool isActive, DateTime carts_created)
        {
            CartId = cartId;
            UserId = userId;
            ItemId = itemId;
            ItemName = itemName;
            ItemNameAr = itemNameAr;
            ItemImage = itemImage;
            ItemPrice = itemPrice;
            ItemDiscount = itemDiscount;
            ItemCount = itemCount;
            CartQuantity = cartQuantity;
            IsFavorite = isFavorite;
            IsActive = isActive;
            Cart_created = carts_created;

        }
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemNameAr { get; set; } = string.Empty;
        public string ItemImage { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
        public int ItemDiscount { get; set; }
        public int ItemCount { get; set; }
        public int CartQuantity { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsActive { get; set; }
        public DateTime Cart_created { get; set; }

    }

    public class CartDataRequestDto
    {
        public int UserId {get; set;}
        public int ItemId {get; set;}
        public int QuantityChange {get; set;}
    }





}

