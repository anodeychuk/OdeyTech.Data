// --------------------------------------------------------------------------
// <copyright file="RangeObservableCollection.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OdeyTech.Data.Model
{
    /// <summary>
    /// Represents an observable collection that supports adding a range of items.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool suppressNotification = false;

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!this.suppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Adds a range of items to the collection.
        /// </summary>
        /// <param name="list">The items to add to the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is null.</exception>
        public void AddRange(IEnumerable<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            this.suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }

            this.suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
