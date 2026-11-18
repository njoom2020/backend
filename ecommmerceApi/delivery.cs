using System.Dynamic;
using System.Reflection.Metadata;
using EcommerceApiDataLayer;
using Shared.OrdersAvailableDTOs;

namespace EcommerceApiBusinessLayer;

public class Delivery
{
    public static List<OrdersAvailableDTO> getAllOrdersAvailable()
    {
        return DeliveryData.getAllAvailableOrders();
    }

    public static List<OrdersAvailableDTO> getAllOrdersAccepted(int deliveryId)
    {
        return DeliveryData.getAllAcceptedOrders(deliveryId);
    }

    public static bool AcceptedOrder(int deliveryId, int orderId)
    {
        return DeliveryData.AcceptedOrder(deliveryId, orderId);
    }

    public static bool deleteAcceptedOrder(int deliveryId, int orderId)
    {
        return DeliveryData.delete_UpdateAcceptedOrder(deliveryId, orderId);
    }

    
}