// --------------------------------------------------------------------------
// <copyright file="BaseProviderTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Threading;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Provider.Interface;

namespace OdeyTech.Data.Test.Provider
{
    public abstract class BaseProviderTests
    {
        internal void LoadProvider<T>(IDataProvider<T> provider) where T : IModel
        {
            // Wait for the LoadingChanged event to be fired
            var loadingChangedEventFired = new ManualResetEvent(false);
            provider.LoadingChanged += (sender, e) =>
            {
                if (!provider.IsLoading)
                {
                    loadingChangedEventFired.Set();
                }
            };

            // Start loading data
            provider.Load();

            // Wait for the LoadingChanged event to be fired and IsLoading to be false
            loadingChangedEventFired.WaitOne();
        }
    }
}
