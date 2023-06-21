// --------------------------------------------------------------------------
// <copyright file="TestDataGridProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using OdeyTech.Data.Provider;
using OdeyTech.Data.Provider.Interface;
using OdeyTech.Data.Test.Model;
using OdeyTech.ProductivityKit.Enum;

namespace OdeyTech.Data.Test.Provider
{
    internal class TestDataGridProvider : DataGridProvider<TestModel>
    {
        public TestDataGridProvider(IDataProvider<TestModel> itemProvider) : base(itemProvider) { }

        public override ButtonName ShowRemoveAsk() => ButtonName.Yes; // For testing, we can just return ButtonName.Yes
    }
}
