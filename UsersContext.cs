using MediaManageAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediaManageAPI;
public class UsersContext : IdentityUserContext<ApplicationUser>
{
    private readonly IConfiguration _config;
    public UsersContext(DbContextOptions<UsersContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_config["connectionString"]!);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}