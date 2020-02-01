using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.UnitTests
{
    [TestClass]
    public class DurationParserUnitTest
    {
        [TestMethod]
        public void DurationParser_ConvertUnderMin_Success()
        {
            //Arrange
            var strDuration = "0.56";
            var dParser = new DurationParser();

            //Act
            var result = dParser.Parse(strDuration);

            //Assert
            Assert.AreEqual("34s",result);
        }

        [TestMethod]
        public void DurationParser_ConvertOveMinUnder60Mins_Success()
        {
            //Arrange
            var strDuration = "47.32";
            var dParser = new DurationParser();

            //Act
            var result = dParser.Parse(strDuration);

            //Assert
            Assert.AreEqual("47m 19s",result);
        }

        [TestMethod]
        public void DurationParser_ConvertOve60MinUnder24hr_Success()
        {
            //Arrange
            var strDuration = "1134.27";
            var dParser = new DurationParser();

            //Act
            var result = dParser.Parse(strDuration);

            //Assert
            Assert.AreEqual("18h 54m 16s",result);
        }

        [TestMethod]
        public void DurationParser_ConvertOver24hr_Success()
        {
            //Arrange
            var strDuration = "2456.48";
            var dParser = new DurationParser();

            //Act
            var result = dParser.Parse(strDuration);

            //Assert
            Assert.AreEqual("1d 16h 56m 28s",result);
        }
    }
}
