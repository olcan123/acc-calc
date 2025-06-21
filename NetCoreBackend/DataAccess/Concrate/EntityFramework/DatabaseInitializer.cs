using Microsoft.EntityFrameworkCore;
using DataAccess.Concrate.EntityFramework.Context;

namespace DataAccess.Concrate.EntityFramework
{
    public static class DatabaseInitializer
    {        public static void InitializeDatabase()
        {
            try
            {
                using (var context = new FcdAccContext())
                {
                    // Ensure database is created
                    context.Database.EnsureCreated();
                    Console.WriteLine("Database ensured successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw; // Re-throw the exception so the application startup fails if database setup fails
            }
        }
    }
}
