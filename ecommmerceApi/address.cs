using EcommerceApiDataLayer;
using Shared.ItemDTOs;

namespace EcommerceApiBusinessLayer;

public class Address
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;
    public AddressDTO addressDTO
    {
        get
        {
            return (new AddressDTO(this.Address_id, this.Address_userId, this.Address_name,
             this.Address_city, this.Address_street, this.Address_lat, this.Address_long));
        }
    }
    public int Address_id { get; set; }
    public int Address_userId { get; set; }
    public string Address_name {get; set;}
    public string Address_city { get; set; }
    public string Address_street { get; set; }
    public decimal Address_lat { get; set; }
    public decimal Address_long { get; set; }

    public Address(AddressDTO addressDto, enMode cMode = enMode.AddNew)
    {
        this.Address_id = addressDto.Address_id;
        this.Address_userId = addressDto.Address_userId;
        this.Address_name = addressDto.Address_name;
        this.Address_city = addressDto.Address_city;
        this.Address_street = addressDto.Address_street;
        this.Address_lat = addressDto.Address_lat;
        this.Address_long = addressDto.Address_long;

        Mode = cMode;
    }

    private bool _AddNew()
    {
        this.Address_id = AddressData.addNewAddress(addressDTO);
        return this.Address_id > 0;
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
            Mode = enMode.Update;
            return _AddNew();
        }
        return false;
    }

    public static List<AddressDTO> getAllAddressesByUserId(int userId)
    {
        return AddressData.getAllAddressesByUserId(userId);
    }
    public static bool deleteAddressByAddressId(int addressId, int userId)
    {
        return AddressData.deleteAddressByAddressId(addressId, userId);
    }


}
