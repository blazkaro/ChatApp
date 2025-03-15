using Npgsql;

namespace ChatApp.Messages.Db;

public static class DbMigrator
{
    public static async Task MigrateDbIfNotExists(this WebApplication app)
    {
        var dataSource = app.Services.GetRequiredService<NpgsqlDataSource>();
        var cmd = dataSource.CreateCommand(@"CREATE TABLE IF NOT EXISTS Messages(
                                    Id uuid,
                                    Content varchar(65536),
                                    SenderId varchar(255),
                                    ConversationId uuid,
                                    SentAt timestamptz,
                                    PRIMARY KEY(Id)
                                    );");

        await cmd.ExecuteNonQueryAsync();
    }
}
