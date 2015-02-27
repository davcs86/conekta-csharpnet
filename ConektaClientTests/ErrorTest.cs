using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConektaCSharp;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    [TestClass]
    public class ErrorTest
    {
        
        JObject valid_visa_card;
        JObject invalid_visa_card;
        JObject invalid_payment_method;
        JObject valid_payment_method;
        public ErrorTest() {
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

        [TestMethod]
        public void testNoIdError() {
            try {
                Charge charge = Charge.find(null);
            } catch (Error e) {
                Assert.IsTrue(e.message.Equals("Could not get the id of Resource instance."));
            }
        }

        /*
         * Conekta.ApiBase is a Constant value (can't be changed)
        [TestMethod]
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

        [TestMethod]
        public void testApiError() {
            try{
                Customer customer = Customer.create(invalid_visa_card);
            }
            catch (Error e)
            {
                Assert.IsTrue(e is ParameterValidationError);
            }
        }

        [TestMethod]
        public void testAuthenticationError() {
            Conekta.ApiKey = "";
            try{
            Customer customer = Customer.create(valid_visa_card);
            } catch (AuthenticationError e) {
                Assert.IsTrue(e is AuthenticationError);
            }
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
        }

        [TestMethod]
        public void testParameterValidationError() {
            valid_visa_card = JObject.Parse("{'card':'tok_test_visa_4242'}");
            JObject _params = invalid_payment_method;
            _params["card"] = valid_visa_card["card"];
            try {
                Charge.create(_params);
                Assert.IsTrue(false);
            } catch (ParameterValidationError e) {
                Assert.IsTrue(e is ParameterValidationError);
            }
        }

        [TestMethod]
        public void testProcessingError() {
            valid_visa_card = JObject.Parse("{'card':'tok_test_visa_4242'}");
            JObject _params = valid_payment_method;
            _params["card"] = valid_visa_card["card"];
            _params["capture"] = "false";
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.status.Equals("pre_authorized"));
            charge.capture();
            try {
                charge.refund();
            } catch(ProcessingError e) {
                Assert.IsTrue(e is ProcessingError);
            }
        }

        [TestMethod]
        public void testResourceNotFoundError() {
            try {
                Charge charge = Charge.find("1");
            } catch (Error e) {
                Assert.IsTrue(e is ResourceNotFoundError);
            }
        }
    }
}
