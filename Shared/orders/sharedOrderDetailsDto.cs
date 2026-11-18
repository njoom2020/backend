namespace Shared.OrderDetailsDTOs;

public class OrdreDetailsDTO
{



    public OrdreDetailsDTO(
    int carts_itemId,
    int carts_quantity,
    decimal carts_itemPrice,
    string items_name,
    string items_name_ar,
    int items_category

    )
    {
        Carts_itemId = carts_itemId;
        Carts_quantity = carts_quantity;
        Carts_itemPrice = carts_itemPrice;
        Items_name = items_name;
        Items_name_ar = items_name_ar;
        Items_category = items_category;
    }
    public int Carts_itemId { get; set; }
    public int Carts_quantity { get; set; }
    public decimal Carts_itemPrice { get; set; }
    public string Items_name { get; set; } = "";
    public string Items_name_ar { get; set; } = "";
    public int Items_category { get; set; }

}

public class OrderInfoDTO
{
    public OrderInfoDTO(
int      couponDiscount,
decimal  priceDelivery ,
decimal  totalPrice    ,
string   city          ,
string   street        ,
decimal   address_lat   ,
decimal   address_long  ,
string   firstName     ,
string   lastName      ,
string   phone         ,
string   email


    )
   {
        
this.CouponDiscount =  couponDiscount;
this.PriceDelivery  =  priceDelivery ;
this.TotalPrice     =  totalPrice    ;
this.City           =  city          ;
this.Street         =  street        ;
this.Address_lat    =  address_lat   ;
this.Address_long   =  address_long  ;
this.FirstName      =  firstName     ;
this.LastName       =  lastName      ;
this.Phone          =  phone         ;
this.Email          =  email         ;






        
    }
    public int      CouponDiscount      {get; set;}
    public decimal  PriceDelivery       {get ; set;}
    public decimal  TotalPrice          {get; set;}
    public string   City                {get;set;} ="";
    public string   Street                          {get; set;} ="";
    public decimal   Address_lat                 {get; set;}
    public decimal   Address_long                    {get;set;}
    public string   FirstName                   {get;set;} = "";
    public string   LastName                    {get;set;} = "";
    public string   Phone                   { get;set;} = "";
    public string   Email                           {get;set;} = "";
}   