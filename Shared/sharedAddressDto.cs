namespace Shared.ItemDTOs;

public class AddressDTO
{
    public AddressDTO(int address_id, int address_userId, string address_name, string address_city, string address_street, decimal address_lat, decimal address_long)
    {
        this.Address_id = address_id;
        this.Address_userId = address_userId;
        this.Address_name = address_name;
        this.Address_city = address_city;
        this.Address_street = address_street;
        this.Address_lat = address_lat;
        this.Address_long = address_long;
    }
    public int Address_id { get; set; }
    public int Address_userId { get; set; }
    public string Address_name { get; set; }
    public string Address_city { get; set; }
    public string Address_street { get; set; }
    public decimal Address_lat { get; set; }
    public decimal Address_long { get; set; }

}
