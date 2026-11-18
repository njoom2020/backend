using EcommerceApiDataLayer;
using Microsoft.Data.SqlClient;
using Shared.OrderDetailsDTOs;
using Shared.OrderDTOs;
using Shared.OrdersDTOs;

namespace EcommerceApiBusinessLayer;

public class Order
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;
    public NewOrderDTO newOrderDto
    {
        get
        {
            return (new NewOrderDTO(
                                    this.Order_id ?? -1,
                                    this.Order_userId,
                                    this.Order_addressId,
                                    this.Order_type,
                                    this.Order_priceDelivery,
                                    this.Order_price,
                                    this.Order_couponId,
                                    this.Order_paymentMethode
                                    ));
        }
    }
    public int? Order_id { get; set; } = -1;
    public int Order_userId { get; set; }
    public int? Order_addressId { get; set; }
    public int Order_type { get; set; }
    public decimal Order_price { get; set; }
    public decimal Order_priceDelivery { get; set; }
    public int Order_couponDiscount { get; set; }
    public int Order_couponId { get; set; }
    public int Order_paymentMethode { get; set; }
    public decimal? Order_totalPrice { get; set; } = -1;
    public byte Order_status { get; set; } = 0;
    public DateTime? Order_Created { get; set; } = DateTime.UtcNow;
    public byte? Order_rating { get; set; } = 0;
    public string? Order_rating_note { get; set; } = "";
    public int deliveryId { get; set; } = -1;


    public Order(NewOrderDTO newOrderDTO, decimal? totalPrice = -1, byte status = 0, int order_couponDiscount = 0,
    DateTime? created = null,
    byte order_rating = 0, string order_rating_note = "", int deliveryId = -1,
    enMode cMdoe = enMode.AddNew)
    {
        this.Order_id = newOrderDTO.Order_id;
        this.Order_userId = newOrderDTO.Order_userId;
        this.Order_addressId = newOrderDTO.Order_addressId;
        this.Order_type = newOrderDTO.Order_type;
        this.Order_priceDelivery = newOrderDTO.Order_priceDelivery;
        this.Order_price = newOrderDTO.Order_price;
        this.Order_couponId = newOrderDTO.Order_couponId;
        this.Order_paymentMethode = newOrderDTO.Order_paymentMethode;
        this.Order_totalPrice = totalPrice; //?? newOrderDTO.Order_price + newOrderDTO.Order_priceDelivery;
        this.Order_status = status;
        this.Order_couponDiscount = order_couponDiscount;

        if (created.HasValue)
        {
            this.Order_Created = created;
        }

        this.Order_rating = order_rating;
        this.Order_rating_note = order_rating_note;
        this.deliveryId = deliveryId;


        Mode = cMdoe;
    }

    private bool _addNew()
    {
        this.Order_id = OrderData.addNewOrder(newOrderDto);
        return this.Order_id > 0;
    }

    public static Order? findOrderById(int orderId)
    {
        NewOrderDTO orderDto = new NewOrderDTO();
        orderDto.Order_id = orderId;

        decimal totalPrice = 0; byte orderStatus = 0; DateTime created = DateTime.UtcNow;
        byte rating = 0; string rating_not = ""; int deliveryId = -1;
        int order_couponDiscount = 0;

        bool isFound = OrderData.findOrderByOrderId(ref orderDto, ref totalPrice, ref orderStatus, ref created,
                ref order_couponDiscount,
                ref rating, ref rating_not, ref deliveryId);
        if (isFound)
            return new Order(orderDto, totalPrice, orderStatus, order_couponDiscount, created, rating, rating_not, deliveryId, enMode.Update);
        else return null;

    }

    private bool _update()
    {
        return OrderData.updateOrderByOrderId(this.newOrderDto, this.Order_totalPrice ?? 0, this.Order_status, this.Order_couponDiscount,
                            this.deliveryId);
    }

    public bool save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                Mode = enMode.Update;
                return _addNew();
            case enMode.Update:
                return _update();

        }
        return false;
    }

    public static List<OrdersDTO> getAllUserOrders(int userId)
    {
        if (userId <= 0)
            return new List<OrdersDTO>();

        var orders = OrderData.getAllUserOrders(userId);
        return orders ?? new List<OrdersDTO>();
    }

    public static List<OrdersDTO> getAllUserArchiveOrders(int userId)
    {
        if (userId <= 0)
            return new List<OrdersDTO>();


        var orders = OrderData.getAllUserArchiveOrders(userId);
        return orders ?? new List<OrdersDTO>();
    }

    public static List<OrdreDetailsDTO> GetOrderDetails(int orderId)
    {
        return OrderData.getOrderDetails(orderId);
    }

    public static bool DeleteOrder(int orderId)
    {
        if (orderId > 0)
            return OrderData.deleteOrder(orderId);

        return false;
    }

    public static bool updateOrderRatingsSetSumbit(int orderId, byte orderRating, string orderRatingNote)
    {
        return OrderData.updateOrderRating(orderId, orderRating, orderRatingNote);
    }
    //__________________________________ admin __________________________

    public static bool updateStatusOrder(int orderId, byte orderStatus, int userId, string title, string body)
    {
        return OrderData.updateStatusOrder(orderId, orderStatus, userId, title, body);
    }

    public static List<OrderMainIfoDTO> getAllNewOrders()
    {
        return OrderData.getAllNewOrdersForAdmin();
    }

    public static List<OrderMainIfoDTO> getAllOrdersInPrepare()
    {
        return OrderData.getAllOrdersInPrepareForAdmin();
    }

    public static List<OrderMainIfoDTO> getAllOrdersInProcess()
    {
        return OrderData.getAllOrdersInProcessForAdmin();
    }

    public static List<OrderMainIfoDTO> getAllOrdersArchive()
    {
        return OrderData.getAllOrdersArchiveForAdmin();
    }

    //__________________________________ delivery : __________________________
    public static OrderInfoDTO? getOrderInfoById(int orderId)
    {
        return OrderData.getOrderInfoByOrderId(orderId);
    }

}
