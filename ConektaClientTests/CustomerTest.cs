using System;
using ConektaCSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    [TestClass]
    public class CustomerTest
    {

        JObject valid_visa_card;

        public CustomerTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
            valid_visa_card = JObject.Parse("{'name': 'test', 'cards':['tok_test_visa_4242']}");
        }

    }
}
