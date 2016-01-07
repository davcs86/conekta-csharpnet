using ConektaCSharp;
using NUnit.Framework;

namespace ConektaCSharpTests
{
    [TestFixture]
    public class EventTest
    {
        public EventTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
        }

        [Test]
        public void testSuccesfulWhere()
        {
            var events = Event.where();
            Assert.IsTrue(events[0] is Event);
        }
    }
}