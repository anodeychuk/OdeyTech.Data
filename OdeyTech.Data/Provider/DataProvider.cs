// --------------------------------------------------------------------------
// <copyright file="DataProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

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
        /// Initializes a new instance of the <see cref="DataProvider{T}"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used for data management.</param>
        public DataProvider(IRepository<T> repository) : base()
        {
            this.repository = repository;
            this.repository.CheckDatabase();
            Load();
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
        /// Gets the raw data from the repository.
        /// </summary>
        protected override IEnumerable<T> RawData => this.repository.Select();

        /// <summary>
        /// Adds a new data item to the provider and the repository.
        /// </summary>
        /// <param name="toAdd">The data item to add.</param>
        public override void Add(T toAdd)
        {
            base.Add(toAdd);
            this.repository.Insert(toAdd);
        }

        /// <summary>
        /// Updates the specified data item in the provider and the repository.
        /// </summary>
        /// <param name="toEdit">The data item to update.</param>
        public override void EndEdit(T toEdit)
        {
            base.EndEdit(toEdit);
            this.repository.Update(toEdit);
        }

        /// <summary>
        /// Removes the specified data item from the provider and the repository.
        /// </summary>
        /// <param name="toRemove">The data item to remove.</param>
        public override void Remove(T toRemove)
        {
            base.Remove(toRemove);
            this.repository.Delete(toRemove);
        }
    }
}
