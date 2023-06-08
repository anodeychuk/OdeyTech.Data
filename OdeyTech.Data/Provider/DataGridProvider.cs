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
using OdeyTech.ProductivityKit.Enum;

namespace OdeyTech.Data.Provider
{
    public abstract class DataGridProvider<T> : ObservableObject, IDisposable where T : IModel
    {
        private ButtonName buttonName;
        private T selectedItem;
        private T editItem;
        private readonly IDataProvider<T> itemProvider;

        public DataGridProvider(IDataProvider<T> itemProvider)
        {
            this.itemProvider = itemProvider;
            this.itemProvider.LoadingChanged += ItemProvider_LoadingChanged;
        }

        public void Dispose() => this.itemProvider.LoadingChanged -= ItemProvider_LoadingChanged;

        public ObservableCollection<T> Items => this.itemProvider.Items;

        public T EditItem
        {
            get => this.editItem;
            set => SetProperty(ref this.editItem, value, nameof(EditItem));
        }

        public bool CanNew => IsOperable && ButtonName != ButtonName.New;

        public bool CanAdd => IsOperable && ButtonName == ButtonName.New;

        public bool CanEdit => IsOperable && ButtonName != ButtonName.New && SelectedItem != null;

        public bool CanDelete => IsOperable && ButtonName != ButtonName.New && SelectedItem != null;

        private bool IsOperable => !this.itemProvider.IsLoading;

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

        private ButtonName ButtonName
        {
            get => this.buttonName;
            set
            {
                SetProperty(ref this.buttonName, value, nameof(ButtonName));
                RefreshButton();
            }
        }

        public event PropertyChangedEventHandler LoadingChanged;

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
                    if (nearItem != null)
                    {
                        SelectedItem = nearItem;
                    }
                    else
                    {
                        EditItem = this.itemProvider.NewItem();
                        buttonName = ButtonName.New;
                    }

                    break;
            }

            ButtonName = buttonName;
        }

        public abstract ButtonName ShowRemoveAsk();

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
