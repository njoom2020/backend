using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using EcommerceApiDataLayer;
using Microsoft.IdentityModel.Logging;
using Shared.DTOs;

namespace EcommerceApiBusinessLayer;

public class User
{
    public enum enMode { AddUser = 0, Update = 1 }
    public enMode Mode = enMode.AddUser;
    public UserDTO UDTO
    {
        get
        {
            return (new UserDTO(this.Id, this.Name,
            this.Email, this.Phone, this.IsActivePhone));
        }
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsActivePhone { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string VerifyCode { get; set; }
    public bool Approve { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime Created { get; set; }

    public User(UserDTO userDTO, string password, string salt, string verifyCode, bool approve,
                                        DateTime created, DateTime updateDate, enMode cMode = enMode.AddUser)
    {
        this.Id = userDTO.Id;
        this.Name = userDTO.Name;
        this.Email = userDTO.Email;
        this.Phone = userDTO.Phone;
        this.IsActivePhone = userDTO.IsActivePhone;

        Mode = cMode;

        if (cMode == enMode.AddUser)
        {
            this.Salt = HashHelper.GenerateSalt();
            this.Password = HashHelper.ComputeSha256Hash(password, this.Salt);
        }
        else
        {
            this.Password = password;
            this.Salt = salt;
        }
        this.VerifyCode = verifyCode;
        this.Approve = approve;
        this.UpdateDate = updateDate;
        this.Created = created;
    }
    public static List<UserDTO> GetAllUsers()
    {
        return UserData.GetAllUserData();
    }
    public static User? GetUserByID(int id)
    {
        UserDTO? UDto = UserData.GetUserDtoByUserID(id);
        if (UDto != null)
        {
            string password = "";
            string salt = "";
            bool Approve = false;
            string verifyCode = "";
            DateTime created = DateTime.Now;
            DateTime updateDate = DateTime.Now;

            if (UserData.GetOtherInfoForUser(id, ref password, ref salt, ref Approve, ref created, ref updateDate))
                return new User(UDto, password, salt, verifyCode, Approve, created, updateDate, enMode.Update);
        }

        return null;
    }
    public static int GetUsersCountByEmail(string email)
    {
        return UserData.GetUsersCountByEmail(email);
    }
    public static bool IsEmailExsit(string email)
    {
        return GetUsersCountByEmail(email) > 0;
    }
    public static int GetUsersCountByPhone(string email)
    {
        return UserData.GetUsersCountByPhone(email);
    }
    private bool _AddNewUser()
    {
        this.Id = UserData.AddUser(UDTO, this.Password, this.Salt);
        return this.Id != -1;
    }
    public int AddNewUser(RegisterRequestDto request)
    {
        UserDTO Udto = new UserDTO(0,request.Name,request.Email, request.Phone, false);
        return UserData.AddUser(Udto, this.Password, this.Salt);
    }
    private bool _UpdateUser()
    {
        return UserData.UpdateUserById(this.Id, this.Name, this.Email, this.Phone, this.IsActivePhone,
         this.Password, this.Salt, this.VerifyCode, this.Approve);

    }
    public static bool UpdateVerifyCodeByUserId(int userId, string verfiyCode)
    {
        return UserData.UpdateVerifyCodeByID(userId, verfiyCode);
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddUser:
                Mode = enMode.Update;
                return _AddNewUser();
            case enMode.Update:
                return _UpdateUser();
        }
        return false;
    }
    public static async Task<(bool Success, string Message)> GenrateAndSendVerifyCodeAsync
    (int userId, Func<string, string, string, Task<(bool Success, string ErrorMessage)>> sendEmailFunc)
    {
        User? user = GetUserByID(userId);
        if (user == null)
            return (false, "no user found.");
        if (string.IsNullOrWhiteSpace(user.Email))
            return (false, "no email found");
        int verifyCode = RandomNumberGenerator.GetInt32(10000, 100000);

        bool isUpdated = UserData.UpdateVerifyCodeByID(userId, verifyCode.ToString());
        if (!isUpdated)
            return (false, "update faild");
        string subject = "your verification Code";
        string body = $@"
        <div style='font-family: Arial, sans-serif; text-align: center; padding: 25px; border: 1px solid #e0e0e0; border-radius: 10px; max-width: 500px; margin: auto;'>
            <h2 style='font-size: 26px; color: #1a1a1a; margin-bottom: 8px;'>Welcome!</h2>
            <p style='font-size: 18px; color: #555555; margin-bottom: 25px;'>Your verification code is:</p>
            <div style='background-color: #f0f4f9; border-radius: 8px; padding: 15px 30px; display: inline-block;'>
                <span style='font-size: 38px; font-weight: bold; letter-spacing: 8px; color: #007bff;'>{verifyCode}</span>
            </div>
            <p style='font-size: 13px; color: #888888; margin-top: 25px;'>This code is valid for a limited time. Please do not share it with anyone.</p>
        </div>";
        var result = await sendEmailFunc(user.Email, subject, body);
        return result.Success ? (true, "email send successfully.") : (false, result.ErrorMessage);
    }
    public static bool IsVerifycode(string email, string verifyCode, int userId)
    {
        User? user = User.GetUserByID(userId);
        if (user == null)
            return false;

        if (user.VerifyCode == verifyCode)
        {
            user.Email = email;
            user.Approve = true;
            if (user.Save())
                return true;
        }
        return false;
    }
    public static string GetVerifyCodeByUserId(int id)
    {
        return UserData.GetVerifyCodeByUserID(id);
    }
    public static User? GetUserByEmail(string email)
    {
        UserDTO? UDto = UserData.GetUserDtoByEmail(email);
        if (UDto != null)
        {
            string password = "";
            string salt = "";
            bool Approve = false;
            string verifyCode = "";
            DateTime created = DateTime.Now;
            DateTime updateDate = DateTime.Now;

            if (UserData.GetOtherInfoForUser(UDto.Id, ref password, ref salt, ref Approve, ref created, ref updateDate))
                return new User(UDto, password, salt, verifyCode, Approve, created, updateDate, enMode.Update);
        }

        return null;
    }

    public static User? GetUserByEmail(string email, string role)
    {
        UserDTO? UDto = UserData.GetUserDtoByEmail(email, role);
        if (UDto != null)
        {
            string password = "";
            string salt = "";
            bool Approve = false;
            string verifyCode = "";
            DateTime created = DateTime.Now;
            DateTime updateDate = DateTime.Now;

            if (UserData.GetOtherInfoForUser(UDto.Id, ref password, ref salt, ref Approve, ref created, ref updateDate))
                return new User(UDto, password, salt, verifyCode, Approve, created, updateDate, enMode.Update);
        }

        return null;
    }

public bool ValidatePassword(string inputPassword)
    {
        if(string.IsNullOrWhiteSpace(inputPassword) || string.IsNullOrWhiteSpace(this.Salt))
         return false;

         string computedHash = HashHelper.ComputeSha256Hash(inputPassword, this.Salt);

         return string.Equals(this.Password, computedHash, StringComparison.Ordinal);
    }

public static bool updateDeviceToken(int userId, string deviceToken)
    {
        return UserData.UpdateUserDeviceToken(userId, deviceToken);
    }

}
