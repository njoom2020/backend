using EcommerceApiDataLayer;
using Shared.SettingDTOs;

namespace EcommerceApiBusinessLayer;

public class Setting
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public SettingDTO SDTO
    {
        get
        {
            return new SettingDTO(
                this.Setting_id,
                this.Setting_titleOme,
                this.Setting_titleOmeAr,
                this.Setting_bodyOme,
                this.Setting_bodyOmeAr,
                this.Setting_deliveryTime,
                this.Setting_created,
                this.Setting_image,
                this.Setting_appType
            );
        }
    }

    public int Setting_id { get; set; }
    public string Setting_titleOme { get; set; } = "";
    public string Setting_titleOmeAr { get; set; } = "";
    public string Setting_bodyOme { get; set; } = "";
    public string Setting_bodyOmeAr { get; set; } = "";
    public int Setting_deliveryTime { get; set; }
    public DateTime Setting_created { get; set; }
    public byte Setting_appType { get; set; }
    public string Setting_image { get; set; }

    public Setting(SettingDTO settingDto, enMode cMode = enMode.AddNew)
    {
        this.Setting_id = settingDto.Setting_id;
        this.Setting_titleOme = settingDto.Setting_titleOme;
        this.Setting_titleOmeAr = settingDto.Setting_titleOmeAr;
        this.Setting_bodyOme = settingDto.Setting_bodyOme;
        this.Setting_bodyOmeAr = settingDto.Setting_bodyOmeAr;
        this.Setting_deliveryTime = settingDto.Setting_deliveryTime;
        this.Setting_created = settingDto.Setting_created;
        this.Setting_appType = settingDto.Setting_appType;
        this.Setting_image = settingDto.Setting_image;
    }

    public static SettingDTO getLastSetting(string baseUrl, byte appType = 0)
    {
        return SettingData.getLastSetting(baseUrl, appType);
    }
}