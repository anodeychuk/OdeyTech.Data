// --------------------------------------------------------------------------
// <copyright file="DataGridProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using OdeyTech.Data.Extension;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Provider.Interface;
using OdeyTech.Data.Repository.Interface;
using OdeyTech.ProductivityKit.Enum;

namespace OdeyTech.Data.Provider
{
    /// <summary>
    /// Base class for a data grid provider of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of the data model.</typeparam>
    public abstract class DataGridProvider<T> : ObservableObject, IDisposable where T : IModel
    {
        private ButtonName buttonName;
        private T selectedItem;
        private T editItem;
        private readonly IDataProvider<T> itemProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridProvider{T}"/> class.
        /// </summary>
        /// <param name="itemProvider">The item provider.</param>
        public DataGridProvider(IDataProvider<T> itemProvider)
        {
            this.itemProvider = itemProvider ?? throw new ArgumentNullException(nameof(itemProvider));
            this.itemProvider.LoadingChanged += ItemProvider_LoadingChanged;
        }

        /// <summary>
        /// Disposes the data grid provider.
        /// </summary>
        public void Dispose() => this.itemProvider.LoadingChanged -= ItemProvider_LoadingChanged;

        /// <summary>
        /// Gets the collection of items.
        /// </summary>
        public ObservableCollection<T> Items => this.itemProvider.Items;

        /// <summary>
        /// Gets or sets the item being edited.
        /// </summary>
        public T EditItem
        {
            get => this.editItem;
            set => SetProperty(ref this.editItem, value, nameof(EditItem));
        }

        /// <summary>
        /// Gets a value indicating whether a new item can be created.
        /// </summary>
        public bool CanNew => IsOperable && ButtonName != ButtonName.New;

        /// <summary>
        /// Gets a value indicating whether an item can be added.
        /// </summary>
        public bool CanAdd => IsOperable && ButtonName == ButtonName.New;

        /// <summary>
        /// Gets a value indicating whether the selected item can be edited.
        /// </summary>
        public bool CanEdit => IsOperable && ButtonName != ButtonName.New && SelectedItem != null;

        /// <summary>
        /// Gets a value indicating whether the selected item can be deleted.
        /// </summary>
        public bool CanDelete => IsOperable && ButtonName != ButtonName.New && SelectedItem != null;

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public T SelectedItem
        {
            get => this.selectedItem;
            set
            {
                SetProperty(ref this.selectedItem, value, nameof(SelectedItem));
                ButtonName = ButtonName.None;
                if (CanEdit)
                {
                    EditItem = this.itemProvider.BeginEdit(SelectedItem);
                }
            }
        }

        private bool IsOperable => !this.itemProvider.IsLoading;

        private ButtonName ButtonName
        {
            get => this.buttonName;
            set
            {
                SetProperty(ref this.buttonName, value, nameof(ButtonName));
                RefreshButton();
            }
        }

        /// <summary>
        /// Event that is raised when the loading state has changed.
        /// </summary>
        public event PropertyChangedEventHandler LoadingChanged;

        /// <summary>
        /// Performs the button click action.
        /// </summary>
        /// <param name="buttonName">The name of the button clicked.</param>
        public virtual void ClickButton(ButtonName buttonName)
        {
            switch (buttonName)
            {
                case ButtonName.New:
                    SetProperty(ref this.selectedItem, default, nameof(SelectedItem));
                    EditItem = this.itemProvider.NewItem();
                    break;

                case ButtonName.Add:
                    var toAdd = (T)EditItem.Clone();
                    this.itemProvider.Add(toAdd);
                    SelectedItem = toAdd;
                    OnPropertyChanged(nameof(Items));
                    break;

                case ButtonName.Edit:
                    this.itemProvider.EndEdit(EditItem);
                    OnPropertyChanged(nameof(Items));
                    break;

                case ButtonName.Remove:

                    if (ShowRemoveAsk() == ButtonName.No)
                    {
                        return;
                    }

                    this.itemProvider.Remove(EditItem);
                    OnPropertyChanged(nameof(Items));
                    T nearItem = Items.Count > 0 ? Items.Neighbor(EditItem.Identifier) : default;
                    if (nearItem is null)
                    {
                        EditItem = this.itemProvider.NewItem();
                        buttonName = ButtonName.New;
                    }
                    else
                    {
                        SelectedItem = nearItem;
                    }

                    break;
            }

            ButtonName = buttonName;
        }

        /// <summary>
        /// Displays a confirmation dialog for removing an item and returns the result.
        /// </summary>
        /// <returns>The <see cref="ButtonName"/> representing the user's response.</returns>
        public abstract ButtonName ShowRemoveAsk();

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs args) => LoadingChanged?.Invoke(this, args);

        private void RefreshButton()
        {
            OnPropertyChanged(nameof(CanNew));
            OnPropertyChanged(nameof(CanAdd));
            OnPropertyChanged(nameof(CanEdit));
            OnPropertyChanged(nameof(CanDelete));
        }

        private void ItemProvider_LoadingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IDataProvider<T>.IsLoading))
            {
                RaisePropertyChanged(e);
                if (Items.Count > 0)
                {
                    SelectedItem = Items.First();
                }
                else
                {
                    ClickButton(ButtonName.New);
                }
            }
        }
    }
}
