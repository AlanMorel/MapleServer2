using Maple2.Sql.Model;
using Microsoft.EntityFrameworkCore;

namespace Maple2.Sql.Context {
    public interface IItemAccessor {
        public DbSet<Item> Item { get; set; }
    }
}