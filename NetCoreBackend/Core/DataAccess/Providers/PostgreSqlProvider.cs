using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Text;

namespace FcdAcc.Core.DataAccess.Providers
{
    public class PostgreSqlProvider : IDatabaseProvider
    {
        public string ProviderName => "PostgreSQL";
        
        public bool SupportsMerge => false; // PostgreSQL doesn't support MERGE, use INSERT ... ON CONFLICT

        public void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });
        }

        public string ConvertColumnName(string columnName)
        {
            // Convert PascalCase to snake_case for PostgreSQL
            if (string.IsNullOrEmpty(columnName))
                return columnName;

            var builder = new StringBuilder();
            for (int i = 0; i < columnName.Length; i++)
            {
                if (i > 0 && char.IsUpper(columnName[i]))
                {
                    builder.Append('_');
                }
                builder.Append(char.ToLower(columnName[i]));
            }
            return builder.ToString();
        }

        public string ConvertTableName(string tableName)
        {
            return ConvertColumnName(tableName); // Same logic for table names
        }

        public string GetMergeStatement(string tableName, string[] keyColumns, string[] updateColumns)
        {
            // PostgreSQL uses INSERT ... ON CONFLICT instead of MERGE
            var keyColumnsList = string.Join(", ", keyColumns);
            var updateColumnsList = string.Join(", ", updateColumns.Select(col => $"{col} = EXCLUDED.{col}"));
            
            return $@"
                INSERT INTO {tableName} ({string.Join(", ", keyColumns.Concat(updateColumns))})
                VALUES ({string.Join(", ", keyColumns.Concat(updateColumns).Select(c => $"@{c}"))})
                ON CONFLICT ({keyColumnsList})
                DO UPDATE SET {updateColumnsList}";
        }
    }
}
