using System.Data;
using System.Dynamic;
using Microsoft.Data.SqlClient;
using Shared.OrdersAvailableDTOs;

namespace EcommerceApiDataLayer;
public class DeliveryData
{
    public static List<OrdersAvailableDTO> getAllAvailableOrders()
    {
        List<OrdersAvailableDTO> OrdersList = [];
        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_delivery_GetAllOrdersAvailable", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrdersList.Add( new OrdersAvailableDTO(
                            reader.GetInt32(reader.GetOrdinal("order_id")),

                            reader.IsDBNull(reader.GetOrdinal("address_city"))? string.Empty:reader.GetString(reader.GetOrdinal("address_city")),
                            reader.IsDBNull(reader.GetOrdinal("address_street"))?string.Empty:reader.GetString(reader.GetOrdinal("address_street")),
                            reader.GetByte(reader.GetOrdinal("order_paymentMethode")),
                            reader.GetDecimal(reader.GetOrdinal("order_priceDelivery")),
                            reader.GetByte(reader.GetOrdinal("order_status"))
                        )
                    );
                }
            }
        }
        return OrdersList;
    }

    public static List<OrdersAvailableDTO> getAllAcceptedOrders(int deliveryId)
    {
        List<OrdersAvailableDTO> OrdersList = [];
        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_delivery_getAllOrderAccepted", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DeliveryId", deliveryId);

            connection.Open();

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrdersList.Add( new OrdersAvailableDTO(
                            reader.GetInt32(reader.GetOrdinal("order_id")),

                            reader.IsDBNull(reader.GetOrdinal("address_city"))? string.Empty:reader.GetString(reader.GetOrdinal("address_city")),
                            reader.IsDBNull(reader.GetOrdinal("address_street"))?string.Empty:reader.GetString(reader.GetOrdinal("address_street")),
                            reader.GetByte(reader.GetOrdinal("order_paymentMethode")),
                            reader.GetDecimal(reader.GetOrdinal("order_priceDelivery")),
                            reader.GetByte(reader.GetOrdinal("order_status"))
                        )
                    );
                }
            }
        }
        return OrdersList;
    }

    public static bool AcceptedOrder(int deliveryId, int orderId)
    {
        bool isSuccess = false;

        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_delivery_acceptedOrder", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DeliveryId", deliveryId);
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            SqlParameter isSuccess_Param = new SqlParameter("@IsSuccess", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(isSuccess_Param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

                if(isSuccess_Param.Value != DBNull.Value)
                    isSuccess = Convert.ToBoolean(isSuccess_Param.Value);
            }
            catch (Exception)
            {
                isSuccess = false;
            }
        }
        return isSuccess;
    }

    public static bool delete_UpdateAcceptedOrder(int deliveryId, int orderId)
    {
        bool isSuccess = false;

        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_delivery_DesAcceptedOrder", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DeliveryId", deliveryId);
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            SqlParameter isSuccess_Param = new SqlParameter("@isUpdate", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(isSuccess_Param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

                if(isSuccess_Param.Value != DBNull.Value)
                    isSuccess = Convert.ToBoolean(isSuccess_Param.Value);
            }
            catch (Exception)
            {
                isSuccess = false;
            }
        }
        return isSuccess;
    }
}