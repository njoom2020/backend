namespace Shared.OrderDTOs;

public class NewOrderDTO
{
    public NewOrderDTO()
    {
        Order_id = -1;
        Order_userId = -1;
        Order_addressId = -1;
        Order_type = 0;
        Order_priceDelivery = 0;
        Order_price = 0;
        Order_couponId = -1;
        Order_paymentMethode = 0;
    }
    public NewOrderDTO(int? orderId, int userId, int? addressId, int orderType, decimal priceDelivery,
         decimal price, int couponId, int paymentMethod)
    {
        Order_id = orderId;
        Order_userId = userId;
        Order_addressId = addressId;
        Order_type = orderType;
        Order_priceDelivery = priceDelivery;
        Order_price = price;
        Order_couponId = couponId;
        Order_paymentMethode = paymentMethod;
    }
    public int? Order_id {get;set;} = -1;
    public int Order_userId { get; set; }
    public int? Order_addressId { get; set; }
    public int Order_type { get; set; }
    public decimal Order_priceDelivery { get; set; }
    public decimal Order_price { get; set; }
    public int Order_couponId { get; set; }
    public int Order_paymentMethode { get; set; }
}
