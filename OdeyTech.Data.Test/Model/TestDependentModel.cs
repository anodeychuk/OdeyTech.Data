// --------------------------------------------------------------------------
// <copyright file="TestModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using OdeyTech.Data.Model;

namespace OdeyTech.Data.Test.Model
{
    internal class TestDependentModel : DependentModel
    {
        public TestDependentModel() { }

        public TestDependentModel(ulong id) : base(id)
        { }
    }
}
