// --------------------------------------------------------------------------
// <copyright file="DataProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Repository.Interface;

namespace OdeyTech.Data.Provider
{
    /// <summary>
    /// Data provider for managing data items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data items.</typeparam>
    public abstract class DataProvider<T> : LoaderProvider<T> where T : IModel
    {
        private readonly IRepository<T> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider{T}"/> class, checks the database and loads data.
        /// </summary>
        /// <param name="repository">The repository to be used for data management.</param>
        /// <exception cref="InvalidOperationException">Thrown when there is an error checking the database or loading data.</exception>
        public DataProvider(IRepository<T> repository) : base()
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            try
            {
                this.repository.CheckDatabase();
                Load();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while checking the database or loading data.", ex);
            }
        }

        /// <summary>
        /// Disposes the resources used by the data provider.
        /// </summary>
        protected override void DoDispose()
        {
            base.DoDispose();
            this.repository.Dispose();
        }

        /// <summary>
        /// Retrieves the raw data from the repository.
        /// </summary>
        /// <returns>A collection of data items from the repository.</returns>
        protected override IEnumerable<T> RawData => this.repository.Select();

        /// <summary>
        /// Adds a new data item to the provider and the repository.
        /// </summary>
        /// <param name="toAdd">The data item to add.</param>
        /// <exception cref="ArgumentException">Thrown when there is an error adding the item to the repository.</exception>
        public override void Add(T toAdd)
        {
            try
            {
                base.Add(toAdd);
                this.repository.Insert(toAdd);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error while adding item with Identifier {toAdd.Identifier}.", ex);
            }
        }

        /// <summary>
        /// Updates the specified data item in the provider and the repository.
        /// </summary>
        /// <param name="toEdit">The data item to update.</param>
        /// <exception cref="ArgumentException">Thrown when there is an error updating the item in the repository.</exception>
        public override void EndEdit(T toEdit)
        {
            try
            {
                base.EndEdit(toEdit);
                this.repository.Update(toEdit);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error while updating item with Identifier {toEdit.Identifier}.", ex);
            }
        }

        /// <summary>
        /// Removes the specified data item from the provider and the repository.
        /// </summary>
        /// <param name="toRemove">The data item to remove.</param>
        /// <exception cref="ArgumentException">Thrown when there is an error removing the item from the repository.</exception>
        public override void Remove(T toRemove)
        {
            try
            {
                base.Remove(toRemove);
                this.repository.Delete(toRemove);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error while removing item with Identifier {toRemove.Identifier}.", ex);
            }
        }
    }
}
