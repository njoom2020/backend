using System.Data;
using Microsoft.Data.SqlClient;
using Shared.DTOs;

namespace EcommerceApiDataLayer;



public class UserData
{

    public static int AddUser(UserDTO newUserDto, string password, string salt)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_AddNewUser", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", newUserDto.Name);
            cmd.Parameters.AddWithValue("@Email", newUserDto.Email);
            cmd.Parameters.AddWithValue("@Phone", (object)newUserDto.Phone ?? string.Empty);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Salt", salt);
            var outputIdParam = new SqlParameter("@NewUserId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);

            connection.Open();
            cmd.ExecuteNonQuery();

            if (outputIdParam.Value != null && outputIdParam.Value != DBNull.Value)
            {
                return Convert.ToInt32(outputIdParam.Value);
            }

            return -1;

        }
    }
    public static int GetUsersCountByEmail(string email)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetCountOfUsersByEmail", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
            var outputIdParam = new SqlParameter("@EmailCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);
            connection.Open();
            cmd.ExecuteNonQuery();
            return (int)outputIdParam.Value;
        }
    }
    public static int GetUsersCountByPhone(string phone)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetUserByPhone", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Phone", phone);
            var outputIdParam = new SqlParameter("@PhoneCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);
            connection.Open();
            cmd.ExecuteNonQuery();
            return (int)outputIdParam.Value;
        }

    }
    public static UserDTO? GetUserDtoByUserID(int id)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetUserByUserId", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", id);
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new UserDTO(
                        reader.GetInt32(reader.GetOrdinal("users_id")),
                        reader.GetString(reader.GetOrdinal("users_name")),
                        reader.GetString(reader.GetOrdinal("users_email")),
                        reader.GetString(reader.GetOrdinal("users_phone")),
                        reader.GetBoolean(reader.GetOrdinal("users_ActivePhone"))

                    );
                }
                else
                    return null;
            }
        }
    }
    public static bool GetOtherInfoForUser(int id, ref string password, ref string salt,
                                                ref bool apporve, ref DateTime created, ref DateTime updateDate)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetOtherInfoForUser", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", id);

            var output_password_param = new SqlParameter("@Password", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_password_param);

            var output_salt_param = new SqlParameter("@Salt", SqlDbType.NVarChar, 35)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_salt_param);

            var output_approve_param = new SqlParameter("@Approve", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_approve_param);

            var output_created_param = new SqlParameter("@Created", SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_created_param);

            var output_updateDate_param = new SqlParameter("@Update", SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_updateDate_param);

            connection.Open();
            cmd.ExecuteNonQuery();
            password = (string)output_password_param.Value;
            salt = (string)output_salt_param.Value;
            apporve = (bool)output_approve_param.Value;
            created = (DateTime)output_created_param.Value;
            updateDate = (DateTime)output_updateDate_param.Value;

            return true;
        }

    }
    public static List<UserDTO> GetAllUserData()
    {
        List<UserDTO> usersList = new List<UserDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetAllUsers", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    usersList.Add(new UserDTO(
                        reader.GetInt32(reader.GetOrdinal("users_id")),
                        reader.GetString(reader.GetOrdinal("users_name")),
                        reader.GetString(reader.GetOrdinal("users_email")),
                        reader.GetString(reader.GetOrdinal("users_phone")),
                        reader.GetBoolean(reader.GetOrdinal("users_ActivePhone"))
                    ));
                }
            }
        }
        return usersList;
    }
    public static bool UpdateUserById(int id, string name, string email, string phone, bool isActivePhone,
         string password, string salt, string verifyCode, bool approve)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_updateUser", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@VerfiyCode", verifyCode);
            cmd.Parameters.AddWithValue("@Approve", approve);
            cmd.Parameters.AddWithValue("@Salt", salt);
            cmd.Parameters.AddWithValue("@ActivePhone", isActivePhone);

            connection.Open();
            cmd.ExecuteNonQuery();
            return true;

        }
    }
    public static bool UpdateVerifyCodeByID(int userId, string verfiyCode)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_updateUserVerifyCode", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@VerifyCode", verfiyCode);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }


        }
    }
    public static string GetVerifyCodeByUserID(int id)
    {
        string verifyCode = "";
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetVerifyCodeByUserID", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", id);

            var output_verifyCode_Param = new SqlParameter("@VerifyCode", SqlDbType.NVarChar, 6)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output_verifyCode_Param);

            connection.Open();

            cmd.ExecuteNonQuery();

            verifyCode = (string)output_verifyCode_Param.Value;
            return verifyCode;
        }
    }
    public static UserDTO? GetUserDtoByEmail(string email)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetUserByEmail", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new UserDTO(
                        reader.GetInt32(reader.GetOrdinal("users_id")),
                        reader.GetString(reader.GetOrdinal("users_name")),
                        reader.GetString(reader.GetOrdinal("users_email")),
                        reader.GetString(reader.GetOrdinal("users_phone")),
                        reader.GetBoolean(reader.GetOrdinal("users_ActivePhone"))

                    );
                }
                else
                    return null;
            }
        }

    }

    public static UserDTO? GetUserDtoByEmail(string email, string role)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_role_GetUserByEmail", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Role", role);

            connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new UserDTO(
                        reader.GetInt32(reader.GetOrdinal("users_id")),
                        reader.GetString(reader.GetOrdinal("users_name")),
                        reader.GetString(reader.GetOrdinal("users_email")),
                        reader.GetString(reader.GetOrdinal("users_phone")),
                        reader.GetBoolean(reader.GetOrdinal("users_ActivePhone"))

                    );
                }
                else
                    return null;
            }
        }
    }

    public static bool UpdateUserDeviceToken(int userId, string deviceToken)
    {
        int rowAffected = 0;
        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_UpdateUserDeviceToken", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@DeviceToken", (object)deviceToken ?? DBNull.Value);

            connection.Open();
            rowAffected = cmd.ExecuteNonQuery();


        }
        return rowAffected > 0;
    }


    public static string? GetUserDeviceToken(int userId)
    {
        string? deviceToken = string.Empty;

        using (SqlConnection connection = new SqlConnection(ConnectionSetting._connectionString))
        using (SqlCommand cmd = new SqlCommand("SP_GetUserDeviceToken", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            var result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                deviceToken = result.ToString();
            }
        }

        return deviceToken;

    }








}

