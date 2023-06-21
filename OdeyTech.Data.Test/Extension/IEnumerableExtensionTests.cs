// --------------------------------------------------------------------------
// <copyright file="IEnumerableExtensionTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdeyTech.Data.Extension;
using OdeyTech.Data.Test.Model;

namespace OdeyTech.Data.Test.Extension
{
    [TestClass]
    public class IEnumerableExtensionTests
    {
        [TestMethod]
        public void Clone_ShouldReturnNewCollectionWithSameElements()
        {
            // Arrange
            var originalCollection = new HashSet<TestModel>
            {
                new TestModel(1),
                new TestModel(2),
                new TestModel(3)
            };

            // Act
            IEnumerable<TestModel> clonedCollection = originalCollection.Clone();

            // Assert
            Assert.AreNotSame(originalCollection, clonedCollection);
            Assert.AreEqual(originalCollection.Count, clonedCollection.Count());

            foreach (TestModel item in clonedCollection)
            {
                Assert.IsTrue(originalCollection.Any(x => x.Identifier == item.Identifier));
            }
        }
    }
}
