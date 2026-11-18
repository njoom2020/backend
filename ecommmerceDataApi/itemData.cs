using System.Data;
using Microsoft.Data.SqlClient;
using Shared.ItemAdminDTOs;
using Shared.ItemDTOs;

namespace EcommerceApiDataLayer;

public class ItemData
{
    public static List<ItemDTO> GetAllItems(bool isOffers = false)
    {
        List<ItemDTO> ItemsList = new List<ItemDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetAllItems", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IsOffers", isOffers);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemsList.Add(
                        new ItemDTO(
                            reader.GetInt32(reader.GetOrdinal("items_id")),
                            reader.GetString(reader.GetOrdinal("items_name")),
                            reader.GetString(reader.GetOrdinal("items_name_ar")),
                            reader.GetString(reader.GetOrdinal("items_desc")),
                            reader.GetString(reader.GetOrdinal("items_desc_ar")),
                            reader.GetString(reader.GetOrdinal("items_image")),
                            reader.GetInt32(reader.GetOrdinal("items_count")),
                            reader.GetDecimal(reader.GetOrdinal("items_price")),
                            reader.GetInt32(reader.GetOrdinal("items_discount")),
                            0,
                            false

                        )
                    );
                }
            }
            return ItemsList;
        }
    }

    public static List<ItemDTO> GetTopSellingItems()
    {
        List<ItemDTO> ItemsList = new List<ItemDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetTopSellingItems", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;


            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemsList.Add(
                        new ItemDTO(
                            reader.GetInt32(reader.GetOrdinal("items_id")),
                            reader.GetString(reader.GetOrdinal("items_name")),
                            reader.GetString(reader.GetOrdinal("items_name_ar")),
                            reader.GetString(reader.GetOrdinal("items_desc")),
                            reader.GetString(reader.GetOrdinal("items_desc_ar")),
                            reader.GetString(reader.GetOrdinal("items_image")),
                            reader.GetInt32(reader.GetOrdinal("items_count")),
                            reader.GetDecimal(reader.GetOrdinal("items_price")),
                            reader.GetInt32(reader.GetOrdinal("items_discount")),
                            reader.GetInt32(reader.GetOrdinal("items_category")),
                            false
                        )
                    );
                }
            }
            return ItemsList;
        }
    }
    public static List<ItemDTO> getAllItemsByCategoryId(int CategoryId)
    {
        List<ItemDTO> listItems = new List<ItemDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetAllItemsByCategoryId", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {


                    listItems.Add(new ItemDTO(
                        reader.GetInt32(reader.GetOrdinal("items_id")),
                                reader.GetString(reader.GetOrdinal("items_name")),
                                reader.GetString(reader.GetOrdinal("items_name_ar")),
                                reader.GetString(reader.GetOrdinal("items_desc")),
                                reader.GetString(reader.GetOrdinal("items_desc_ar")),
                                reader.GetString(reader.GetOrdinal("items_image")),
                                reader.GetInt32(reader.GetOrdinal("items_count")),
                                reader.GetDecimal(reader.GetOrdinal("items_price")),
                                reader.GetInt32(reader.GetOrdinal("items_discount"))
                    ));
                }
            }
        }
        return listItems;
    }


    public static List<ItemDTO> getItemsByCategoryIdWithPagination(int categoryId, int userId, int pageNumber,
                int pageSize, string baseUrl)
    {
        List<ItemDTO> listItems = new List<ItemDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetItemsByCategoryId", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@categoryId", categoryId);
            cmd.Parameters.AddWithValue("@pageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                                reader.GetInt32(reader.GetOrdinal("categoryId")),
                                reader.GetBoolean(reader.GetOrdinal("is_favorite"))
                    ));
                }
            }
        }
        return listItems;
    }

    public static List<ItemDTO> searchItems(int userId, string searchText, string baseUrl, int categoryId = 0)
    {
        List<ItemDTO> listItems = new List<ItemDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_searchItems", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@SearchText", searchText);
            cmd.Parameters.AddWithValue("@categoryId", categoryId);

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
                                reader.GetInt32(reader.GetOrdinal("categoryId")),
                                reader.GetBoolean(reader.GetOrdinal("is_favorite"))
                    ));
                }
            }
        }
        return listItems;
    }

    public static bool findItemById(ref ItemDTO itemDto, ref DateTime created)
    {
        bool isFind = false;

        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_GetItemById", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@itemId", itemDto.Id);

            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFind = true;

                        itemDto.Name = reader.GetString(reader.GetOrdinal("items_name"));
                        itemDto.NameAr = reader.GetString(reader.GetOrdinal("items_name_ar"));
                        itemDto.Desc = reader.GetString(reader.GetOrdinal("items_desc"));
                        itemDto.DescAr = reader.GetString(reader.GetOrdinal("items_desc_ar"));
                        itemDto.Image = reader.GetString(reader.GetOrdinal("items_image"));
                        itemDto.Count = reader.GetInt32(reader.GetOrdinal("items_count"));
                        itemDto.IsActive = reader.GetBoolean(reader.GetOrdinal("items_active"));
                        itemDto.Price = reader.GetDecimal(reader.GetOrdinal("items_price"));
                        itemDto.Discount = reader.GetInt32(reader.GetOrdinal("items_discount"));
                        itemDto.CategoryId = reader.GetInt32(reader.GetOrdinal("categoryId"));

                        created = reader.GetDateTime(reader.GetOrdinal("items_datetime"));
                    }
                    else
                        isFind = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                isFind = false;
            }
        }
        return isFind;
    }


    //----------------- admin -------------

    public static List<ItemAdminDTO> getAllItems(string? baseUrl = null)
    {
        List<ItemAdminDTO> listItems = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_GetAllItems", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string imageName = reader.IsDBNull(reader.GetOrdinal("items_image"))
                   ? ""
                   : reader.GetString(reader.GetOrdinal("items_image"));
                    string fullImageUrl = string.IsNullOrWhiteSpace(imageName) ? "" : $"{baseUrl}/api/ItemApi/image/{imageName}";



                    listItems.Add(new ItemAdminDTO(
                                reader.GetInt32(reader.GetOrdinal("items_id")),
                                reader.GetString(reader.GetOrdinal("items_name")),
                                reader.GetString(reader.GetOrdinal("items_name_ar")),
                                reader.GetInt32(reader.GetOrdinal("items_count")),
                                reader.GetBoolean(reader.GetOrdinal("items_active")),
                                reader.GetInt32(reader.GetOrdinal("categoryId")),
                                fullImageUrl,
                                reader.GetDateTime(reader.GetOrdinal("items_datetime")),
                                reader.GetString(reader.GetOrdinal("category_name")),
                                reader.GetString(reader.GetOrdinal("category_name_ar"))

                    ));
                }
            }
        }
        return listItems;
    }

    public static int AddNewItem(ItemDTO IDTO)
    {
        int newItemId = -1;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_AddNewItem", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemName", IDTO.Name);
            cmd.Parameters.AddWithValue("@ItemNameAr", IDTO.NameAr);
            cmd.Parameters.AddWithValue("@ItemDesc", IDTO.Desc);
            cmd.Parameters.AddWithValue("@ItemDescAr", IDTO.DescAr);
            cmd.Parameters.AddWithValue("@ItemPrice", IDTO.Price);
            cmd.Parameters.AddWithValue("@ItemCount", IDTO.Count);
            cmd.Parameters.AddWithValue("@ItemDiscount", IDTO.Discount);
            cmd.Parameters.AddWithValue("@ItemActive", IDTO.IsActive);
            cmd.Parameters.AddWithValue("@ItemCategoryId", IDTO.CategoryId);
            cmd.Parameters.AddWithValue("@ItemImage", IDTO.Image);

            var id_param = new SqlParameter("@NewItemId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(id_param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

                if (id_param.Value != null && id_param.Value != DBNull.Value)
                    newItemId = Convert.ToInt32(id_param.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
        return newItemId;
    }

    public static ItemInfoAdminDTO? getItemInfo(int itemId)
    {

        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_getItemInfo", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemId);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new ItemInfoAdminDTO(
                        reader.GetString(reader.GetOrdinal("items_desc")),
                        reader.GetString(reader.GetOrdinal("items_desc_ar")),
                        reader.GetDecimal(reader.GetOrdinal("items_price")),
                        reader.GetInt32(reader.GetOrdinal("items_discount"))
                    );
                }
                else
                {
                    return null;
                }
            }
        }

    }

    public static bool updateItemInfo(ItemDTO itemDto)
    {
        bool isSuccess = false;

        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_updateItem", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ItemId", itemDto.Id);
            cmd.Parameters.AddWithValue("@ItemName", itemDto.Name);
            cmd.Parameters.AddWithValue("@ItemNameAr", itemDto.NameAr);
            cmd.Parameters.AddWithValue("@ItemDesc", itemDto.Desc);
            cmd.Parameters.AddWithValue("@ItemDescAr", itemDto.DescAr);
            cmd.Parameters.AddWithValue("@ItemPrice", itemDto.Price);
            cmd.Parameters.AddWithValue("@ItemCount", itemDto.Count);
            cmd.Parameters.AddWithValue("@ItemDiscount", itemDto.Discount);
            cmd.Parameters.AddWithValue("@ItemIsActive", itemDto.IsActive);
            cmd.Parameters.AddWithValue("@ItemCategoryId", itemDto.CategoryId);
            cmd.Parameters.AddWithValue("@ItemImageName", itemDto.Image);

            SqlParameter isSuccess_param = new SqlParameter("@isSuccess", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(isSuccess_param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                if (isSuccess_param.Value != null && isSuccess_param.Value != DBNull.Value)
                    isSuccess = Convert.ToBoolean(isSuccess_param.Value);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: ${ex.Message}");
            }

        }
        return isSuccess;
    }

    public static bool deleteItem(int itemId)
    {
        bool isDeleted = false;

        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_admin_deleteItem", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@itemId", itemId);
            SqlParameter isDeleted_param = new SqlParameter("@isDeleted", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(isDeleted_param);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                if (isDeleted_param.Value != null && isDeleted_param.Value != DBNull.Value)
                    isDeleted = Convert.ToBoolean(isDeleted_param.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                isDeleted = false;
            }
        }
        return isDeleted;
    }



}