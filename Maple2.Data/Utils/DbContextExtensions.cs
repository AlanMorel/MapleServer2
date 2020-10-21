using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Maple2.Data.Utils {
    public static class DbContextExtensions {
        public static bool TrySaveChanges(this DbContext context, bool autoAccept = true) {
            try {
                Console.WriteLine($"> Begin Save... {context.ContextId}");
                DisplayStates(context.ChangeTracker.Entries());
                context.SaveChanges(autoAccept);
                Console.WriteLine($"> Completed {context.ContextId}");
                return true;
            } catch (Exception ex) {
                Console.WriteLine($"> Failed {context.ContextId}");
                Console.WriteLine(ex);
                return false;
            }
        }

        private static void DisplayStates(IEnumerable<EntityEntry> entries) {
            foreach (EntityEntry entry in entries) {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()} ");
            }
        }
    }
}