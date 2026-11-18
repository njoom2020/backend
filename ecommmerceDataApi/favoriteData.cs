using System.Data;
using Microsoft.Data.SqlClient;
using Shared.ItemDTOs;
using Shared.MyFavoriteDTO;

namespace EcommerceApiDataLayer;

public class FavoriteData
{
    public static int addNewFavorite(int userId, int itemId)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_AddNewFavorite", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@ItemId", itemId);

            var outputFavoriteIdParam = new SqlParameter("@NewFavoriteId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputFavoriteIdParam);

            connection.Open();
            cmd.ExecuteNonQuery();

            if (outputFavoriteIdParam.Value != null && outputFavoriteIdParam.Value != DBNull.Value)
                return Convert.ToInt32(outputFavoriteIdParam.Value);

            return -1;
        }
    }
    public static bool deleteFavorite(int userId, int itemId)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_DeleteFavorite", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@ItemId", itemId);

            connection.Open();
            cmd.ExecuteNonQuery();

            return true;
        }
    }

    public static List<MyFavoriteDTO> getUserFavoriteItems(int userId, string baseUrl, int pageNumber, int pageSize = 20)
    {
        List<MyFavoriteDTO> listMyFavorites = new List<MyFavoriteDTO>();

        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_GetUserFavoriteItems", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

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

                    listMyFavorites.Add(new MyFavoriteDTO(
                       favoriteId: reader.GetInt32(reader.GetOrdinal("favorite_id")),
                       userId: reader.GetInt32(reader.GetOrdinal("favorite_userId")),
                       itemId: reader.GetInt32(reader.GetOrdinal("items_id")),
                       name: reader.GetString(reader.GetOrdinal("items_name")),
                       nameAr: reader.GetString(reader.GetOrdinal("items_name_ar")),
                       desc: reader.GetString(reader.GetOrdinal("items_desc")),
                       descAr: reader.GetString(reader.GetOrdinal("items_desc_ar")),
                       image: fullImageUrl,
                       count: reader.GetInt32(reader.GetOrdinal("items_count")),
                       active: reader.GetBoolean(reader.GetOrdinal("items_active")),
                       price: reader.GetDecimal(reader.GetOrdinal("items_price")),
                       discount: reader.GetInt32(reader.GetOrdinal("items_discount")),
                       created: reader.GetDateTime(reader.GetOrdinal("items_datetime")),
                       categoryId: reader.GetInt32(reader.GetOrdinal("items_category")),
                       isFavorite: reader.GetBoolean(reader.GetOrdinal("isFavorite"))
                 ));
                }
            }
        }
        return listMyFavorites;
    }


     public static List<ItemDTO> SearchInUserFavoriteItems(int userId, string searchText, string baseUrl)
    {
        List<ItemDTO> listItems = new List<ItemDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_searchInUserFavoriteItems", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SearchText", searchText);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string imageName = reader.IsDBNull(reader.GetOrdinal("items_image"))
                    ? ""
                    : reader.GetString(reader.GetOrdinal("items_image"));
                    string fullImageUrl = string.IsNullOrWhiteSpace(imageName) ? "" : $"{baseUrl}/api/ItemApi/image/{imageName}";

                    listItems.Add(new ItemDTO(
                        reader.GetInt32(reader.GetOrdinal("items_id")),
                                reader.GetString(reader.GetOrdinal("items_name")),
                                reader.GetString(reader.GetOrdinal("items_name_ar")),
                                reader.GetString(reader.GetOrdinal("items_desc")),
                                reader.GetString(reader.GetOrdinal("items_desc_ar")),
                                fullImageUrl,
                                reader.GetInt32(reader.GetOrdinal("items_count")),
                                reader.GetDecimal(reader.GetOrdinal("items_price")),
                                reader.GetInt32(reader.GetOrdinal("items_discount")),
                                reader.GetInt32(reader.GetOrdinal("items_category")),
                                true
                    ));
                }
            }
        }
        return listItems;
    }

}