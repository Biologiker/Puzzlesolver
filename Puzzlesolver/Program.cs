using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;


IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Set the base path for appsettings.json
    .AddJsonFile("appsettings.json") // Add the appsettings.json file
    .Build();

var connectionUri = configuration.GetSection("ConnectionStrings:MongoDB").Value;
var settings = MongoClientSettings.FromConnectionString(connectionUri);
// Set the ServerApi field of the settings object to set the version of the Stable API on the client
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
// Create a new client and connect to the server
var client = new MongoClient(settings);
// Send a ping to confirm a successful connection
try
{
    Console.WriteLine(connectionUri);
    //var result = client.GetDatabase("Puzzlesolver").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    //client.ListDatabaseNames(default);
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));//set connection string
builder.Services.AddSingleton<IMongoClient>(mongoClient);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
