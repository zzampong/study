using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DapperDemo.Models.Tests
{
    [TestClass]
    public class TableRepositoryTests
    {
        [TestMethod]
        public void FirstDatCountIsZero()
        {
            ITableRepository repo = new TableRepository();

            //var tables = repo.GetAll();

            //var actual = tables.Count;
            //var expect = 0;

            //Assert.AreEqual(expect, actual);

            TableViewModel model = new TableViewModel();
            model.Note = "테이터 루루";
            var id = repo.Add(model);

            Assert.AreEqual(3, id);
        }
    }
}
