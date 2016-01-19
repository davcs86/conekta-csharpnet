using System;
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

		public void setApiKey() {
			string apiFromEnvironment = Environment.GetEnvironmentVariable("CONEKTA_APIKEY", EnvironmentVariableTarget.Machine);
			if (string.IsNullOrWhiteSpace(apiFromEnvironment))
				apiFromEnvironment = "key_eYvWV7gSDkNYXsmr"; // use your public key
			Conekta.ApiKey = apiFromEnvironment;
		}

        public ErrorTest()
        {
			setApiKey();

            valid_visa_card = JObject.Parse("{'name': 'test', 'cards':['tok_test_visa_4242']}");
            invalid_visa_card = JObject.Parse("{'name': 'test', 'cards':[{0:'tok_test_visa_4242'}]}");
            invalid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                                   "'reference_id':'9839-wolf_pack'," +
                                                   "'amount':10," +
                                                   "'currency':'MXN'}");
            valid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                                 "'reference_id':'9839-wolf_pack'," +
												 "'details':{" +
												 "   'name':'Wolverine', " +
												 "   'email':'buyeremail@email.com', "+
												 "   'line_items': [" +
												 "            {" +
												 "                'type': 'ecommerce_shopping'," +
												 "                'quantity': 2," +
												 "                'name': 'Bg Ss Shoshanna Yd Polo Dress'," +
												 "                'description': 'Un vestido que podría ser definido como sporty- chic. De cuello blanco y líneas horizontales en varios colores, hace referencia a la vida en altamar.  '," +
												 "                'sku': '7500244741187'," +
												 "                'unit_price': 59000" +
												 "            }]" +
												 "}, " +
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
			setApiKey();
        }

        [Test]
        public void testParameterValidationError()
        {
			var _params = (JObject)invalid_payment_method.DeepClone();;
			_params["card"] = JObject.Parse("{'card':'tok_test_visa_4242'}")["card"];
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
			var _params = (JObject)valid_payment_method.DeepClone();
			_params["card"] = JObject.Parse("{'card':'tok_test_visa_4242'}")["card"];
            _params["capture"] = "false";
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.status.Equals("pre_authorized"));
            charge.capture();
            try
            {
                charge.refund();
            }
			catch (Exception e)
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