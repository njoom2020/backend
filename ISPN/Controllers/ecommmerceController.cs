using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using EcommerceApiBusinessLayer;
using ISPN.Services;
using System.Security.Cryptography;
using static EcommerceApiBusinessLayer.User;
using System.Security.Cryptography.X509Certificates;
using static System.Console;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Ocsp;
using MailKit;
using Org.BouncyCastle.Bcpg.OpenPgp;
using EcommerceApiDataLayer;



namespace EcommerceApiBusinessLayer.Controllers
{


    [Route("api/EcommmerceApi")]
    [ApiController]
    public class ecommmerceController : ControllerBase
    {


        [HttpPost("AddUser", Name = "AddUser")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<UserDTO>> AddNewUser([FromBody] RegisterRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) ||
             string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("invalid user data.");

            UserDTO newUserDto = new UserDTO(0, request.Name, request.Email, request.Phone, false);
            User user = new User(newUserDto, request.Password, "", "", false, DateTime.Now, DateTime.Now, enMode.AddUser);
            user.Phone = request.Phone;
            if (user.Save())
            {
                if (user.Id == -2)
                    return Conflict(new { status = false, message = "The email address is already registered." });

                newUserDto.Id = user.Id;
                Console.WriteLine(user.Id);
                return CreatedAtRoute("GetUserById", new { id = newUserDto.Id }, newUserDto);
            }
            else
                return StatusCode(500, new { message = "Error: Data not saved. Failed to add new user." });
        }

        [HttpGet("All", Name = "GetAllUsers")]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            List<UserDTO> usersList = EcommerceApiBusinessLayer.User.GetAllUsers();
            if (usersList.Count == 0)
                return NotFound("no users found!.");
            return Ok(usersList);
        }

        [HttpGet("UserById/{id}", Name = "GetUserById")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UserDTO> GetUserById(int id)
        {
            if (id < 1)
                return BadRequest($"not accepted id: {id}");
            User? user = EcommerceApiBusinessLayer.User.GetUserByID(id);
            if (user == null)
                return NotFound("no user found.");

            return Ok(user.UDTO);
        }

        [HttpPost("LogIn", Name = "LogInByEmail")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<UserDTO> LogIn([FromBody] LoginRequestDto loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("email and password are required.");
            User? user = EcommerceApiBusinessLayer.User.GetUserByEmail(loginDto!.Email);
            if (user == null || !user.ValidatePassword(loginDto.Password))
                return Unauthorized(new { status = false, message = "invalid email or password." });
            if (user.Approve == false)
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    status = false,
                    message =
                "The account is not activated. Please enter the verification code."
                });

            return Ok(user.UDTO);
        }

