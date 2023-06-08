// --------------------------------------------------------------------------
// <copyright file="IDataProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Provider
{
    /// <summary>
    /// Data provider that can provide and manipulate a collection of data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public interface IDataProvider<T> : IDisposable where T : IModel
    {
        /// <summary>
        /// Occurs when the loading state of the provider changes.
        /// </summary>
        event PropertyChangedEventHandler LoadingChanged;

        /// <summary>
        /// Indicating whether the provider is currently loading data.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Data items provided by the provider.
        /// </summary>
        ObservableCollection<T> Items { get; set; }

        /// <summary>
        /// Creates a new instance of the data item.
        /// </summary>
        /// <returns>The new instance of the data item.</returns>
        T NewItem();

        /// <summary>
        /// Adds the specified data item to the collection of data items.
        /// </summary>
        /// <param name="toAdd">The data item to add.</param>
        void Add(T toAdd);

        /// <summary>
        /// Begins an edit session for the specified data item and returns a copy of the item for editing.
        /// </summary>
        /// <param name="toEdit">The data item to edit.</param>
        /// <returns>A copy of the data item for editing.</returns>
        T BeginEdit(T toEdit);

        /// <summary>
        /// Ends an edit session for the specified data item and updates the original item with the changes made to the copy.
        /// </summary>
        /// <param name="toEdit">The edited data item.</param>
        void EndEdit(T toEdit);

        /// <summary>
        /// Begins a data load operation.
        /// </summary>
        void BeginLoad();

        /// <summary>
        /// Loads the data into the provider.
        /// </summary>
        void Load();

        /// <summary>
        /// Ends a data load operation.
        /// </summary>
        void EndLoad();

        /// <summary>
        /// Removes the specified data item from the collection of data items.
        /// </summary>
        /// <param name="toRemove">The data item to remove.</param>
        void Remove(T toRemove);
    }
}
