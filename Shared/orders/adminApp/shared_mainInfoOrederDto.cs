namespace Shared.OrdersDTOs;

public class OrderMainIfoDTO
{
    public OrderMainIfoDTO() { }
    public OrderMainIfoDTO(

                            int order_id,
                            int user_id,
                            int delivery_id,
                            byte order_type,
                            byte order_paymentMethode,
                            decimal order_totalPrice,
                            string address_city,
                            string address_street,
                            DateTime order_created,
                            byte order_status

    )
    {
        Order_id = order_id;
        User_id = user_id;
        Delivery_id = delivery_id;
        Order_type = order_type;
        Order_paymentMethode = order_paymentMethode;
        Order_totalPrice = order_totalPrice;
        Address_city = address_city;
        Address_street = address_street;
        Order_created = order_created;
        Order_status = order_status;
    }
    public int Order_id { get; set; } = -1;
    public int User_id { get; set; } = -1;
    public int Delivery_id { get; set; }
    public byte Order_type { get; set; }
    public byte Order_paymentMethode { get; set;}
    public decimal Order_totalPrice { get; set; }
    public string Address_city { get; set; } = "";
    public string Address_street { get; set; } = "";
    public DateTime Order_created { get; set; }
    public byte Order_status { get; set; }
}

