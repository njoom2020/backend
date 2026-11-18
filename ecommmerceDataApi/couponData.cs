using System.Data;
using Microsoft.Data.SqlClient;
using Shared.CouponDTOs;

namespace EcommerceApiDataLayer;

public class CouponData
{
    public static CouponDTO checkCoupon(string couponName)
    {
        CouponDTO couponDto = new CouponDTO();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_checkCoupon", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CouponName", couponName);

            connection.Open();
            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    couponDto = new CouponDTO(
                        reader.GetInt32(reader.GetOrdinal("coupon_id")),
                        reader.GetString(reader.GetOrdinal("coupon_name")),
                        reader.GetInt32(reader.GetOrdinal("coupon_count")),
                        reader.GetInt32(reader.GetOrdinal("coupon_discount")),
                        reader.GetDateTime(reader.GetOrdinal("coupon_expiredata"))
                        
                    );
                }
            }
        }
        return couponDto;
    }
}