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
using OdeyTech.SqlProvider.Entity.Database.Checker;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Entity.Table.Column;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Repository
{
    /// <summary>
    /// Abstract repository class for managing dependent data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public abstract class DependentModelRepository<T> : ModelRepository<T>, IDependentModelRepository<T>
      where T : IDependentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependentModelRepository{T}"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbChecker">The database checker to verify the existence of a database or a database item.</param>
        protected DependentModelRepository(IDbConnection dbConnection, IDbChecker dbChecker) : base(dbConnection, dbChecker)
        { }

        /// <summary>
        /// Gets the name of the column in the data source that represents the parent dependency.
        /// </summary>
        protected abstract string DependenceColumnName { get; }

        /// <inheritdoc/>
        public IEnumerable<T> SelectByParent(IModel parent) => SelectByParent(parent.Identifier);

        /// <inheritdoc/>
        public IEnumerable<T> SelectByParent(ulong parentId) => SelectByCondition($"{DependenceColumnName} = {parentId}");

        /// <summary>
        /// Deletes all data items with the specified parent from the repository.
        /// </summary>
        /// <param name="parent">The parent item.</param>
        public virtual void DeleteByParent(IModel parent)
        {
            if (parent == null)
            {
                return;
            }

            SqlTable tableSource = GetTableTemplate();
            tableSource.AddConditions($"{DependenceColumnName} = {parent.Identifier}");
            SqlExecutor.Query(SqlQueryGenerator.Delete(tableSource));
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
    }
}
