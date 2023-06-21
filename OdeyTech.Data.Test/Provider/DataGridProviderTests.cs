// --------------------------------------------------------------------------
// <copyright file="DataGridProviderTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OdeyTech.Data.Model;
using OdeyTech.Data.Provider;
using OdeyTech.Data.Provider.Interface;
using OdeyTech.Data.Test.Model;
using OdeyTech.ProductivityKit.Enum;

namespace OdeyTech.Data.Test.Provider
{
    [TestClass]
    public class DataGridProviderTests
    {
        private Mock<IDataProvider<TestModel>> dataProviderMock;
        private DataGridProvider<TestModel> dataGridProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.dataProviderMock = new Mock<IDataProvider<TestModel>>();
            this.dataGridProvider = new TestDataGridProvider(this.dataProviderMock.Object);
        }

        [TestMethod]
        public void ClickButton_New_ShouldCreateNewItem()
        {
            this.dataGridProvider.ClickButton(ButtonName.New);
            this.dataProviderMock.Verify(dp => dp.NewItem(), Times.Once);
        }

        [TestMethod]
        public void ClickButton_Add_ShouldAddNewItem()
        {
            var newItem = new TestModel(1);
            this.dataGridProvider.EditItem = newItem;

            this.dataGridProvider.ClickButton(ButtonName.Add);

            this.dataProviderMock.Verify(dp => dp.Add(It.IsAny<TestModel>()), Times.Once);
        }

        [TestMethod]
        public void ClickButton_Edit_ShouldEndEdit()
        {
            var editItem = new TestModel(1);
            this.dataGridProvider.EditItem = editItem;

            this.dataGridProvider.ClickButton(ButtonName.Edit);

            this.dataProviderMock.Verify(dp => dp.EndEdit(It.IsAny<TestModel>()), Times.Once);
        }

        [TestMethod]
        public void ClickButton_Remove_ShouldRemoveItem()
        {
            var removeItem = new TestModel(1);
            this.dataProviderMock.Setup(r => r.Items).Returns(new RangeObservableCollection<TestModel>() { removeItem });
            this.dataGridProvider.EditItem = removeItem;

            this.dataGridProvider.ClickButton(ButtonName.Remove);

            this.dataProviderMock.Verify(dp => dp.Remove(It.IsAny<TestModel>()), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldUnsubscribeFromLoadingChangedEvent()
        {
            this.dataGridProvider.Dispose();

            this.dataProviderMock.VerifyRemove(dp => dp.LoadingChanged -= It.IsAny<PropertyChangedEventHandler>(), Times.Once);
        }
    }
}
