using EcommerceApiDataLayer;
using Shared.NotificationDTOs;

namespace EcommerceApiBusinessLayer;

public class Notification
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public NotificationDTO NDTO
    {
        get
        {
            return (new NotificationDTO(
                                    this.Notifi_Id,
                                    this.Notifi_title,
                                    this.Notifi_titleAr,
                                    this.Notifi_body,
                                    this.Notifi_bodyAr,
                                    this.Notifi_userId,
                                    this.Notifi_delete,
                                    this.Notifi_created));
        }
    }
    public int Notifi_Id { get; set; }
    public string Notifi_title { get; set; } = "";
    public string Notifi_titleAr { get; set; } = "";
    public string Notifi_body { get; set; } = "";
    public string Notifi_bodyAr { get; set; } = "";
    public int Notifi_userId { get; set; }
    public bool Notifi_delete { get; set; }
    public DateTime Notifi_created { get; set; }


    public static List<NotificationDTO> getAllNotificationsUser(int userId)
    {
        return NotificationData.getAllNotificationsUser(userId);
    }


}
