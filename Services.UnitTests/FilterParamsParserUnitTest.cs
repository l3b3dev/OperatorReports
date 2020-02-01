using System;
using System.Collections.Generic;
using DataAccessLogicComponent.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Services.UnitTests
{
    [TestClass]
    public class FilterParamsParserUnitTest
    {
        private List<string> _allDevices = new List<string>()
        {
            "Desktop",
            "Mobile"
        };
        private List<string> _allWebsites = new List<string>()
        {
            "Gucci",
            "ToysRUs"
        };

        [TestMethod]
        public void FilterParamsParser_ParseToday_Success()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sdate = "Today";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(null,null,null,null, sdate);

            //Assert
            Assert.AreEqual(DateTime.Today,result.SelectedFromDate);
            Assert.AreEqual(DateTime.Today.AddDays(1),result.SelectedToDate);
        }

        [TestMethod]
        public void FilterParamsParser_ParseYesterday_Success()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sdate = "Yesterday";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(null,null,null,null, sdate);

            //Assert
            Assert.AreEqual(DateTime.Today.AddDays(-1),result.SelectedFromDate);
            Assert.AreEqual(DateTime.Today,result.SelectedToDate);
        }

        [TestMethod]
        public void FilterParamsParser_ParseThisWeek_Success()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sdate = "ThisWeek";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(null,null,null,null, sdate);

            //Assert
            Assert.AreEqual("1/26/2020",result.SelectedFromDate.Value.ToShortDateString());
            Assert.AreEqual("2/2/2020",result.SelectedToDate.Value.ToShortDateString());
        }

        [TestMethod]
        public void FilterParamsParser_ParseThisMonth_Success()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sdate = "ThisMonth";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(null,null,null,null, sdate);

            //Assert
            Assert.AreEqual("1/31/2020",result.SelectedFromDate.Value.ToShortDateString());
            Assert.AreEqual("2/29/2020",result.SelectedToDate.Value.ToShortDateString());
        }

        [TestMethod]
        public void FilterParamsParser_ParseThisYear_Success()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sdate = "ThisYear";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(null,null,null,null, sdate);

            //Assert
            Assert.AreEqual("1/1/2020",result.SelectedFromDate.Value.ToShortDateString());
            Assert.AreEqual("12/31/2020",result.SelectedToDate.Value.ToShortDateString());
        }

        [TestMethod]
        public void FilterParamsParser_ParseInjectParams_SuccessfullyHandled()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sw = ";SELECT * FROM Visitor; DROP members--";
            var sd = "if ((select user) = 'sa' OR (select user) = 'dbo') select 1 else select 1/0";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(sw,sd,null,null, null);

            //Assert
            Assert.IsNull(result.SelectedDevice);
            Assert.IsNull(result.SelectedWebsite);
        }

        [TestMethod]
        public void FilterParamsParser_ParseCorrectDeviceAndWebsite_Success()
        {
            //Arrange
            var mock = new Mock<IReportsRepository>();
            mock.Setup(fn => fn.GetAllDevices()).Returns(_allDevices);
            mock.Setup(fn => fn.GetAllWebsites()).Returns(_allWebsites);

            var sw = "Gucci";
            var sd = "Mobile";
            var parser = new FilterParamsParser(mock.Object);

            //Act
            var result = parser.Parse(sw,sd,null,null, null);

            //Assert
            Assert.AreEqual("Mobile", result.SelectedDevice);
            Assert.AreEqual("Gucci",result.SelectedWebsite);
        }
    }
}
