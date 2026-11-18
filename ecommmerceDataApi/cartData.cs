using System.Data;
using Microsoft.Data.SqlClient;
using Shared.UserCartDTO;

namespace EcommerceApiDataLayer;

public class CartData
{
    public static List<UserCartDTO> getUserCarts(int userId, string baseUrl)
    {
        List<UserCartDTO> UserCartsList = new List<UserCartDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetUserCart", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    string imageName = reader.IsDBNull(reader.GetOrdinal("items_image"))
                    ? ""
                    : reader.GetString(reader.GetOrdinal("items_image"));

                    string fullImageUrl = string.IsNullOrWhiteSpace(imageName)
                     ? ""
                     : $"{baseUrl}/api/itemApi/image/{imageName}";


                    UserCartsList.Add(new UserCartDTO(
                      cartId: reader.GetInt32(reader.GetOrdinal("carts_id")),
                      userId: reader.GetInt32(reader.GetOrdinal("carts_userId")),
                      itemId: reader.GetInt32(reader.GetOrdinal("carts_itemId")),
                      itemName: reader.GetString(reader.GetOrdinal("items_name")),
                      itemNameAr: reader.GetString(reader.GetOrdinal("items_name_ar")),
                      itemImage: fullImageUrl,
                      itemPrice: reader.GetDecimal(reader.GetOrdinal("items_price")),
                      itemDiscount: reader.GetInt32(reader.GetOrdinal("items_discount")),
                      itemCount: reader.GetInt32(reader.GetOrdinal("items_count")),
                      cartQuantity: reader.GetInt32(reader.GetOrdinal("carts_quantity")),
                      isFavorite: reader.GetBoolean(reader.GetOrdinal("is_favorite")),
                      isActive: reader.GetBoolean(reader.GetOrdinal("items_active")),
                      carts_created: reader.GetDateTime(reader.GetOrdinal("carts_created"))
                    ));
                }
            }
        }
        return UserCartsList;
    }

    public static ManageCartResultDto addSubtractQuantityAndAddNewOrUpdateOrDelete(CartDataRequestDto requestData)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_ManageCartItem", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", requestData.UserId);
            cmd.Parameters.AddWithValue("@ItemId", requestData.ItemId);
            cmd.Parameters.AddWithValue("@QuantityChange", requestData.QuantityChange);

            var output_isComplete_param = new SqlParameter("@IsComplete", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_isComplete_param);

            var output_finalQuantity_param = new SqlParameter("@FinalQuantityResult", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_finalQuantity_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            bool isSuccess = output_isComplete_param.Value != DBNull.Value
                && Convert.ToBoolean(output_isComplete_param.Value);

            int finalQty = output_finalQuantity_param.Value != DBNull.Value
                ? Convert.ToInt32(output_finalQuantity_param.Value)
                :-1;
            return new ManageCartResultDto
            {
                IsComplete = isSuccess,
                FinalQuantity = finalQty
            };

        }
    }

    public static bool addUpdateCarts(int userId, int ItemId, int quantity)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_AddUpdateCart", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@ItemId", ItemId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            var output_isComplete_param = new SqlParameter("@IsComplete", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_isComplete_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            bool isSuccess = output_isComplete_param.Value != DBNull.Value
                && Convert.ToBoolean(output_isComplete_param.Value);

            return isSuccess;
        }
    }
}

public class ManageCartResultDto
{
    public bool IsComplete {get; set;}
    public int FinalQuantity {get; set;}
}