        [HttpPost("Role_LogIn", Name = "Role_LogInByEmail")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<UserDTO> Role_LogIn([FromBody] LoginRequestDto loginDto)
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password) || string.IsNullOrWhiteSpace(loginDto.Role))
                return BadRequest("email and password are required.");
            User? user = EcommerceApiBusinessLayer.User.GetUserByEmail(loginDto!.Email, loginDto.Role);
            if (user == null || !user.ValidatePassword(loginDto.Password))
                return Unauthorized(new { status = false, message = "invalid email or password." });
            if (user.Approve == false)
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    status = false,
                    message =
                "The account is not activated. Please enter the verification code."
                });

            return Ok(user.UDTO);
        }


        [HttpPost("verifyCode", Name = "VerifyCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> IsVerifycodeAndActive([FromBody] VerifyCodeRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.VerifyCode))
                return BadRequest(new { message = "invalid parameters." });
            User? user;
            if (request.UserId < 1)
                user = EcommerceApiBusinessLayer.User.GetUserByEmail(request.Email);
            else
                user = EcommerceApiBusinessLayer.User.GetUserByID(request.UserId);
            if (user == null)
                return NotFound("no user found.");
            if ((DateTime.UtcNow - user.UpdateDate).TotalHours > 24)
                return BadRequest(new { message = "the verify code is out of date palease resend new verify code" });
            user.VerifyCode = EcommerceApiBusinessLayer.User.GetVerifyCodeByUserId(user.Id);
            if (user.VerifyCode == request.VerifyCode)
            {
                user.Email = request.Email;
                user.Approve = true;
                if (user.Save())
                    return Ok(true);
                else
                    return StatusCode(500, new { message = "verify code is true put error update new data " });
            }
            return Ok(false);
        }

        [HttpPost("CheckEmailAndSendVerifyCode", Name = "CheckEmailAndSendVerifyCode")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CheckEmailAndSendVerifyCode([FromBody] CheckEmailRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.email))
            {
                return BadRequest(new { status = false, message = "email parameter is required" });
            }

            User? user = EcommerceApiBusinessLayer.User.GetUserByEmail(request.email);
            if (user == null)
                return NotFound(new { status = false, message = "this email is not registered before" });
            string verifyCode = new Random().Next(10000, 99999).ToString();
            user.VerifyCode = verifyCode;
            user.UpdateDate = DateTime.UtcNow;
            if (!user.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "error updating verify code" });
            var result = await EcommerceApiBusinessLayer.User.GenrateAndSendVerifyCodeAsync(user.Id, _emailService.SendEmailAsync);
            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = result.Message });
            return Ok(new { status = true, userId = user.Id });

        }

        [HttpPost("Role_CheckEmailAndSendVerifyCode", Name = "Role_CheckEmailAndSendVerifyCode")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Role_CheckEmailAndSendVerifyCode([FromBody] CheckEmailRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.email) ||string.IsNullOrWhiteSpace(request.role))
            {
                return BadRequest(new { status = false, message = "email parameter is required" });
            }

            User? user = EcommerceApiBusinessLayer.User.GetUserByEmail(request.email, request.role);
            if (user == null)
                return NotFound(new { status = false, message = "this email is not registered before" });
            string verifyCode = new Random().Next(10000, 99999).ToString();
            user.VerifyCode = verifyCode;
            user.UpdateDate = DateTime.UtcNow;
            if (!user.Save())
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "error updating verify code" });
            var result = await EcommerceApiBusinessLayer.User.GenrateAndSendVerifyCodeAsync(user.Id, _emailService.SendEmailAsync);
            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = result.Message });
            return Ok(new { status = true, userId = user.Id });

        }


        [HttpPost("SendVerifyCode")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendVerifyCode([FromBody] SendCodeRequestDTO request)
        {
            if (request == null || request.UserId <= 0)
                return BadRequest(new { message = "Invalid User ID" });

            try
            {
                var result = await EcommerceApiBusinessLayer.User.GenrateAndSendVerifyCodeAsync(
                    request.UserId,
                    (email, subject, body) => _emailService.SendEmailAsync(email, subject, body)
                );
                if (result.Success)
                    return Ok(new { message = result.Message });
                return StatusCode(500, new { message = result.Message });
            }
            catch (Exception ex)
            {
                WriteLine($"Error sendVerifyCode: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "An error occurred",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }
        private readonly EmailService _emailService;
        public ecommmerceController(EmailService emailService)
        {
            _emailService = emailService;
        }
        public class TestEmailDTO
        {
            public string Email { get; set; } = string.Empty;
        }
        [HttpPost("TestEmail")]
        public async Task<IActionResult> TestEmail([FromBody] TestEmailDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            try
            {
                int verifyCode = System.Security.Cryptography.RandomNumberGenerator.GetInt32(10000, 100000);
                string subject = "verfiy code:";


                string body = @"
                                    <div style='font-family: Arial, sans-serif; text-align: center; padding: 25px; border: 1px solid #e0e0e0; border-radius: 10px; max-width: 500px; margin: auto;'>
                                        <h2 style='font-size: 26px; color: #1a1a1a; margin-bottom: 8px;'>Welcome!</h2>
                                        <p style='font-size: 18px; color: #555555; margin-bottom: 25px;'>Your verification code is:</p>
                                        <div style='background-color: #f0f4f9; border-radius: 8px; padding: 15px 30px; display: inline-block;'>
                                            <span style='font-size: 38px; font-weight: bold; letter-spacing: 8px; color: #007bff;'>" + verifyCode + @"</span>
                                        </div>
                                        <p style='font-size: 13px; color: #888888; margin-top: 25px;'>This code is valid for a limited time. Please do not share it with anyone.</p>
                                    </div>";

                // استقبال الـ Tuple الصحيح (Success, ErrorMessage)
                var result = await _emailService.SendEmailAsync(request.Email, subject, body);

                if (result.Success)
                    return Ok(new { message = $"Email sent successfully to {request.Email}" });

                return StatusCode(500, new { message = "Failed to send email", error = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to send email", error = ex.Message });
            }
        }

        [HttpPost("ResetPassword", Name = "ResetPassword")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<bool> ResetPassword([FromBody] ResetPasswordRequestDto RPRequestDto)
        {
            if (RPRequestDto == null || RPRequestDto.UserId < 1 || string.IsNullOrWhiteSpace(RPRequestDto.NewPassword))
                return BadRequest(new { status = false, message = "not invalid data!." });
            User? user = EcommerceApiBusinessLayer.User.GetUserByID(RPRequestDto.UserId);
            if (user == null)
                return NotFound(new { status = false, message = "no user found!." });
            string newSalt = HashHelper.GenerateSalt();
            string newHashPassword = HashHelper.ComputeSha256Hash(RPRequestDto.NewPassword, newSalt);
            user.Password = newHashPassword;
            user.Salt = newSalt;
            if (user.Save())
                return Ok(true);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "falid to update password." });
        }



        [HttpPost("SendVerifyCodeByEmail")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendVerifyCodeByEmail([FromBody] ChangeAndVerifyEmailRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.OldEmail) || string.IsNullOrWhiteSpace(request.NewEmail))
                return BadRequest(new { status = false, message = "UserId and Email parameters are required" });
            try
            {
                User? user = EcommerceApiBusinessLayer.User.GetUserByEmail(request.OldEmail);
                if (user == null)
                    return NotFound(new { status = false, message = "user not found" });
                if (request.OldEmail != request.NewEmail)
                {
                    bool isNewEmailTaken = EcommerceApiBusinessLayer.User.IsEmailExsit(request.NewEmail);
                    if (isNewEmailTaken)
                        return BadRequest(new { status = false, message = "the new email is already registered to another account." });
                    user.Email = request.NewEmail;
                    if (!user.Save())
                        return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { status = false, message = "failed to update email in database" });
                }
                var result = await EcommerceApiBusinessLayer.User.GenrateAndSendVerifyCodeAsync(user.Id,
                (email, subject, body) => _emailService.SendEmailAsync(email, subject, body));
                if (result.Success)
                    return Ok(new { status = true, messag = result.Message, userId = user.Id });
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = result.Message });
            }
            catch (Exception ex)
            {
                WriteLine($"Error SendVerifyCodeByEmail: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                            new { status = false, message = "an error occurred", error = ex.Message });
            }

        }

        [HttpPost("UpdateDeviceToken")]
        public IActionResult UpdateDeviceToken([FromQuery] int userId, string deviceToken)
        {
            if (userId < 1 || string.IsNullOrWhiteSpace(deviceToken))
                return BadRequest(new { status = false, message = "invalid paramentr." });

            bool isUpdate = EcommerceApiBusinessLayer.User.updateDeviceToken(userId, deviceToken);

            if (isUpdate)
                return Ok(new { status = true, message = "update device token done successfully." });

            return NotFound(new { status = false, message = "no user found" });
        }


        [HttpGet("test-topic-notification")]
        public async Task<IActionResult> TestTopicNotification(string topicName)
        {
            bool result = await NotificationServices.SendNotificationToTopicAsync(
                topicName,
                "🔥 عرض خاص من المتجر!",
                "تم تفعيل الخصم الخاص لجميع المستخدمين المشتركين!"
            );

            if (result)
                return Ok(new { status = "Success", message = "تم إرسال إشعار الـ Topic بنجاح" });

            return BadRequest(new { status = "Failed", message = "فشل إرسال إشعار الـ Topic" });
        }

    }
}

