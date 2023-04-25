namespace AirFinder.Infra.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public Context Context { get; set; }

        public UnitOfWork(Context context)
        {
            Context = context;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                Context.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

