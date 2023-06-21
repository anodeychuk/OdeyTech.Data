// --------------------------------------------------------------------------
// <copyright file="ReadableRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Repository.Interface;
using OdeyTech.ProductivityKit;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;

namespace OdeyTech.Data.Repository
{
    /// <summary>
    /// Provides functionality for reading data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public abstract class ReadableRepository<T> : SqlRepository, IReadableRepository<T> where T : IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadableRepository{T}"/> class with the specified database connection.
        /// </summary>
        /// <param name="databaseType">The type of the database.</param>
        /// <param name="dbConnection">The database connection.</param>
        public ReadableRepository(DatabaseType databaseType, IDbConnection dbConnection) : base(databaseType, dbConnection)
        { }

        /// <inheritdoc/>
        public IEnumerable<T> Select()
        {
            SqlTable tableSource = SetSelectQueryExtension(GetTableTemplate());
            return Select(tableSource);
        }

        /// <inheritdoc/>
        public T SelectByIdentifier(ulong identifier) => SelectByCondition(GetSelectIdCondition(identifier)).FirstOrDefault();

        /// <summary>
        /// Selects items by the specified conditions.
        /// </summary>
        /// <param name="conditions">An array of conditions to apply.</param>
        /// <returns>An enumerable collection of items that match the specified conditions.</returns>
        protected IEnumerable<T> SelectByCondition(params string[] conditions)
        {
            SqlTable tableSource = SetSelectQueryExtension(GetTableTemplate());
            tableSource.AddConditions(conditions);
            return Select(tableSource);
        }

        /// <summary>
        /// Returns a condition for selecting an item by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the item.</param>
        /// <returns>An array containing the condition for selecting an item by its identifier.</returns>
        protected virtual string[] GetSelectIdCondition(ulong id) => new[] { $"{nameof(IModel.Identifier)} = {id}" };

        /// <summary>
        /// Sets an extension for the select query.
        /// </summary>
        /// <param name="tableSource">The query source to extend.</param>
        /// <returns>The extended query source.</returns>
        protected virtual SqlTable SetSelectQueryExtension(SqlTable tableSource) => tableSource;

        /// <summary>
        /// Gets an item from the specified data row.
        /// </summary>
        /// <param name="row">The data row containing the item.</param>
        /// <returns>The item retrieved from the data row.</returns>
        protected virtual T GetItem(DataRow row)
        {
            var identifier = (ulong)Convert.ToInt64(row[nameof(IModel.Identifier)]);
            return Accessor.CreateInstance<T>(identifier);
        }

        /// <summary>
        /// Selects items using the specified query source.
        /// </summary>
        /// <param name="tableSource">The query source to use for selection.</param>
        /// <returns>A list of items that match the specified query source.</returns>
        private List<T> Select(SqlTable tableSource)
        {
            DataTable data = SqlExecutor.Select(GetSelectQuery(tableSource));
            var result = new List<T>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(GetItem(row));
            }

            return result;
        }

        private string GetSelectQuery(SqlTable tableSource) => SqlQueryGenerator.Select(tableSource);
    }
}
