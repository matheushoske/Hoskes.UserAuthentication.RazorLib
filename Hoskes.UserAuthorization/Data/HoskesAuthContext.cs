using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hoskes.Account.Core;

    public class HoskesAuthContext : DbContext
    {   
        public HoskesAuthContext(DbContextOptions<HoskesAuthContext> options)
            : base(options)
        {
        }

        public DbSet<Hoskes.Account.Core.User> User { get; set; } = default!;
    }
