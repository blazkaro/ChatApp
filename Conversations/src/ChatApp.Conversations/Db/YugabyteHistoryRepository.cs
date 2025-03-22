using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace ChatApp.Conversations.Db;

public class YugabyteHistoryRepository : NpgsqlHistoryRepository
{
    public YugabyteHistoryRepository(HistoryRepositoryDependencies dependencies)
        : base(dependencies)
    {
    }

    private sealed class YugabyteMigrationsDatabaseLock(IHistoryRepository historyRepository) : IMigrationsDatabaseLock
    {
        public IHistoryRepository HistoryRepository => historyRepository;

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
            => default;
    }

    // Yugabyte doesn't currently support ACCESS LOCK, so we have to get rid of it

    public override Task<IMigrationsDatabaseLock> AcquireDatabaseLockAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IMigrationsDatabaseLock>(new YugabyteMigrationsDatabaseLock(this));
    }

    public override IMigrationsDatabaseLock AcquireDatabaseLock()
    {
        return new YugabyteMigrationsDatabaseLock(this);
    }
}
