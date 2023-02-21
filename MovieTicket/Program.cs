using MovieTicket;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BUS.Services;
using DAL;

Console.OutputEncoding = Encoding.Unicode;
Console.InputEncoding = Encoding.Unicode;


var app = new Application();

var builder = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", true, true);
var configuration = builder.Build();

// Configure MailSettings and Email Sender
app.Services.AddOptions();
app.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
app.Services.AddScoped<IEmailSender, SendEmailService>();

// Configure DbConnection
app.Services.AddSingleton<IDbConnection>(serviceprovider =>
	new DbConnection(configuration["ConnectionStrings:DefaultConnection"])
);

app.AddViewServices();
app.AddBusinesses();

app.Run();
