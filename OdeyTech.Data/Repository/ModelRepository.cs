// --------------------------------------------------------------------------
// <copyright file="ModelRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Repository.Interface;
using OdeyTech.ProductivityKit;
using OdeyTech.ProductivityKit.Extension;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;

namespace OdeyTech.Data.Repository
{
    /// <summary>
    /// Provides functionality for managing data items of type <typeparamref name="T"/> with read, write, and batch operations.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public abstract class ModelRepository<T> : ReadableRepository<T>, IBatchRepository<T>, IRepository<T> where T : IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRepository{T}"/> class with the specified database connection.
        /// </summary>
        /// <param name="databaseType">The type of the database.</param>
        /// <param name="dbConnection">The database connection.</param>
        protected ModelRepository(DatabaseType databaseType, IDbConnection dbConnection) : base(databaseType, dbConnection)
        { }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is null.</exception>
        public void Insert(T item)
        {
            ThrowHelper.ThrowIfNull(item, nameof(item));

            PrepareInsert(item);
            SqlExecutor.Query(GetInsertQuery(item));
            InsertInternal(item);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is null.</exception>
        public void Update(T item)
        {
            ThrowHelper.ThrowIfNull(item, nameof(item));

            PrepareUpdate(item);
            SqlExecutor.Query(GetUpdateQuery(item));
            UpdateInternal(item);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="item"/> is null.</exception>
        public void Delete(T item)
        {
            ThrowHelper.ThrowIfNull(item, nameof(item));

            PrepareDelete(item);
            DeleteInternal(item);
            SqlExecutor.Query(GetDeleteQuery(item));
        }

        /// <summary>
        /// Saves the specified item to the specified SQL table.
        /// </summary>
        /// <param name="table">The SQL table to save the item to.</param>
        /// <param name="item">The item to be saved.</param>
        /// <remarks>
        /// This method sets the value of the "Identifier" column in the SQL table using the item's identifier.
        /// If the "Identifier" column is excluded from the table, no action is performed.
        /// </remarks>
        protected virtual void SaveItem(SqlTable table, T item)
        {
            if (table.Columns.Get(nameof(IModel.Identifier)).IsExcluded)
            {
                return;
            }

            table.Columns.SetValue(nameof(IModel.Identifier), item.Identifier);
        }

        /// <inheritdoc/>
        public void InsertBatch(IEnumerable<T> items) => ExecuteBatch(items, GetInsertQuery);

        /// <inheritdoc/>
        public void UpdateBatch(IEnumerable<T> items) => ExecuteBatch(items, GetUpdateQuery);

        /// <inheritdoc/>
        public void DeleteBatch(IEnumerable<T> items) => ExecuteBatch(items, GetDeleteQuery);

        private void ExecuteBatch(IEnumerable<T> items, Func<T, string> queryGenerator)
        {
            if (items.IsNullOrEmpty())
            {
                return;
            }

            var queues = new Queue<string>();
            items.ForEach(item => queues.Enqueue(queryGenerator(item)));
            SqlExecutor.Query(queues);
        }

        /// <summary>
        /// Prepares the data item for insertion.
        /// </summary>
        /// <param name="item">The data item.</param>
        protected virtual void PrepareInsert(T item) { }

        /// <summary>
        /// Prepares the data item for updating.
        /// </summary>
        /// <param name="item">The data item.</param>
        protected virtual void PrepareUpdate(T item) { }

        /// <summary>
        /// Prepares the data item for deletion.
        /// </summary>
        /// <param name="item">The data item.</param>
        protected virtual void PrepareDelete(T item) { }

        /// <summary>
        /// Performs additional actions after inserting a data item.
        /// </summary>
        /// <param name="item">The data item.</param>
        protected virtual void InsertInternal(T item) { }

        /// <summary>
        /// Performs additional actions after updating a data item.
        /// </summary>
        /// <param name="item">The data item.</param>
        protected virtual void UpdateInternal(T item) { }

        /// <summary>
        /// Performs additional actions after deleting a data item.
        /// </summary>
        /// <param name="item">The data item.</param>
        protected virtual void DeleteInternal(T item) { }

        private string GetInsertQuery(T item)
        {
            SqlTable tableSource = GetTableTemplate();
            SaveItem(tableSource, item);
            return SqlQueryGenerator.Insert(tableSource);
        }

        private string GetUpdateQuery(T item)
        {
            SqlTable tableSource = GetTableTemplate();
            tableSource.Columns.Get(nameof(IModel.Identifier)).IsExcluded = true;
            SaveItem(tableSource, item);
            tableSource.AddConditions($"{nameof(IModel.Identifier)} = {item.Identifier}");
            return SqlQueryGenerator.Update(tableSource);
        }

        private string GetDeleteQuery(T item)
        {
            SqlTable tableSource = GetTableTemplate();
            tableSource.AddConditions($"{nameof(IModel.Identifier)} = {item.Identifier}");
            return SqlQueryGenerator.Delete(tableSource);
        }
    }
}
