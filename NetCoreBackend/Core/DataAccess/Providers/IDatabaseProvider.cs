using Microsoft.EntityFrameworkCore;

namespace FcdAcc.Core.DataAccess.Providers
{
    public interface IDatabaseProvider
    {
        string ProviderName { get; }
        void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString);
        string ConvertColumnName(string columnName);
        string ConvertTableName(string tableName);
        bool SupportsMerge { get; }
        string GetMergeStatement(string tableName, string[] keyColumns, string[] updateColumns);
    }
}
