using ConektaCSharp;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    [TestFixture]
    public class ErrorTest
    {
        private JObject valid_visa_card;
        private readonly JObject invalid_payment_method;
        private readonly JObject invalid_visa_card;
        private readonly JObject valid_payment_method;

        public ErrorTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
            valid_visa_card = JObject.Parse("{'name': 'test', 'cards':['tok_test_visa_4242']}");
            invalid_visa_card = JObject.Parse("{'name': 'test', 'cards':[{0:'tok_test_visa_4242'}]}");
            invalid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                                   "'reference_id':'9839-wolf_pack'," +
                                                   "'amount':10," +
                                                   "'currency':'MXN'}");
            valid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                                 "'reference_id':'9839-wolf_pack'," +
                                                 "'amount':20000," +
                                                 "'currency':'MXN'}");
        }

        [Test]
        public void testNoIdError()
        {
            try
            {
                var charge = Charge.find(null);
            }
            catch (Error e)
            {
                Assert.IsTrue(e.message.Equals("Could not get the id of Resource instance."));
            }
        }

        /*
         * Conekta.ApiBase is a Constant value (can't be changed)
        [Test]
        public void testNoConnectionError() {
            String _base = Conekta.ApiBase;
            Conekta.ApiBase = "http://localhost:3001";
            try{
            Customer customer = Customer.create(valid_visa_card);
            } catch (NoConnectionError e) {
                Assert.IsTrue(e is NoConnectionError);
            }
            Conekta.ApiBase = _base;
        }
         * 
         */

        [Test]
        public void testApiError()
        {
            try
            {
                var customer = Customer.create(invalid_visa_card);
            }
            catch (Error e)
            {
                Assert.IsTrue(e is ParameterValidationError);
            }
        }

        [Test]
        public void testAuthenticationError()
        {
            Conekta.ApiKey = "";
            try
            {
                var customer = Customer.create(valid_visa_card);
            }
            catch (AuthenticationError e)
            {
                Assert.IsTrue(e is AuthenticationError);
            }
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
        }

        [Test]
        public void testParameterValidationError()
        {
            valid_visa_card = JObject.Parse("{'card':'tok_test_visa_4242'}");
            var _params = invalid_payment_method;
            _params["card"] = valid_visa_card["card"];
            try
            {
                Charge.create(_params);
                Assert.IsTrue(false);
            }
            catch (ParameterValidationError e)
            {
                Assert.IsTrue(e is ParameterValidationError);
            }
        }

        [Test]
        public void testProcessingError()
        {
            valid_visa_card = JObject.Parse("{'card':'tok_test_visa_4242'}");
            var _params = valid_payment_method;
            _params["card"] = valid_visa_card["card"];
            _params["capture"] = "false";
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.status.Equals("pre_authorized"));
            charge.capture();
            try
            {
                charge.refund();
            }
            catch (ProcessingError e)
            {
                Assert.IsTrue(e is ProcessingError);
            }
        }

        [Test]
        public void testResourceNotFoundError()
        {
            try
            {
                var charge = Charge.find("1");
            }
            catch (Error e)
            {
                Assert.IsTrue(e is ResourceNotFoundError);
            }
        }
    }
}