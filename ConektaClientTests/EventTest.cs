using ConektaCSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void testSuccesfulWhere()
        {
            var events = Event.where();
            Assert.IsTrue(events[0] is Event);
        }
    }
}