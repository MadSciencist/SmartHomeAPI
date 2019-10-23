using Matty.Framework.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Transactions;

namespace SmartHome.Core.Data
{
    public class EntityFrameworkTransaction : ITransaction
    {
        public Guid Identifier => _transaction.TransactionId;

        private IDbContextTransaction _transaction;
        private TransactionScope _scope;

        public EntityFrameworkTransaction()
        {
            //_transaction = context.Database.BeginTransaction();
            _scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Commit()
        {
            //_transaction.Commit();
            _scope.Complete();
        }

        public void Rollback()
        {
            //_transaction.Rollback();
            _scope.Dispose();
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
                //if (_transaction != null)
                //{
                //    _transaction.Dispose();
                //    _transaction = null;
                //}

                if (_scope != null)
                {
                    _scope.Dispose();
                    _scope = null;
                }
            }
        }
    }
}
