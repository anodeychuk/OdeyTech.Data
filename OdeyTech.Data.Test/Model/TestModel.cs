// --------------------------------------------------------------------------
// <copyright file="TestModel.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using OdeyTech.Data.Model;
using OdeyTech.Data.Model.Interface;

namespace OdeyTech.Data.Test.Model
{
    internal class TestModel : BasicModel, IComparable<TestModel>, IComparable
    {
        public TestModel() : base() { }

        public TestModel(ulong id) : base(id) { }

        public string TestProperty { get; set; }

        public override void CopyFrom(IModel obj)
        {
            if (obj is not TestModel testModel)
            {
                throw new ArgumentException("Cannot copy from an object that is not a TestModel.", nameof(obj));
            }

            base.CopyFrom(testModel);
            TestProperty = testModel.TestProperty;
        }

        public int CompareTo(TestModel other)
            => other is null
                ? 1
                : Identifier.CompareTo(other.Identifier);

        int IComparable.CompareTo(object obj)
            => obj == null
                ? 1
                : obj is TestModel other
                    ? CompareTo(other)
                    : throw new ArgumentException("Object is not a TestModel");
    }
}
