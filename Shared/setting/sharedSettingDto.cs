namespace Shared.SettingDTOs;

public class SettingDTO
{

    public SettingDTO() { }

    public SettingDTO(
        int setting_id,
        string setting_titleOme,
        string setting_titleOmeAr,
        string setting_bodyOme,
        string setting_bodyOmeAr,
        int setting_deliveryTime,
        DateTime setting_created,
        string setting_image,
        byte setting_appType
        )
    {


        Setting_id = setting_id;
        Setting_titleOme = setting_titleOme;
        Setting_titleOmeAr = setting_titleOmeAr;
        Setting_bodyOme = setting_bodyOme;
        Setting_bodyOmeAr = setting_bodyOmeAr;
        Setting_deliveryTime = setting_deliveryTime;
        Setting_created = setting_created;
        Setting_image = setting_image;
        Setting_appType = setting_appType;

    }
    public int Setting_id { get; set; }
    public string Setting_titleOme { get; set; } = "";
    public string Setting_titleOmeAr { get; set; } = "";
    public string Setting_bodyOme { get; set; } = "";
    public string Setting_bodyOmeAr { get; set; } = "";
    public int Setting_deliveryTime { get; set; }
    public DateTime Setting_created { get; set; }
    public string Setting_image { get; set; } = "";
    public byte Setting_appType { get; set; }
}
