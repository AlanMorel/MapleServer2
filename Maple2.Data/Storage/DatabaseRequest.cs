using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Maple2.Data.Storage {
    public abstract class DatabaseRequest<TContext> : IDisposable where TContext : DbContext {
        protected readonly TContext context;
        protected readonly ILogger logger;

        public DatabaseRequest(TContext context, ILogger logger) {
            this.context = context;
            this.logger = logger;
        }

        public TContext GetContext() {
            return context;
        }

        public void Commit() {
            try {
                logger.LogWarning("Saving changes:");
                DisplayStates(context.ChangeTracker.Entries());
                context.SaveChanges();
                logger.LogWarning("Saved...");
            } catch (Exception ex) {
                logger.LogCritical("Exception while saving", ex);
                throw;
            }
        }

        public void Dispose() => context.Dispose();

        private static void DisplayStates(IEnumerable<EntityEntry> entries) {
            foreach (EntityEntry entry in entries) {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()} ");
            }
        }
    }
}