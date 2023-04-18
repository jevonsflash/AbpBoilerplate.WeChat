using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using WeChatPaySample.Authorization.Roles;
using WeChatPaySample.Authorization.Users;
using WeChatPaySample.MultiTenancy;
using WeChatPaySample.Common;

namespace WeChatPaySample.EntityFrameworkCore
{
    public class WeChatPaySampleDbContext : AbpZeroDbContext<Tenant, Role, User, WeChatPaySampleDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Order.Order> Order { get; set; }

        public WeChatPaySampleDbContext(DbContextOptions<WeChatPaySampleDbContext> options)
            : base(options)
        {
        }
    }
}
