using System.Data;
using Microsoft.Data.SqlClient;
using Shared.SettingDTOs;

namespace EcommerceApiDataLayer;

public class SettingData
{
    public static SettingDTO getLastSetting(string baseUrl, byte appType = 0)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_getSetting", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppType",appType);

            connection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                     string imageName = reader.IsDBNull(reader.GetOrdinal("setting_image"))
                    ? ""
                    : reader.GetString(reader.GetOrdinal("setting_image"));
                    string fullImageUrl = string.IsNullOrWhiteSpace(imageName) ? "" : $"{baseUrl}/api/SettingApi/image/{imageName}";

                    return new SettingDTO(
                        setting_id: reader.GetInt32(reader.GetOrdinal("setting_id")),
                        setting_titleOme: reader.GetString(reader.GetOrdinal("setting_titleOme")),
                        setting_titleOmeAr: reader.GetString(reader.GetOrdinal("setting_titleOmeAr")),
                        setting_bodyOme: reader.GetString(reader.GetOrdinal("setting_bodyOme")),
                        setting_bodyOmeAr: reader.GetString(reader.GetOrdinal("setting_bodyOmeAr")),
                        setting_deliveryTime: reader.GetInt32(reader.GetOrdinal("setting_deliveryTime")),
                        setting_created: reader.GetDateTime(reader.GetOrdinal("setting_created")),
                        fullImageUrl,
                        setting_appType: reader.GetByte(reader.GetOrdinal("setting_appType"))
                        );
                }
                return new SettingDTO();
                
            }

        }


    }
}