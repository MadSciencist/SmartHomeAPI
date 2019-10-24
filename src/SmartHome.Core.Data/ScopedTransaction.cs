using Matty.Framework.Abstractions;
using System;
using System.Transactions;

namespace SmartHome.Core.Data
{
    public class ScopedTransaction : ITransaction
    {
        public Guid Identifier => Guid.NewGuid();

        private TransactionScope _scope;

        public ScopedTransaction()
        {
            //_transaction = context.Database.BeginTransaction();
            _scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Commit()
        {
            _scope.Complete();
        }

        public void Rollback()
        {
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
                if (_scope != null)
                {
                    _scope.Dispose();
                    _scope = null;
                }
            }
        }
    }
}