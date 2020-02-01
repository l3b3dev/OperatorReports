using System;
using System.Collections.Generic;
using System.IO;
using BusinessEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.Excel.IntegrationTests
{
    [TestClass]
    public class ReportCreatorIntegrationTest
    {
        [TestMethod]
        public void ReportCreator_CreateAndSaveLocally_Success()
        {
            //Arrange
            var data = new List<OperatorReport>
            {
                new OperatorReport
                {
                    Id = 1,
                    Name = "Faisal",
                    ProactiveSent = 105,
                    ProactiveAnswered = 100,
                    ProactiveResponseRate = 95,
                    ReactiveReceived = 104,
                    ReactiveAnswered = 100,
                    ReactiveResponseRate = 96,
                    TotalChatLength = "3d 1h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Greg",
                    ProactiveSent = 23,
                    ProactiveAnswered = 45,
                    ProactiveResponseRate = 34,
                    ReactiveReceived = 45,
                    ReactiveAnswered = 43,
                    ReactiveResponseRate = 34,
                    TotalChatLength = "1d 11h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Stuart",
                    ProactiveSent = 45,
                    ProactiveAnswered = 87,
                    ProactiveResponseRate = 48,
                    ReactiveReceived = 86,
                    ReactiveAnswered = 36,
                    ReactiveResponseRate = 56,
                    TotalChatLength = "7d 1h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Michael",
                    ProactiveSent = 87,
                    ProactiveAnswered = 98,
                    ProactiveResponseRate = 30,
                    ReactiveReceived = 43,
                    ReactiveAnswered = 48,
                    ReactiveResponseRate = 86,
                    TotalChatLength = "6d 1h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Johny",
                    ProactiveSent = 0,
                    ProactiveAnswered = 0,
                    ProactiveResponseRate = 0,
                    ReactiveReceived = 0,
                    ReactiveAnswered = 0,
                    ReactiveResponseRate = 0,
                    TotalChatLength = null,
                    AverageChatLength = null
                }
            };

            var path = @"C:/temp/rep.xlsx";

            var creator = new ReportCreator();

            //Act
            creator.Generate(data, path);

            //Assert
            Assert.IsTrue(File.Exists(path));
        }

         [TestMethod]
        public void ReportCreator_CreateAndGetMemStream_Success()
        {
            //Arrange
            var data = new List<OperatorReport>
            {
                new OperatorReport
                {
                    Id = 1,
                    Name = "Faisal",
                    ProactiveSent = 105,
                    ProactiveAnswered = 100,
                    ProactiveResponseRate = 95,
                    ReactiveReceived = 104,
                    ReactiveAnswered = 100,
                    ReactiveResponseRate = 96,
                    TotalChatLength = "3d 1h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Greg",
                    ProactiveSent = 23,
                    ProactiveAnswered = 45,
                    ProactiveResponseRate = 34,
                    ReactiveReceived = 45,
                    ReactiveAnswered = 43,
                    ReactiveResponseRate = 34,
                    TotalChatLength = "1d 11h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Stuart",
                    ProactiveSent = 45,
                    ProactiveAnswered = 87,
                    ProactiveResponseRate = 48,
                    ReactiveReceived = 86,
                    ReactiveAnswered = 36,
                    ReactiveResponseRate = 56,
                    TotalChatLength = "7d 1h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Michael",
                    ProactiveSent = 87,
                    ProactiveAnswered = 98,
                    ProactiveResponseRate = 30,
                    ReactiveReceived = 43,
                    ReactiveAnswered = 48,
                    ReactiveResponseRate = 86,
                    TotalChatLength = "6d 1h 20m",
                    AverageChatLength = "22m"
                },
                new OperatorReport
                {
                    Id = 1,
                    Name = "Johny",
                    ProactiveSent = 0,
                    ProactiveAnswered = 0,
                    ProactiveResponseRate = 0,
                    ReactiveReceived = 0,
                    ReactiveAnswered = 0,
                    ReactiveResponseRate = 0,
                    TotalChatLength = null,
                    AverageChatLength = null
                }
            };
            var path = @"C:/temp/repFromMemStr.xlsx";
            var creator = new ReportCreator();

            //Act
            using (var stream = new MemoryStream())
            {
                creator.Generate(data, stream);

                //dump to file for testing
                using (var fs = File.Open(path, FileMode.Create))
                {
                    stream.WriteTo(fs);
                }
            }

            //Assert
            Assert.IsTrue(File.Exists(path));
        }
    }
}
