using Matty.Framework.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace SmartHome.Core.DataAccess
{
    public class EntityFrameworkTransaction : ITransaction
    {
        public Guid Identifier => _transaction.TransactionId;

        private readonly IDbContextTransaction _transaction;

        public EntityFrameworkTransaction(EntityFrameworkContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
