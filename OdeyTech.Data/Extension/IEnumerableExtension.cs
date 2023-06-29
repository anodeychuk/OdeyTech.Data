// --------------------------------------------------------------------------
// <copyright file="IEnumerableExtension.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using OdeyTech.Data.Model.Interface;
using OdeyTech.ProductivityKit;
using OdeyTech.ProductivityKit.Extension;

namespace OdeyTech.Data.Extension
{
    /// <summary>
    /// Provides extension methods for working with IEnumerable collections.
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Clones the elements of the given collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to clone.</param>
        /// <returns>A new collection with cloned elements.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="collection"/> is null or empty.</exception>
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> collection) where T : IModel
        {
            ThrowHelper.ThrowIfNullOrEmpty(collection, nameof(collection));
            var newCollection = new HashSet<T>(collection.Count());
            collection.ForEach(p => newCollection.Add((T)p.Clone()));
            return newCollection;
        }
    }
}
