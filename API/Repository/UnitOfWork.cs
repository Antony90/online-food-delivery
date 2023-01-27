using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using OnlineFoodDelivery.Data;

namespace OnlineFoodDelivery.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDBContext _context;
        private bool disposed = false;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
        }

        public async void Save()
        {
            await _context.SaveChangesAsync();
        }

        protected void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    _context.Dispose();
                }
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