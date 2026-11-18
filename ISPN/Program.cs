using ISPN.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;



var builder = WebApplication.CreateBuilder(args);

string firebaseKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-key.json");

if(File.Exists(firebaseKeyPath) && FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebaseKeyPath)
    });
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EmailService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();