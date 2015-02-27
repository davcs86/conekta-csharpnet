using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConektaCSharp;

namespace ConektaCSharpTests
{
    [TestClass]
    public class EventTest
    {

        public EventTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
        }

        [TestMethod]
        public void testSuccesfulWhere() {
            ConektaObject events = Event.where();
            Assert.IsTrue(events[0] is Event);
        }  
    }
}
