// --------------------------------------------------------------------------
// <copyright file="IDependentModelRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Repository.Interface
{
    /// <summary>
    /// Repository that provides read and write access to data items of type <typeparamref name="T"/> that have a parent dependency.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public interface IDependentModelRepository<T> : IRepository<T> where T : IDependentModel
    {
        /// <summary>
        /// Deletes all the data items that have the specified parent from the repository.
        /// </summary>
        /// <param name="parent">The parent item.</param>
        void DeleteByParent(IModel parent);

        /// <summary>
        /// Selects all the data items that have the specified parent from the repository.
        /// </summary>
        /// <param name="parent">The parent item.</param>
        /// <returns>An enumerable collection of the selected data items.</returns>
        IEnumerable<T> SelectByParent(IModel parent);

        /// <summary>
        /// Selects all the data items that have the parent with the specified identifier from the repository.
        /// </summary>
        /// <param name="parentId">The identifier of the parent item.</param>
        /// <returns>An enumerable collection of the selected data items.</returns>
        IEnumerable<T> SelectByParent(ulong parentId);
    }
}
