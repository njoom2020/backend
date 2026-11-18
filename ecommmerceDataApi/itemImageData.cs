using System.Data;
using Microsoft.Data.SqlClient;
using Shared.ItemImageDTOs;

namespace EcommerceApiDataLayer;

public class ItemImageData
{
    public static List<ItemImageDTO> getAllItemImages(int itemId, string baseUrl)
    {
        List<ItemImageDTO> itemImagesList = [];
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_Items_GetAllImages", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", itemId);
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string fileName = reader.IsDBNull(reader.GetOrdinal("image_fileName"))
                    ? ""
                    : reader.GetString(reader.GetOrdinal("image_fileName"));
                    string folderName = reader.IsDBNull(reader.GetOrdinal("image_folderName"))
                    ? ""
                    : reader.GetString(reader.GetOrdinal("image_folderName"));
                    string fullImageUrl = "";
                    if (string.IsNullOrWhiteSpace(fileName) && string.IsNullOrWhiteSpace(folderName))
                        fullImageUrl = "";
                    else
                        fullImageUrl = $"{baseUrl}/api/ImageApi/image/{folderName}/{fileName}";
                    itemImagesList.Add(
                        new ItemImageDTO(
                            reader.GetInt32(reader.GetOrdinal("r_id")),
                            itemId,
                            reader.GetInt32(reader.GetOrdinal("Image_id")),
                            reader.GetInt32(reader.GetOrdinal("Image_index")),
                            fullImageUrl,
                            reader.GetString(reader.GetOrdinal("image_folderName"))
                        )
                    );
                }
            }
        }
        return itemImagesList;
    }
}