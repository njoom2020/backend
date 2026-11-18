namespace Shared.OrdersAvailableDTOs;
public class OrdersAvailableDTO
{
    public OrdersAvailableDTO(int orderId, string city, string street, byte paymentMethode, decimal priceDelivery, byte orderStatus)
    {
        this.Order_id = orderId;
        this.City = city;
        this.Street = street;
        this.PaymentMethode = paymentMethode;
        this.PriceDelivery = priceDelivery;
        this.OrderStatus = orderStatus;
    }
    public int Order_id {get; set;}
    public string City {get; set;} = "";
    public string Street {get; set;} = "";
    public byte PaymentMethode {get; set;}
    public decimal PriceDelivery {get;set;}
    public byte OrderStatus {get;set;}

}
