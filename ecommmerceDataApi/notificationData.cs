
using System.Data;
using Microsoft.Data.SqlClient;
using Shared.NotificationDTOs;

namespace EcommerceApiDataLayer;
public class NotificationData
{
    public static List<NotificationDTO> getAllNotificationsUser(int userId)
    {
        List<NotificationDTO> notificationsList = [];

        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_GetAllNotificationUser", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId",userId);

            connection.Open();

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    notificationsList.Add(new NotificationDTO(
                        reader.GetInt32(reader.GetOrdinal("notifi_id")),
                        reader.GetString(reader.GetOrdinal("notifi_title")),
                        reader.GetString(reader.GetOrdinal("notifi_titleAr")),
                        reader.GetString(reader.GetOrdinal("notifi_body")),
                        reader.GetString(reader.GetOrdinal("notifi_bodyAr")),
                        userId,
                        false,
                        reader.GetDateTime(reader.GetOrdinal("notifi_datatime"))
                        
                    ));
                }
            }
        }
        return notificationsList;
    }
}
