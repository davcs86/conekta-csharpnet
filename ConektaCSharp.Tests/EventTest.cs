using System;
using ConektaCSharp;
using NUnit.Framework;

namespace ConektaCSharpTests
{
    [TestFixture]
    public class EventTest
    {

		public void setApiKey() {
			string apiFromEnvironment = Environment.GetEnvironmentVariable("CONEKTA_APIKEY");
			if (string.IsNullOrWhiteSpace(apiFromEnvironment))
				apiFromEnvironment = "key_eYvWV7gSDkNYXsmr"; // use your public key
			Conekta.ApiKey = apiFromEnvironment;
		}

        public EventTest()
        {
			setApiKey();
        }

        [Test]
        public void testSuccesfulWhere()
        {
            var events = Event.where();
            Assert.IsTrue(events[0] is Event);
        }
    }
}