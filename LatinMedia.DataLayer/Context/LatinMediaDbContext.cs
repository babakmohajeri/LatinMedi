using LatinMedia.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.DataLayer.Context
{
    public class LatinMediaDbContext:DbContext
    {
        public LatinMediaDbContext(DbContextOptions<LatinMediaDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
    }
}
