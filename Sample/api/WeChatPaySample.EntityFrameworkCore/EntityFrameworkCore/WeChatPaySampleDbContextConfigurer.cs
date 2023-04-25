using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace WeChatPaySample.EntityFrameworkCore
{
    public static class WeChatPaySampleDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WeChatPaySampleDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WeChatPaySampleDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
