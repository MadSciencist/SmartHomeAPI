using System;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Core.Entities.Abstractions;

namespace SmartHome.Core.DataAccess
{
    public class EntityFrameworkTransaction : IDatabaseTransaction
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
