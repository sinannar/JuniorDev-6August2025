using System;
using Microsoft.EntityFrameworkCore;

namespace dotnetwebapi;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public virtual DbSet<TestData> TestDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
