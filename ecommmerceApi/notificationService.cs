using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

public class NotificationServices
{
    private static void EnsureInitialized()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string keyPath = Path.Combine(baseDir, "firebase-key.json");

            // في حال عدم وجوده في مجلد الـ bin، نبحث في مجلد المشروع الرئيسي
            if (!File.Exists(keyPath))
            {
                keyPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase-key.json");
            }

            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException($"لم يتم العثور على ملف firebase-key.json في المسار: {keyPath}");
            }

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(keyPath)
            });
        }
    }

    public static async Task<bool> SendNotificationAsync(string deviceToken, string title, string body)
    {
        try
        {
            EnsureInitialized();

            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return !string.IsNullOrEmpty(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FCM ERROR]: {ex.ToString()}");
            throw; // إعادة رفع الاستثناء لمعرفة السبب في Swagger
        }
    }

    public static async Task<bool> SendNotificationToTopicAsync(string topicName, string title, string body,
    Dictionary<string,string>? customData = null)
            
    {
        try
        {
            EnsureInitialized();

            var message = new Message()
            {
                Topic = topicName,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    
                    
                    
                },
                Data = customData ?? new Dictionary<string, string>()
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return !string.IsNullOrEmpty(response);
        }catch(Exception ex)
        {
            Console.WriteLine($"[topic notification error]: {ex.Message}");
            throw;
        }
    }
}