// --------------------------------------------------------------------------
// <copyright file="SqlRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Data;
using OdeyTech.ProductivityKit;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Database.Checker;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Repository
{
    /// <summary>
    /// Provides a base class for repositories that work with SQL databases.
    /// </summary>
    public abstract class SqlRepository : IDisposable
    {
        protected readonly IDbConnection dbConnection;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlRepository"/> class with the specified database connection and checker.
        /// </summary>
        /// <param name="databaseType">The type of the database.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dbConnection"/> is null.</exception>
        public SqlRepository(DatabaseType databaseType, IDbConnection dbConnection)
        {
            ThrowHelper.ThrowIfNull(dbConnection, nameof(dbConnection));

            this.dbConnection = dbConnection;
            TableTemplate = new SqlTable();

            DbChecker = DbCheckerFactory.GetDbChecker(databaseType, dbConnection);
            DbChecker.DatabaseItemSource = TableTemplate;

            SqlExecutor = new SqlExecutor(dbConnection);
            SqlQueryGenerator = QueryGenerationFactory.GetGenerator(databaseType);
        }

        /// <summary>
        /// Checks the database and creates the database item if it does not exist.
        /// </summary>
        public void CheckDatabase() => DbChecker.CheckDatabase();

        /// <summary>
        /// Releases all resources used by this repository.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected IDbChecker DbChecker { get; set; }

        protected ISqlExecutor SqlExecutor { get; set; }

        protected SqlTable TableTemplate { get; set; }

        protected ISqlQueryGenerator SqlQueryGenerator { get; set; }

        /// <summary>
        /// Releases all resources used by this repository.
        /// </summary>
        /// <param name="disposing">true if called from the <see cref="Dispose"/> method, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.dbConnection.Dispose();
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Gets the query source for the current repository.
        /// </summary>
        /// <returns>A <see cref="SqlTable"/> object representing the query source for the repository.</returns>
        protected SqlTable GetTableTemplate() => (SqlTable)TableTemplate.Clone();
    }
}
