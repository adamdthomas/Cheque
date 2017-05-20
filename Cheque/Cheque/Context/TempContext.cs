using HouseFly.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HouseFly.Context
{
    public class TempContext : DbContext 
    {
        public DbSet<TempModels> TempModels { get; set; }
    }
}