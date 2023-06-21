// --------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using OdeyTech.Data.Provider;
using OdeyTech.Data.Repository.Interface;
using OdeyTech.Data.Test.Model;

namespace OdeyTech.Data.Test.Provider
{
    internal class TestDataProvider : DataProvider<TestModel>
    {
        public TestDataProvider(IRepository<TestModel> repository) : base(repository)
        { }
    }
}
