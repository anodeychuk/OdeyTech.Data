// --------------------------------------------------------------------------
// <copyright file="DependenceModelRepository.cs" author="Andrii Odeychuk">
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
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Entity.Table.Column;

namespace OdeyTech.Data.Repository
{
    /// <summary>
    /// Provides functionality for managing dependent data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public abstract class DependentModelRepository<T> : ModelRepository<T>, IDependentModelRepository<T>
      where T : IDependentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependentModelRepository{T}"/> class.
        /// </summary>
        /// <param name="databaseType">The type of the database.</param>
        /// <param name="dbConnection">The database connection.</param>
        protected DependentModelRepository(DatabaseType databaseType, IDbConnection dbConnection) : base(databaseType, dbConnection)
        { }

        /// <summary>
        /// Gets the name of the column in the data source that represents the parent dependency.
        /// </summary>
        protected abstract string DependenceColumnName { get; }

        /// <inheritdoc/>
        public IEnumerable<T> SelectByParent(IModel parent)
        {
            CheckNullParent(parent);
            return SelectByParent(parent.Identifier);
        }

        /// <inheritdoc/>
        public IEnumerable<T> SelectByParent(ulong parentId) => SelectByCondition($"{DependenceColumnName} = {parentId}");

        /// <summary>
        /// Deletes all data items with the specified parent from the repository.
        /// </summary>
        /// <param name="parent">The parent item.</param>
        public virtual void DeleteByParent(IModel parent)
        {
            CheckNullParent(parent);
            SqlTable tableSource = GetTableTemplate();
            tableSource.AddConditions($"{DependenceColumnName} = {parent.Identifier}");
            SqlExecutor.Query(GetDeleteQuery(tableSource));
        }

        /// <summary>
        /// Gets the column values for a given data item.
        /// </summary>
        /// <param name="item">The data item.</param>
        /// <returns>A <see cref="SqlColumns"/> object containing the columns.</returns>
        protected override void SaveItem(SqlTable tableSource, T item)
        {
            base.SaveItem(tableSource, item);
            tableSource.Columns.SetValue(DependenceColumnName, item.ParentIdentifier);
        }

        /// <summary>
        /// Creates a data item from a given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The data item.</returns>
        protected override T GetItem(DataRow row)
        {
            T obj = base.GetItem(row);
            obj.ParentIdentifier = (ulong)Convert.ToInt64(row[DependenceColumnName]);
            return obj;
        }

        private string GetDeleteQuery(SqlTable tableSource) => SqlQueryGenerator.Delete(tableSource);

        private void CheckNullParent(IModel parent)
        {
            if (parent is null)
            {
                throw new ArgumentNullException(nameof(parent));
            }
        }
    }
}
