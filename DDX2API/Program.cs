using System.Security.Cryptography;
using System.Text;
using DDX2API;
using DDX2API.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
var publicKey = builder.Configuration["PublicKey"];
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/register", async (User user, AppDbContext db) =>
{
    using SHA256 sha256Hash = SHA256.Create();
    var userPublicKey = GetHash(sha256Hash, user.Phone + Guid.NewGuid());
    var secret = userPublicKey + publicKey;
    string hash = GetHash(sha256Hash, secret);
    user.Secret = hash;

    db.Add(user);
    db.SaveChanges();

    return userPublicKey;
});

app.MapGet("/login", (HttpContext context, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    user[0].Secret = "";
    return Results.Accepted(null, user[0]);
});

app.MapGet("/getUserInfo/{id}", (int id, HttpContext context, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    var profile = db.UserProfiles.Find(id);

    if (profile == null)
    {
        return Results.NotFound();
    }

    var info = new UserModel
    {
        UserId = id,
        City = profile.City,
        Name = profile.Name
    };
    
    return Results.Accepted(null, info);
});

app.MapGet("/getMyProfile", (HttpContext context, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    var profile = db.UserProfiles.Find(user[0].Id);
    
    return Results.Accepted(null, profile);
});

app.MapPost("/saveMyProfile", (HttpContext context, UserProfile profile, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    if (profile.UserId != user[0].Id)
    {
        return Results.BadRequest();
    }
    
    db.Entry(profile).State = EntityState.Modified;
    db.SaveChanges();
    
    return Results.Accepted(null, profile);
});


app.MapGet("/chats", (HttpContext context, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    List<Chat?> chats = new List<Chat?>();
    
    foreach (var i in user[0].Chats)
    {
        chats.Add(db.Chats.Find(i));
    }

    return Results.Accepted(null, chats);
});

app.MapPost("/newChat", (HttpContext context, Chat chat, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    var lastId = 1;

    if (db.Chats.Any(c => c.Id == 1))
    {
        var last = db.Chats.OrderByDescending(c => c.Id).FirstOrDefault();
        lastId = last.Id + 1;
    }

    if (chat.Users.Select(i => db.Users.Find(i)).Any(u => u == null))
    {
        return Results.BadRequest();
    }
    
    foreach (var i in chat.Users)
    {
        var u = db.Users.Find(i);
        u?.Chats.Add(lastId);
    }
    
    db.Add(chat);
    db.SaveChanges();
    
    var chats = new List<Chat?>();
    
    foreach (var i in user[0].Chats)
    {
        chats.Add(db.Chats.Find(i));
    }

    return Results.Accepted(null, chats);
});

app.MapPost("/sendMessage", (HttpContext context, Message msg, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }

    if (msg.Type != ContentType.Text)
    {
        return Results.BadRequest("Content type unsupported");
    }

    bool correct = false;
    
    foreach (var i in user[0].Chats)
    {
        if (i == msg.ChatId)
        {
            correct = true;
            break;
        }
    }

    if (!correct)
    {
        return Results.Unauthorized();
    }

    var chat = db.Chats.Find(msg.ChatId);
    if (chat.Messages == null)
    {
        chat.Messages = new List<Message>();
        chat?.Messages.Add(msg);
    }
    else
    {
        chat?.Messages.Add(msg);
    }

    db.SaveChanges();

    return Results.Accepted(null, msg);
});

app.MapPost("/loadMessages", (HttpContext context, Chat chat, AppDbContext db) =>
{
    var key = context.Request.Headers["Authorization"].ToString();
    if (key == "")
    {
        return Results.BadRequest();
    }

    using SHA256 sha256Hash = SHA256.Create();
    string hash = GetHash(sha256Hash, key + publicKey);
    var user = (from u in db.Users where u.Secret == hash select u).ToList();
    if (!user.Any())
    {
        return Results.Unauthorized();
    }
    
    foreach (var i in user[0].Chats)
    {
        if (i == chat.Id)
        {
            break;
        }
        return Results.Unauthorized();
    }

    db.Entry(chat).Collection(u => u.Messages).Load();
    return Results.Accepted(null, chat.Messages);
});


app.Run();


string GetHash(HashAlgorithm hashAlgorithm, string input)
{
    // Convert the input string to a byte array and compute the hash.
    byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

    // Create a new Stringbuilder to collect the bytes
    // and create a string.
    var sBuilder = new StringBuilder();

    // Loop through each byte of the hashed data
    // and format each one as a hexadecimal string.
    for (int i = 0; i < data.Length; i++)
    {
        sBuilder.Append(data[i].ToString("x2"));
    }

    // Return the hexadecimal string.
    return sBuilder.ToString();
}