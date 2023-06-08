// --------------------------------------------------------------------------
// <copyright file="SqlRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Data;
using OdeyTech.SqlProvider.Entity.Database.Checker;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;

namespace OdeyTech.Data.Repository
{
    /// <summary>
    /// Represents a base class for repositories that work with SQL databases.
    /// </summary>
    public abstract class SqlRepository : IDisposable
    {
        protected readonly IDbConnection dbConnection;
        protected SqlExecutor sqlExecutor;
        protected IDbChecker dbChecker;
        protected SqlTable tableTemplate;
        private bool isDisposed;

        /// <summary>
        /// Gets the SQL executor for this repository.
        /// </summary>
        protected SqlExecutor SqlExecutor
        {
            get
            {
                this.sqlExecutor ??= new SqlExecutor(this.dbConnection);
                return this.sqlExecutor;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlRepository"/> class with the specified database connection and checker.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbChecker">The database checker to verify the existence of a database or a database item.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either <paramref name="dbConnection"/> or <paramref name="dbChecker"/> is null.
        /// </exception>
        public SqlRepository(IDbConnection dbConnection, IDbChecker dbChecker)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

            this.tableTemplate = new SqlTable();

            this.dbChecker = dbChecker ?? throw new ArgumentNullException(nameof(dbChecker));
            this.dbChecker.DbConnection = this.dbConnection;
            this.dbChecker.SqlExecutor = SqlExecutor;
            this.dbChecker.DatabaseItemSource = this.tableTemplate;
        }

        /// <summary>
        /// Releases all resources used by this repository.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Checks the database and creates the database item if it does not exist.
        /// </summary>
        public void CheckDatabase() => this.dbChecker.CheckDatabase();

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
                this.sqlExecutor?.Dispose();
                this.dbConnection?.Close();
                this.dbConnection?.Dispose();
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Gets the query source for the current repository.
        /// </summary>
        /// <returns>A <see cref="SqlTable"/> object representing the query source for the repository.</returns>
        protected SqlTable GetTableTemplate() => (SqlTable)this.tableTemplate.Clone();
    }
}
