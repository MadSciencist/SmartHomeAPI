using Matty.Framework.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace SmartHome.Core.Data
{
    public class EntityFrameworkTransaction : ITransaction
    {
        public Guid Identifier => _transaction.TransactionId;

        private IDbContextTransaction _transaction;

        public EntityFrameworkTransaction(EntityFrameworkContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }
    }
}
