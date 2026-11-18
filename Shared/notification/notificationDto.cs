namespace Shared.NotificationDTOs;

public class NotificationDTO
{


    public NotificationDTO(
        int notifi_Id,
    string notifi_titel,
    string notifi_titelAr,
    string notifi_body,
    string notifi_bodyAr,
    int notifi_userId,
    bool notifi_delete,
    DateTime notifi_created
    )
    {
        this.Notifi_Id = notifi_Id;
        this.Notifi_titel = notifi_titel;
        this.Notifi_titleAr = notifi_titelAr;
        this.Notifi_body = notifi_body;
        this.Notifi_bodyAr = notifi_bodyAr;
        this.Notifi_userId = notifi_userId;
        this.Notifi_delete = notifi_delete;
        this.Notifi_created = notifi_created;
    }




    public int Notifi_Id { get; set; }
    public string Notifi_titel { get; set; }
    public string Notifi_titleAr { get; set; }
    public string Notifi_body { get; set; }
    public string Notifi_bodyAr { get; set; }
    public int Notifi_userId { get; set; }
    public bool Notifi_delete { get; set; }
    public DateTime Notifi_created { get; set; }




}
