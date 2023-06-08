// --------------------------------------------------------------------------
// <copyright file="IReadableRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace OdeyTech.Data.Repository.Interface
{
    /// <summary>
    /// Repository that provides read-only access to data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public interface IReadableRepository<T> : IDisposable
    {
        /// <summary>
        /// Selects all items in the repository.
        /// </summary>
        /// <returns>An enumerable collection of items.</returns>
        IEnumerable<T> Select();

        /// <summary>
        /// Selects an item by its identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the item to select.</param>
        /// <returns>The item with the specified identifier, or null if no such item exists.</returns>
        T SelectByIdentifier(ulong identifier);

        /// <summary>
        /// Checks the database and creates the database item if it does not exist.
        /// </summary>
        void CheckDatabase();
    }
}
