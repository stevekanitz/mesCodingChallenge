using System.Text.Json;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var connectionString = "Data Source=config.db";

InitializeDatabase(connectionString);

//Search by filter
app.MapGet("/config", async (string? filter) =>
{
    using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var cmd = connection.CreateCommand();

    if (!string.IsNullOrWhiteSpace(filter))
    {
        cmd.CommandText = "SELECT Key, Value FROM Config WHERE Key LIKE $filter";
        cmd.Parameters.AddWithValue("$filter", $"%{filter}%");
    }
    else
    {
        cmd.CommandText = "SELECT Key, Value FROM Config";
    }

    var results = new List<object>();

    using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        var key = reader.GetString(0);
        var valueJson = reader.GetString(1);

        using var doc = JsonDocument.Parse(valueJson);

        results.Add(new
        {
            Key = key,
            Value = doc.RootElement.Clone()
        });
    }

    return Results.Ok(results);
});

//Get by key
app.MapGet("/config/{key}", async (string key) =>
{
    using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var cmd = connection.CreateCommand();
    cmd.CommandText = "SELECT Value FROM Config WHERE Key = $key";
    cmd.Parameters.AddWithValue("$key", key);

    var result = await cmd.ExecuteScalarAsync();
    if (result is null)
        return Results.NotFound();

    var valueJson = (string)result;
    using var doc = JsonDocument.Parse(valueJson);

    return Results.Ok(new
    {
        Key = key,
        Value = doc.RootElement.Clone()
    });
});

//Add Entry
app.MapPost("/config", async (ConfigItem item) =>
{
    using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var valueJson = item.Value.GetRawText();

    var cmd = connection.CreateCommand();
    cmd.CommandText = "INSERT INTO Config (Key, Value) VALUES ($key, $value)";
    cmd.Parameters.AddWithValue("$key", item.Key);
    cmd.Parameters.AddWithValue("$value", valueJson);

    try
    {
        await cmd.ExecuteNonQueryAsync();
        return Results.Created($"/config/{item.Key}", item);
    }
    catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
    {
        return Results.Conflict($"Key '{item.Key}' already exists.");
    }
});

//Update by key
app.MapPut("/config/{key}", async (string key, JsonElement value) =>
{
    using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var valueJson = value.GetRawText();

    var cmd = connection.CreateCommand();
    cmd.CommandText = "UPDATE Config SET Value = $value WHERE Key = $key";
    cmd.Parameters.AddWithValue("$key", key);
    cmd.Parameters.AddWithValue("$value", valueJson);

    var rows = await cmd.ExecuteNonQueryAsync();
    if (rows == 0)
        return Results.NotFound();

    return Results.Ok(new { Key = key, Value = value });
});

//Delete by key
app.MapDelete("/config/{key}", async (string key) =>
{
    using var connection = new SqliteConnection(connectionString);
    await connection.OpenAsync();

    var cmd = connection.CreateCommand();
    cmd.CommandText = "DELETE FROM Config WHERE Key = $key";
    cmd.Parameters.AddWithValue("$key", key);

    var rows = await cmd.ExecuteNonQueryAsync();
    if (rows == 0)
        return Results.NotFound();

    return Results.NoContent();
});

app.Run();

static void InitializeDatabase(string connectionString)
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var cmd = connection.CreateCommand();
    cmd.CommandText =
    @"
        CREATE TABLE IF NOT EXISTS Config (
            Key   TEXT PRIMARY KEY,
            Value TEXT NOT NULL
        );
    ";
    cmd.ExecuteNonQuery();
}

public record ConfigItem(string Key, JsonElement Value);

