using System.Data;
using Microsoft.Data.SqlClient;
using Shared.OrderDetailsDTOs;
using Shared.OrderDTOs;
using Shared.OrdersDTOs;

namespace EcommerceApiDataLayer;

public class OrderData
{
    public static int addNewOrder(NewOrderDTO newOrderDTO)
    {
        int newOrderId = -1;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_createNewOrder", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Order_userId", newOrderDTO.Order_userId);
            cmd.Parameters.AddWithValue("@Order_addressId", newOrderDTO.Order_addressId);
            cmd.Parameters.AddWithValue("@Order_type", newOrderDTO.Order_type);
            cmd.Parameters.AddWithValue("@Order_pricDelivery", newOrderDTO.Order_priceDelivery);
            cmd.Parameters.AddWithValue("@Order_price", newOrderDTO.Order_price);
            cmd.Parameters.AddWithValue("@Order_couponId", newOrderDTO.Order_couponId);
            cmd.Parameters.AddWithValue("@Order_paymentMethode", newOrderDTO.Order_paymentMethode);

            var output_newOrderId_param = new SqlParameter("@NewOrder_id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_newOrderId_param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

                if (output_newOrderId_param.Value != null && output_newOrderId_param.Value != DBNull.Value)
                    newOrderId = Convert.ToInt32(output_newOrderId_param.Value);

            }
            catch (Exception)
            {
                newOrderId = -1;
            }



        }
        return newOrderId;
    }

    public static List<OrdersDTO> getAllUserOrders(int userId)
    {
        List<OrdersDTO> OrdersList = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetAll_Orders_ForUser", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrdersList.Add(
                        new OrdersDTO(
                            reader.GetInt32(reader.GetOrdinal("order_id")),
                             reader.GetInt32(reader.GetOrdinal("order_addressId")),
                            Convert.ToInt32(reader.GetByte(reader.GetOrdinal("order_type"))),
                            reader.GetDecimal(reader.GetOrdinal("order_price")),
                             reader.GetDecimal(reader.GetOrdinal("order_priceDelivery")),
                             reader.GetInt32(reader.GetOrdinal("order_couponDiscount")),
                            Convert.ToInt32(reader.GetByte(reader.GetOrdinal("order_paymentMethode"))),
                             Convert.ToInt32(reader.GetByte(reader.GetOrdinal("order_status"))),
                             reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),


                            reader["address_name"] as string ?? "",
                            reader["address_city"] as string ?? "",
                            reader["address_street"] as string ?? "",

                            reader["address_lat"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["address_lat"]),
                            reader["address_long"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["address_long"]),

                             reader.GetDateTime(reader.GetOrdinal("order_datetime"))
                        )
                                );
                }
            }
        }
        return OrdersList;
    }

    public static List<OrdersDTO> getAllUserArchiveOrders(int userId)
    {
        List<OrdersDTO> OrdersList = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetAll_ArchiveOrders_ForUser", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrdersList.Add(
                        new OrdersDTO(
                            reader.GetInt32(reader.GetOrdinal("order_id")),
                             reader.GetInt32(reader.GetOrdinal("order_addressId")),
                            Convert.ToInt32(reader.GetByte(reader.GetOrdinal("order_type"))),
                            reader.GetDecimal(reader.GetOrdinal("order_price")),
                             reader.GetDecimal(reader.GetOrdinal("order_priceDelivery")),
                             reader.GetInt32(reader.GetOrdinal("order_couponDiscount")),
                            Convert.ToInt32(reader.GetByte(reader.GetOrdinal("order_paymentMethode"))),
                             Convert.ToInt32(reader.GetByte(reader.GetOrdinal("order_status"))),
                             reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),


                            reader["address_name"] as string ?? "",
                            reader["address_city"] as string ?? "",
                            reader["address_street"] as string ?? "",

                            reader["address_lat"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["address_lat"]),
                            reader["address_long"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["address_long"]),

                             reader.GetDateTime(reader.GetOrdinal("order_datetime")),
                             reader.GetByte(reader.GetOrdinal("order_rating")),
                             reader.GetString(reader.GetOrdinal("order_rating_note"))
                        )
                                );
                }
            }
        }
        return OrdersList;
    }

    public static List<OrdreDetailsDTO> getOrderDetails(int orderId)
    {
        List<OrdreDetailsDTO> OrdersDetailsList = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetOrderDetails", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderId", orderId);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrdersDetailsList.Add(
                        new OrdreDetailsDTO(
                            reader.GetInt32(reader.GetOrdinal("carts_itemId")),
                             reader.GetInt32(reader.GetOrdinal("carts_quantity")),
                            reader.GetDecimal(reader.GetOrdinal("carts_itemPrice")),
                            reader.GetString(reader.GetOrdinal("Items_name")),
                             reader.GetString(reader.GetOrdinal("Items_name_ar")),
                             reader.GetInt32(reader.GetOrdinal("items_category"))

                        )
                                );
                }
            }
        }
        return OrdersDetailsList;
    }

    public static bool deleteOrder(int orderId)
    {
        bool isDeleted = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_DeleteOrder", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            var output_isDelete_param = new SqlParameter("@IsDeleted", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_isDelete_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            if (output_isDelete_param.Value != null && output_isDelete_param.Value != DBNull.Value)
                isDeleted = Convert.ToBoolean(output_isDelete_param.Value);
        }
        return isDeleted;
    }

    public static bool updateOrderRating(int orderId, byte orderRating, string orderRatingNote)
    {
        bool isRating = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_ratingOrder", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@OrderRating", orderRating);
            cmd.Parameters.AddWithValue("@OrderRatingNote", orderRatingNote);

            var output_isRating_param = new SqlParameter("@IsRating", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_isRating_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            if (output_isRating_param.Value != null && output_isRating_param.Value != DBNull.Value)
                isRating = Convert.ToBoolean(output_isRating_param.Value);

        }
        return isRating;
    }

    public static bool findOrderByOrderId(ref NewOrderDTO orderDto, ref decimal totalPrice, ref byte orderStatus, ref DateTime created,
            ref int copounDiscount, ref byte rating, ref string rating_not, ref int deliveryId)
    {
        bool isFound = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_findOrderByOrderId", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderId", orderDto.Order_id);

            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        orderDto.Order_userId = reader.GetInt32(reader.GetOrdinal("Order_userId"));
                        orderDto.Order_addressId = reader.GetInt32(reader.GetOrdinal("order_addressId"));
                        orderDto.Order_type = reader.GetByte(reader.GetOrdinal("order_type"));
                        orderDto.Order_priceDelivery = reader.GetDecimal(reader.GetOrdinal("order_priceDelivery"));

                        orderDto.Order_price = reader.GetDecimal(reader.GetOrdinal("order_price"));
                        orderDto.Order_couponId = reader.GetInt32(reader.GetOrdinal("order_couponId"));
                        orderDto.Order_paymentMethode = reader.GetByte(reader.GetOrdinal("order_paymentMethode"));

                        totalPrice = reader.GetDecimal(reader.GetOrdinal("order_totalPrice"));
                        orderStatus = reader.GetByte(reader.GetOrdinal("order_status"));
                        created = reader.GetDateTime(reader.GetOrdinal("order_datetime"));
                        copounDiscount = reader.GetInt32(reader.GetOrdinal("order_couponDiscount"));
                        rating = reader.GetByte(reader.GetOrdinal("order_rating"));
                        rating_not = reader.GetString(reader.GetOrdinal("order_rating_note"));
                        deliveryId = reader.GetInt32(reader.GetOrdinal("order_deliveryId"));

                    }
                    else
                    {
                        isFound = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                isFound = false;
            }
        }
        return isFound;
    }
    public static bool updateOrderByOrderId(NewOrderDTO orderDto, decimal totalPrice, byte orderStatus, int couponDiscount,
                    int deliveryId)
    {
        bool isUpdate = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_updateOrderByOrderId", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderId", orderDto.Order_id);

            cmd.Parameters.AddWithValue("@orderUserId", orderDto.Order_userId);
            cmd.Parameters.AddWithValue("@orderAddressId", orderDto.Order_addressId);
            cmd.Parameters.AddWithValue("@orderType", orderDto.Order_type);
            cmd.Parameters.AddWithValue("@orderPriceDelivery", orderDto.Order_priceDelivery);
            cmd.Parameters.AddWithValue("@orderPrice", orderDto.Order_price);
            cmd.Parameters.AddWithValue("@orderCouponId", orderDto.Order_couponId);
            cmd.Parameters.AddWithValue("@orderPaymentMethode", orderDto.Order_paymentMethode);
            cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
            cmd.Parameters.AddWithValue("@orderStatus", orderStatus);
            cmd.Parameters.AddWithValue("@couponDiscount", couponDiscount);
            cmd.Parameters.AddWithValue("@deliveryId", deliveryId);

            SqlParameter isUpdate_param = new SqlParameter("@isUpdate", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(isUpdate_param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

                if (isUpdate_param.Value != null && isUpdate_param.Value != DBNull.Value)
                    isUpdate = Convert.ToBoolean(isUpdate_param.Value);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                isUpdate = false;
            }
        }
        return isUpdate;
    }



    //_________________________________________________ admin ______________________

    public static bool updateStatusOrder(int orderId, byte orderStatus, int userId, string title, string body)
    {
        bool isSuccess = false;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_updateStatusOrder", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Body", body);

            var output_isSuccess_param = new SqlParameter("@IsSuccess", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_isSuccess_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            if (output_isSuccess_param.Value != null || output_isSuccess_param.Value != DBNull.Value)
                isSuccess = Convert.ToBoolean(output_isSuccess_param.Value);

        }
        return isSuccess;
    }
    public static List<OrderMainIfoDTO> getAllNewOrdersForAdmin()
    {
        List<OrderMainIfoDTO> orders = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_GetAllNewOrders", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderMainIfoDTO(
                             reader.GetInt32(reader.GetOrdinal("order_id")),
                             reader.GetInt32(reader.GetOrdinal("order_userId")),
                             reader.GetInt32(reader.GetOrdinal("order_deliveryId")),
                             reader.GetByte(reader.GetOrdinal("order_type")),
                             reader.GetByte(reader.GetOrdinal("order_paymentMethode")),
                             reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),
                             reader.IsDBNull(reader.GetOrdinal("address_city")) ? "" : reader.GetString(reader.GetOrdinal("address_city")),
                             reader.IsDBNull(reader.GetOrdinal("address_street")) ? "" : reader.GetString(reader.GetOrdinal("address_street")),
                             reader.GetDateTime(reader.GetOrdinal("order_datetime")),
                             reader.GetByte(reader.GetOrdinal("order_status"))
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return orders;
        }
    }
    public static List<OrderMainIfoDTO> getAllOrdersInPrepareForAdmin()
    {
        List<OrderMainIfoDTO> orders = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_GetAllOrdersInPrepare", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderMainIfoDTO(
                             reader.GetInt32(reader.GetOrdinal("order_id")),
                             reader.GetInt32(reader.GetOrdinal("order_userId")),
                             reader.GetInt32(reader.GetOrdinal("order_deliveryId")),
                             reader.GetByte(reader.GetOrdinal("order_type")),
                             reader.GetByte(reader.GetOrdinal("order_paymentMethode")),
                             reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),
                             reader.IsDBNull(reader.GetOrdinal("address_city")) ? "" : reader.GetString(reader.GetOrdinal("address_city")),
                             reader.IsDBNull(reader.GetOrdinal("address_street")) ? "" : reader.GetString(reader.GetOrdinal("address_street")),
                             reader.GetDateTime(reader.GetOrdinal("order_datetime")),
                             reader.GetByte(reader.GetOrdinal("order_status"))
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return orders;
        }
    }
    public static List<OrderMainIfoDTO> getAllOrdersInProcessForAdmin()
    {
        List<OrderMainIfoDTO> orders = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_GetAllOrdersInProcess", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderMainIfoDTO(
                             reader.GetInt32(reader.GetOrdinal("order_id")),
                             reader.GetInt32(reader.GetOrdinal("order_userId")),
                             reader.GetInt32(reader.GetOrdinal("order_deliveryId")),
                             reader.GetByte(reader.GetOrdinal("order_type")),
                             reader.GetByte(reader.GetOrdinal("order_paymentMethode")),
                             reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),
                             reader.IsDBNull(reader.GetOrdinal("address_city")) ? "" : reader.GetString(reader.GetOrdinal("address_city")),
                             reader.IsDBNull(reader.GetOrdinal("address_street")) ? "" : reader.GetString(reader.GetOrdinal("address_street")),
                             reader.GetDateTime(reader.GetOrdinal("order_datetime")),
                             reader.GetByte(reader.GetOrdinal("order_status"))
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return orders;
        }
    }
    public static List<OrderMainIfoDTO> getAllOrdersArchiveForAdmin()
    {
        List<OrderMainIfoDTO> orders = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_GetAllOrdersArchive", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderMainIfoDTO(
                             reader.GetInt32(reader.GetOrdinal("order_id")),
                             reader.GetInt32(reader.GetOrdinal("order_userId")),
                             reader.GetInt32(reader.GetOrdinal("order_deliveryId")),
                             reader.GetByte(reader.GetOrdinal("order_type")),
                             reader.GetByte(reader.GetOrdinal("order_paymentMethode")),
                             reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),
                             reader.IsDBNull(reader.GetOrdinal("address_city")) ? "" : reader.GetString(reader.GetOrdinal("address_city")),
                             reader.IsDBNull(reader.GetOrdinal("address_street")) ? "" : reader.GetString(reader.GetOrdinal("address_street")),
                             reader.GetDateTime(reader.GetOrdinal("order_datetime")),
                             reader.GetByte(reader.GetOrdinal("order_status"))
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return orders;
        }
    }


    // ------------- delivery ----

    public static OrderInfoDTO? getOrderInfoByOrderId(int orderId)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_delivery_orderInfo", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@orderId", orderId);
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderInfoDTO(
                            reader.GetInt32(reader.GetOrdinal("order_couponDiscount")),
                            reader.GetDecimal(reader.GetOrdinal("order_priceDelivery")),
                            reader.GetDecimal(reader.GetOrdinal("order_totalPrice")),
                            reader.GetString(reader.GetOrdinal("address_city")),
                            reader.GetString(reader.GetOrdinal("address_street")),
                            reader.GetDecimal(reader.GetOrdinal("address_lat")),
                            reader.GetDecimal(reader.GetOrdinal("address_long")),
                            reader.GetString(reader.GetOrdinal("users_firsName")),
                            reader.GetString(reader.GetOrdinal("users_lastName")),
                            reader.GetString(reader.GetOrdinal("users_phone")),
                            reader.GetString(reader.GetOrdinal("users_email"))
                        );
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex) { Console.WriteLine($"error: {ex.Message}"); }
        }
        return null;
    }


}
