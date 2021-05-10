﻿using System.Data;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;

namespace ToroInvestimentos.Backend.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        IDbConnection IUnitOfWork.DbConnection => _dbConnection;
        IDbTransaction IUnitOfWork.DbTransaction => _dbTransaction;

        public void Begin()
        {
            if (_dbConnection.State == ConnectionState.Closed) _dbConnection.Open();
            
            if (_dbTransaction != null) return;
            
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            _dbTransaction = null;
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }

        public void Commit()
        {
            if (_dbTransaction == null) return;

            try
            {
                _dbTransaction.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }
    }
}