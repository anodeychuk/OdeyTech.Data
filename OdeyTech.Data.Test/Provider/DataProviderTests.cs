// --------------------------------------------------------------------------
// <copyright file="DataProviderTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OdeyTech.Data.Repository.Interface;
using OdeyTech.Data.Test.Model;

namespace OdeyTech.Data.Test.Provider
{
    [TestClass]
    public class DataProviderTests : BaseProviderTests
    {
        private Mock<IRepository<TestModel>> repositoryMock;
        private TestDataProvider dataProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            this.repositoryMock = new Mock<IRepository<TestModel>>();
            this.repositoryMock.Setup(r => r.CheckDatabase());

            this.dataProvider = new TestDataProvider(this.repositoryMock.Object);
        }

        [TestMethod]
        public void Add_ShouldAddNewItem()
        {
            // Arrange
            var newItem = new TestModel(1);

            // Act
            this.dataProvider.Add(newItem);

            // Assert
            this.repositoryMock.Verify(r => r.Insert(newItem), Times.Once);
        }

        [TestMethod]
        public void EndEdit_ShouldUpdateExistingItem()
        {
            // Arrange
            var updatedItem = new TestModel(1);
            var testData = new List<TestModel> { updatedItem };
            this.repositoryMock.Setup(r => r.Select()).Returns(testData);

            // Act
            LoadProvider(this.dataProvider);
            this.dataProvider.EndEdit(updatedItem);

            // Assert
            this.repositoryMock.Verify(r => r.Update(updatedItem), Times.Once);
        }

        [TestMethod]
        public void Remove_ShouldRemoveExistingItem()
        {
            // Arrange
            var removedItem = new TestModel(1);

            // Act
            this.dataProvider.Remove(removedItem);

            // Assert
            this.repositoryMock.Verify(r => r.Delete(removedItem), Times.Once);
        }

        [TestMethod]
        public void Constructor_ShouldLoadData()
        {
            // Arrange
            var testData = new List<TestModel> { new TestModel(1) };
            this.repositoryMock.Setup(r => r.Select()).Returns(testData);

            // Act
            LoadProvider(this.dataProvider);

            // Assert
            Assert.AreEqual(testData.Count, this.dataProvider.Items.Count);
            Assert.AreEqual(testData.First().Identifier, this.dataProvider.Items.First().Identifier);
        }

        [TestMethod]
        public void Add_ShouldThrowExceptionWhenRepositoryFails()
        {
            // Arrange
            this.repositoryMock.Setup(r => r.Insert(It.IsAny<TestModel>())).Throws<Exception>();
            var newItem = new TestModel(1);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.dataProvider.Add(newItem));
        }

        [TestMethod]
        public void EndEdit_ShouldThrowExceptionWhenRepositoryFails()
        {
            // Arrange
            this.repositoryMock.Setup(r => r.Update(It.IsAny<TestModel>())).Throws<Exception>();
            var updatedItem = new TestModel(1);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.dataProvider.EndEdit(updatedItem));
        }

        [TestMethod]
        public void Remove_ShouldThrowExceptionWhenRepositoryFails()
        {
            // Arrange
            this.repositoryMock.Setup(r => r.Delete(It.IsAny<TestModel>())).Throws<Exception>();
            var removedItem = new TestModel(1);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.dataProvider.Remove(removedItem));
        }
    }
}
