// --------------------------------------------------------------------------
// <copyright file="TestLoaderProvider.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using OdeyTech.Data.Provider;
using OdeyTech.Data.Test.Model;

namespace OdeyTech.Data.Test.Provider
{
    internal class TestLoaderProvider : LoaderProvider<TestModel>
    {
        private readonly List<TestModel> repository;

        protected override IEnumerable<TestModel> RawData => this.repository;

        public TestLoaderProvider(List<TestModel> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
