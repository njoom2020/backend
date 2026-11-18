using System.Data;
using System.Net.Mail;
using System.Security.Cryptography.Pkcs;
using Microsoft.Data.SqlClient;
using Shared.ItemDTOs;

namespace EcommerceApiDataLayer;
public class AddressData
{
    public static int addNewAddress(AddressDTO addressDto)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_AddNewAddress", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", addressDto.Address_userId);
            cmd.Parameters.AddWithValue("@Name", addressDto.Address_name);
            cmd.Parameters.AddWithValue("@City", addressDto.Address_city);
            cmd.Parameters.AddWithValue("@Street", addressDto.Address_street);
            cmd.Parameters.AddWithValue("@Lat", addressDto.Address_lat);
            cmd.Parameters.AddWithValue("@Long", addressDto.Address_long);

            var output_addressId_param = new SqlParameter("@AddressId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_addressId_param);

            connection.Open();
            cmd.ExecuteNonQuery();

            if(output_addressId_param != null && output_addressId_param.Value != DBNull.Value)
            {
                return Convert.ToInt32(output_addressId_param.Value);
            }
            return -1;
        }
    }

    public static List<AddressDTO> getAllAddressesByUserId(int userId)
    {
        List<AddressDTO> addressesList = new List<AddressDTO>();

        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_getAllUserAddress", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            connection.Open();

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    addressesList.Add(
                        new AddressDTO(
                            address_id: reader.GetInt32(reader.GetOrdinal("address_id")),
                            address_userId:  reader.GetInt32(reader.GetOrdinal("address_userId")),
                            address_name: reader.GetString(reader.GetOrdinal("address_name")),
                            address_city: reader.GetString(reader.GetOrdinal("address_city")),
                            address_street: reader.GetString(reader.GetOrdinal("address_street")),
                            address_lat: reader.GetDecimal(reader.GetOrdinal("address_lat")),
                            address_long: reader.GetDecimal(reader.GetOrdinal("address_long"))
                        )
                        
                    );
                    
                }
            }

            return addressesList;
        }
    }

    public static bool deleteAddressByAddressId(int addressId, int userId)
    {
        bool isDelete = false;
        using(SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using(SqlCommand cmd = new SqlCommand("SP_deleteAddressByAddressId", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AddressId", addressId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            var output_isDelete_param = new SqlParameter("@isDelete", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_isDelete_param);

            connection.Open();

            cmd.ExecuteNonQuery();

            if(output_isDelete_param.Value != null && output_isDelete_param.Value !=  DBNull.Value)
                isDelete = Convert.ToBoolean(output_isDelete_param.Value);
            
        }
        return isDelete;
    }
}
