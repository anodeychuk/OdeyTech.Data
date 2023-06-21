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
        public void Insert(T item)
        {
            if (item is null)
            {
                return;
            }

            PrepareInsert(item);
            SqlExecutor.Query(GetInsertQuery(item));
            InsertInternal(item);
        }

        /// <inheritdoc/>
        public void Update(T item)
        {
            if (item is null)
            {
                return;
            }

            PrepareUpdate(item);
            SqlExecutor.Query(GetUpdateQuery(item));
            UpdateInternal(item);
        }

        /// <inheritdoc/>
        public void Delete(T item)
        {
            if (item is null)
            {
                return;
            }

            PrepareDelete(item);
            DeleteInternal(item);
            SqlExecutor.Query(GetDeleteQuery(item));
        }

        /// <summary>
        /// Gets the column values for the specified data item.
        /// </summary>
        /// <param name="item">The data item.</param>
        /// <returns>A <see cref="ColumnValues"/> object containing the column values for the data item.</returns>
        protected virtual void SaveItem(SqlTable table, T item)
        {
            if (table.Columns.GetColumn(nameof(IModel.Identifier)).IsExcluded)
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
            tableSource.Columns.GetColumn(nameof(IModel.Identifier)).IsExcluded = true;
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
