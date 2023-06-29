using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                var readerRoleId = "2a9ed249-cb34-4f5c-93b5-f1374167a3d5";
                var writerRoleId = "cb72b5df-eacc-4e39-90e2-3511b3e275f8";
                base.OnModelCreating(builder);
                //We 'll seed some data for roles
                var roles = new List<IdentityRole>
                    {
                        new IdentityRole
                        {
                            Id=writerRoleId,
                            ConcurrencyStamp=writerRoleId,
                            Name="Writer",
                            NormalizedName="Writer".ToUpper()
                        },

                        new IdentityRole
                        {
                            Id=readerRoleId,
                            ConcurrencyStamp=readerRoleId,
                            Name="Reader",
                            NormalizedName="Reader".ToUpper()
                        }
                    };
                builder.Entity<IdentityRole>().HasData(roles);

            }

        
    }
}
