﻿using NUnit.Framework;
using pst.tests.Properties;
using System.IO;
using System.Linq;

namespace pst.tests
{
    [TestFixture]
    public class MessageStoreTests
    {
        [Test]
        public void ShouldCorrectlyReadThePSTDisplayName()
        {
            //Arrange
            var sut = new PSTFile(new MemoryStream(Resources.PST));

            //Act
            var store = sut.GetMessageStore();

            //Assert
            Assert.AreEqual("Test", store.DisplayName);
        }

        [Test]
        public void ShouldDetectInboxFolder()
        {
            //Arrange
            var sut = new PSTFile(new MemoryStream(Resources.PST));

            //Act
            var topOfPSTDataFile = sut.GetRootFolder().GetSubFolders();

            //Assert
            Assert.IsTrue(topOfPSTDataFile[0].GetSubFolders().Any(f => f.DisplayName == "Inbox"));
        }
    }
}
