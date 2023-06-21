// --------------------------------------------------------------------------
// <copyright file="LoaderProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OdeyTech.Data.Model;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Provider.Interface;
using OdeyTech.ProductivityKit;

namespace OdeyTech.Data.Provider
{
    /// <summary>
    /// Base class for data loader providers that implement the <see cref="IDataProvider{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the data to provide.</typeparam>
    public abstract class LoaderProvider<T> : IDataProvider<T> where T : IModel
    {
        protected CancellationTokenSource cancellationTokenSource;
        private int loadingCounter;
        private Task loadingTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderProvider{T}"/> class with the specified next identifier.
        /// </summary>
        public LoaderProvider()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            Items = new();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler LoadingChanged;

        /// <inheritdoc/>
        public bool IsLoading => this.loadingCounter > 0;

        /// <inheritdoc/>
        public RangeObservableCollection<T> Items { get; private set; }

        /// <summary>
        /// Raw data to load from.
        /// </summary>
        protected virtual IEnumerable<T> RawData { get; }

        /// <summary>
        /// Indicating whether the provider has been disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public virtual void Add(T toAdd) => Items.Add(toAdd);

        /// <inheritdoc/>
        public virtual T BeginEdit(T toEdit)
            => Items.Contains(toEdit)
                ? (T)toEdit.Clone()
                : throw new ArgumentException("Provider does not contain the item you want to edit.");

        /// <inheritdoc/>
        public virtual void EndEdit(T toEdit)
        {
            if (!Items.Any(x => x.Identifier == toEdit.Identifier))
            {
                throw new ArgumentException("Provider does not contain the item you want to edit.");
            }

            Items.First(p => p.Identifier == toEdit.Identifier).CopyFrom(toEdit);
        }

        /// <inheritdoc/>
        public virtual T NewItem() => Accessor.CreateInstance<T>();

        /// <inheritdoc/>
        public virtual void Remove(T toRemove) => Items.Remove(toRemove);

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);
                IsDisposed = true;
            }
        }

        /// <inheritdoc/>
        public void BeginLoad()
        {
            if (Interlocked.Increment(ref this.loadingCounter) == 1)
            {
                OnLoadingChanged();
            }
        }

        /// <inheritdoc/>
        public void EndLoad()
        {
            if (Interlocked.Decrement(ref this.loadingCounter) == 0)
            {
                OnLoadingChanged();
            }
        }

        /// <inheritdoc/>
        public void Load()
        {
            BeginLoad();
            this.loadingTask = Task.Run(() =>
            {
                try
                {
                    Items.AddRange(LoadInternal());
                }
                catch (OperationCanceledException)
                {
                    // Do nothing if the operation was cancelled.
                }
                finally
                {
                    EndLoad();
                    lock (this.cancellationTokenSource)
                    {
                        this.loadingTask = null;
                    }
                }
            });
        }

        /// <summary>
        /// Loads the data from the raw data source and prepares it for use.
        /// </summary>
        /// <returns>The prepared data.</returns>
        protected virtual IEnumerable<T> LoadInternal()
        {
            List<T> preparedData = new();
            if (RawData is null)
            {
                return preparedData;
            }

            foreach (T item in RawData)
            {
                this.cancellationTokenSource.Token.ThrowIfCancellationRequested();
                preparedData.Add(item);
            }

            return preparedData;
        }

        /// <summary>
        /// Raises the <see cref="LoadingChanged"/> event.
        /// </summary>
        protected virtual void OnLoadingChanged()
        {
            if (!IsDisposed)
            {
                RaisePropertyChanged(new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }

        /// <summary>
        /// Raises the <see cref="LoadingChanged"/> event.
        /// </summary>
        /// <param name="args">The property changed event arguments.</param>
        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs args) => LoadingChanged?.Invoke(this, args);

        /// <summary>
        /// Disposes the provider.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();

                lock (this.cancellationTokenSource)
                {
                    if (this.loadingTask != null)
                    {
                        this.loadingTask.ContinueWith(_ => DoDispose());
                    }
                    else
                    {
                        DoDispose();
                    }
                }
            }
        }

        /// <summary>
        /// Disposes the resources used by the provider.
        /// </summary>
        protected virtual void DoDispose() => this.cancellationTokenSource.Dispose();
    }
}
