using Backend.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//read dump.sql and execute it
var sql = File.ReadAllText("dump.sql");
var db = new DatabaseManager();
await db.Execute(sql);


builder.Services.AddControllers();
builder.Services.AddTransient(s => new DatabaseManager());
builder.Services.AddSingleton(new SessionManager());
builder.Services.AddTransient(s => new GroupManager(new DatabaseManager()));
builder.Services.AddTransient(s => new UserManager(new DatabaseManager()));
builder.Services.AddTransient(s => new MessageManager(new DatabaseManager()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Backend", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
