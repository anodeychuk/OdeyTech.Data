// --------------------------------------------------------------------------
// <copyright file="IRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

namespace OdeyTech.Data.Repository.Interface
{
    /// <summary>
    /// Repository that provides read and write access to data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public interface IRepository<T> : IReadableRepository<T>
    {
        /// <summary>
        /// Inserts a new data item into the repository.
        /// </summary>
        /// <param name="obj">The data item to insert.</param>
        void Insert(T obj);

        /// <summary>
        /// Deletes the specified data item from the repository.
        /// </summary>
        /// <param name="obj">The data item to delete.</param>
        void Delete(T obj);

        /// <summary>
        /// Updates the specified data item in the repository.
        /// </summary>
        /// <param name="obj">The data item to update.</param>
        void Update(T obj);
    }
}
