namespace Shared.OrdersDTOs;

public class OrdersDTO
{
    public OrdersDTO() { }
    public OrdersDTO(

int order_id,
int order_addressId,
int order_type,
decimal order_price,
decimal order_priceDelivery,
int order_couponDiscount,
int order_paymentMethode,
int order_status,
decimal order_totalPrice,
string? address_name,
string? address_city,
string? address_street,
decimal? address_lat,
decimal? address_long,
DateTime order_created,
 byte? order_rating = 0,
 string? order_rating_note = ""
 

    )
    {

        Order_id = order_id;
        Order_addressId = order_addressId;
        Order_type = order_type;
        Order_price = order_price;
        Order_priceDelivery = order_priceDelivery;
        Order_couponDiscount = order_couponDiscount;
        Order_paymentMethode = order_paymentMethode;
        Order_status = order_status;
        Order_totalPrice = order_totalPrice;


        Address_name = address_name;
        Address_city = address_city;
        Address_street = address_street;
        Address_lat = address_lat;
        Address_long = address_long;
        Order_created = order_created;
        Order_rating = order_rating;
        Order_rating_note = order_rating_note;
    }
    public int? Order_id { get; set; } = -1;
    //public int Order_userId {get; set;} = -1;
    public int? Order_addressId { get; set; } = -1;
    public int Order_type { get; set; }
    public decimal Order_price { get; set; }
    public decimal Order_priceDelivery { get; set; }
    public int Order_couponDiscount { get; set; }
    public int Order_paymentMethode { get; set; }
    public int Order_status { get; set; }
   // public int Order_couponId {get;set;}
    public decimal Order_totalPrice { get; set; }

    public string? Address_name { get; set; } = "";
    public string? Address_city { get; set; } = "";
    public string? Address_street { get; set; } = "";
    public decimal? Address_lat { get; set; } = -1;
    public decimal? Address_long { get; set; } = -1;

    public DateTime Order_created { get; set; }


    public byte? Order_rating { get; set; } = 0;
    public string? Order_rating_note { get; set; } = "";

}



