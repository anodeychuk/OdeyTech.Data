// --------------------------------------------------------------------------
// <copyright file="ObservableCollectionExtension.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Extension
{
    public static class ObservableCollectionExtension
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            var sorted = collection.OrderBy(x => x).ToList();
            for (var i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        public static ObservableCollection<T> Copy<T>(this ObservableCollection<T> collection) where T : IModel
        {
            ObservableCollection<T> tree = new();
            foreach (T item in collection)
            {
                tree.Add((T)item.Clone());
            }

            return tree;
        }

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
    }
}
