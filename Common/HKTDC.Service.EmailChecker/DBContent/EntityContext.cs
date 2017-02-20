using HKTDC.WebAPI.EmailChecker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.WebAPI.EmailChecker.DBContent
{
    class EntityContext : DbContext
    {
        public virtual DbSet<AppSetting> AppSetting { get; set; }
    }
}
