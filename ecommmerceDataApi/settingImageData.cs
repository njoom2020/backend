using System.Data;
using Microsoft.Data.SqlClient;
using Shared.SettingImageDTOs;

namespace EcommerceApiDataLayer;

public class SettingImageData
{
    public static List<SettingImageDTO> getAllSettingImages(int settingId, string baseUrl)
    {
        List<SettingImageDTO> settingimagesList = [];
        
        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_Settings_GetAllImages", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SettingId", settingId);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string fileName = reader.IsDBNull(reader.GetOrdinal("image_fileName"))
                    ?""
                    :reader.GetString(reader.GetOrdinal("image_fileName"));

                    string folderName = reader.IsDBNull(reader.GetOrdinal("image_folderName"))
                    ?""
                    :reader.GetString(reader.GetOrdinal("image_folderName"));

                    string fullImageUrl = "";

                    if(string.IsNullOrWhiteSpace(fileName) && string.IsNullOrWhiteSpace(folderName))
                        fullImageUrl = "";
                    else
                        fullImageUrl = $"{baseUrl}/api/ImageApi/image/{folderName}/{fileName}";

                    settingimagesList.Add(
                        new SettingImageDTO(
                            reader.GetInt32(reader.GetOrdinal("r_id")),
                            settingId,
                            reader.GetInt32(reader.GetOrdinal("image_id")),
                            reader.GetInt32(reader.GetOrdinal("imageIndex")),
                            fullImageUrl,
                            reader.GetString(reader.GetOrdinal("image_folderName")),
                            reader.GetString(reader.GetOrdinal("title")),
                            reader.GetString(reader.GetOrdinal("body"))
                            
                        )
                    );
                }
            }
        }

        return settingimagesList;
    }



}