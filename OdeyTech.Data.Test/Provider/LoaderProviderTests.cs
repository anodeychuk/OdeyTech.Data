// --------------------------------------------------------------------------
// <copyright file="LoaderProviderTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdeyTech.Data.Test.Model;

namespace OdeyTech.Data.Test.Provider
{
    [TestClass]
    public class LoaderProviderTests : BaseProviderTests
    {
        [TestMethod]
        public void Load_ShouldLoadData()
        {
            // Arrange
            var rawData = new List<TestModel>
            {
                new TestModel(1),
                new TestModel(2),
                new TestModel(3)
            };
            using var loaderProvider = new TestLoaderProvider(rawData);

            // Act
            LoadProvider(loaderProvider);

            // Assert
            Assert.AreEqual(rawData.Count, loaderProvider.Items.Count);
            foreach (TestModel item in loaderProvider.Items)
            {
                Assert.IsTrue(rawData.Any(x => x.Identifier == item.Identifier));
            }
        }

        [TestMethod]
        public void Add_ShouldAddNewItem()
        {
            // Arrange
            using var loaderProvider = new TestLoaderProvider(new List<TestModel>());
            var newItem = new TestModel(1);

            // Act
            loaderProvider.Add(newItem);

            // Assert
            Assert.AreEqual(1, loaderProvider.Items.Count);
            Assert.AreEqual(newItem, loaderProvider.Items.First());
        }

        [TestMethod]
        public void BeginEdit_ShouldReturnCloneOfItem()
        {
            // Arrange
            var originalItem = new TestModel(1);
            using var loaderProvider = new TestLoaderProvider(new List<TestModel> { originalItem });
            LoadProvider(loaderProvider);

            // Act
            TestModel clonedItem = loaderProvider.BeginEdit(originalItem);

            // Assert
            Assert.AreNotSame(originalItem, clonedItem);
            Assert.AreEqual(originalItem.Identifier, clonedItem.Identifier);
        }

        [TestMethod]
        public void EndEdit_ShouldUpdateExistingItem()
        {
            // Arrange
            var originalItem = new TestModel(1) { TestProperty = "Test" };
            using var loaderProvider = new TestLoaderProvider(new List<TestModel> { originalItem });
            LoadProvider(loaderProvider);

            // Act
            TestModel updatedItem = loaderProvider.BeginEdit(originalItem);
            updatedItem.TestProperty = "Updated Test";
            loaderProvider.EndEdit(updatedItem);

            // Assert
            Assert.AreEqual(1, loaderProvider.Items.Count);
            Assert.AreEqual(updatedItem.Identifier, loaderProvider.Items.First().Identifier);
            Assert.AreEqual(updatedItem.TestProperty, loaderProvider.Items.First().TestProperty);
        }

        [TestMethod]
        public void Remove_ShouldRemoveExistingItem()
        {
            // Arrange
            var originalItem = new TestModel(1);
            using var loaderProvider = new TestLoaderProvider(new List<TestModel> { originalItem });
            LoadProvider(loaderProvider);

            // Act
            loaderProvider.Remove(originalItem);

            // Assert
            Assert.AreEqual(0, loaderProvider.Items.Count);
        }
    }
}
