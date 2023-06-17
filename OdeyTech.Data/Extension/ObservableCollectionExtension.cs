// --------------------------------------------------------------------------
// <copyright file="ObservableCollectionExtension.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Extension
{
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// Sorts the elements of the given collection in ascending order.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            var sorted = SortCollection(collection);
            RearrangeCollection(collection, sorted);
        }

        /// <summary>
        /// Creates a copy of the given collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to copy.</param>
        /// <returns>A new collection with copied elements.</returns>
        public static ObservableCollection<T> Copy<T>(this ObservableCollection<T> collection) where T : IModel
        {
            ObservableCollection<T> newCollection = CreateNewCollection<T>();
            PopulateCollectionWithClones(collection, newCollection);
            return newCollection;
        }

        /// <summary>
        /// Finds the element in the given collection that has the closest identifier to the given identifier.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to search.</param>
        /// <param name="identifier">The identifier to compare with.</param>
        /// <returns>The element with the closest identifier, or the default value of T if the collection is empty.</returns>
        public static T Neighbor<T>(this ObservableCollection<T> collection, ulong identifier) where T : IModel
        {
            T value = default;
            var minDistance = ulong.MaxValue;
            foreach (T item in collection)
            {
                var distance = item.Identifier < identifier ? identifier - item.Identifier : item.Identifier - identifier;
                if (minDistance > distance)
                {
                    minDistance = distance;
                    value = item;
                }
            }

            return value;
        }

        private static List<T> SortCollection<T>(ObservableCollection<T> collection) where T : IComparable => collection.OrderBy(x => x).ToList();

        private static void RearrangeCollection<T>(ObservableCollection<T> collection, List<T> sorted) where T : IComparable
        {
            for (var i = 0; i < sorted.Count; i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        private static ObservableCollection<T> CreateNewCollection<T>() where T : IModel => new();

        private static void PopulateCollectionWithClones<T>(ObservableCollection<T> original, ObservableCollection<T> newCollection) where T : IModel
        {
            foreach (T item in original)
            {
                newCollection.Add((T)item.Clone());
            }
        }
    }
}
