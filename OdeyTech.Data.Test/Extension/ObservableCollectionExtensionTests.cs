// --------------------------------------------------------------------------
// <copyright file="ObservableCollectionExtensionTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdeyTech.Data.Extension;
using OdeyTech.Data.Test.Model;

namespace OdeyTech.Data.Test.Extension
{
    [TestClass]
    public class ObservableCollectionExtensionTests
    {
        [TestMethod]
        public void Copy_ShouldReturnNewCollectionWithSameElements()
        {
            // Arrange
            var originalCollection = new ObservableCollection<TestModel>
            {
                new TestModel(1),
                new TestModel(2),
                new TestModel(3)
            };

            // Act
            ObservableCollection<TestModel> copiedCollection = originalCollection.Copy();

            // Assert
            Assert.AreNotSame(originalCollection, copiedCollection);
            Assert.AreEqual(originalCollection.Count, copiedCollection.Count);

            foreach (TestModel item in copiedCollection)
            {
                Assert.IsTrue(originalCollection.Any(x => x.Identifier == item.Identifier));
            }
        }

        [TestMethod]
        public void Copy_ShouldReturnEmptyCollectionWhenOriginalIsEmpty()
        {
            // Arrange
            var originalCollection = new ObservableCollection<TestModel>();

            // Act
            ObservableCollection<TestModel> copiedCollection = originalCollection.Copy();

            // Assert
            Assert.AreNotSame(originalCollection, copiedCollection);
            Assert.AreEqual(0, copiedCollection.Count);
        }

        [TestMethod]
        public void Sort_ShouldSortCollectionInAscendingOrder()
        {
            // Arrange
            var collection = new ObservableCollection<TestModel>
            {
                new TestModel(3),
                new TestModel(1),
                new TestModel(2)
            };

            // Act
            collection.Sort();

            // Assert
            Assert.AreEqual(1ul, collection[0].Identifier);
            Assert.AreEqual(2ul, collection[1].Identifier);
            Assert.AreEqual(3ul, collection[2].Identifier);
        }

        [TestMethod]
        public void Sort_ShouldNotChangeOrderOfAlreadySortedCollection()
        {
            // Arrange
            var collection = new ObservableCollection<TestModel>
            {
                new TestModel(1),
                new TestModel(2),
                new TestModel(3)
            };

            // Act
            collection.Sort();

            // Assert
            Assert.AreEqual(1ul, collection[0].Identifier);
            Assert.AreEqual(2ul, collection[1].Identifier);
            Assert.AreEqual(3ul, collection[2].Identifier);
        }

        [TestMethod]
        public void Sort_ShouldNotChangeOrderOfEmptyCollection()
        {
            // Arrange
            var collection = new ObservableCollection<TestModel>();

            // Act
            collection.Sort();

            // Assert
            Assert.AreEqual(0, collection.Count);
        }
    }
}
