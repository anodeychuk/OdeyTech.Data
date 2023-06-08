// --------------------------------------------------------------------------
// <copyright file="IBatchRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;

namespace OdeyTech.Data.Repository.Interface
{
    /// <summary>
    /// Repository that provides batch read and write access to data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public interface IBatchRepository<T> : IRepository<T>
    {
        /// <summary>
        /// Inserts a batch of new data items into the repository.
        /// </summary>
        /// <param name="items">The batch of data items to insert.</param>
        void InsertBatch(IEnumerable<T> items);

        /// <summary>
        /// Updates a batch of data items in the repository.
        /// </summary>
        /// <param name="items">The batch of data items to update.</param>
        void UpdateBatch(IEnumerable<T> items);

        /// <summary>
        /// Deletes a batch of data items from the repository.
        /// </summary>
        /// <param name="items">The batch of data items to delete.</param>
        void DeleteBatch(IEnumerable<T> items);
    }
}
