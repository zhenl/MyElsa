using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

var builder = WebApplication.CreateBuilder(args);


var elsaSection = builder.Configuration.GetSection("Elsa");

// Elsa services.
builder.Services
    .AddElsa(elsa => elsa
        .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
        .AddConsoleActivities()
        .AddHttpActivities(elsaSection.GetSection("Server").Bind)
        .AddJavaScriptActivities()
    );

// Elsa API endpoints.
builder.Services.AddElsaApiEndpoints();

// Allow arbitrary client browser apps to access the API.
// In a production environment, make sure to allow only origins you trust.
builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition"))
);

var app = builder.Build();

app
    .UseCors()
    .UseHttpActivities()
    .UseRouting()
    .UseEndpoints(endpoints =>
     {
       // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
       endpoints.MapControllers();
     })
    .UseWelcomePage();



app.Run();
