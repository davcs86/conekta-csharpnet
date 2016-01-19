using System;
using System.Diagnostics;
using ConektaCSharp;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    [TestFixture]
    public class ChargeTest
    {
        private readonly JObject invalid_payment_method;
        private readonly JObject valid_payment_method;
        private readonly JObject valid_visa_card;

        public ChargeTest()
        {
			string apiFromEnvironment = Environment.GetEnvironmentVariable("CONEKTA_APIKEY");
			if (string.IsNullOrWhiteSpace(apiFromEnvironment))
				apiFromEnvironment = "key_eYvWV7gSDkNYXsmr"; // use your public key
			Conekta.ApiKey = apiFromEnvironment;
			Debug.WriteLine(Conekta.ApiKey);
            valid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                                 "'reference_id':'9839-wolf_pack'," +
                                                 "'amount':20000," +
                                                 "'currency':'MXN'," +
                                                 "'details':{" +
                                                 "   'name':'Wolverine', " +
												 "   'email':'buyeremail@email.com', "+
                                                 "   'billing_address': {" +
                                                 "       'street1':'tamesis'" +
                                                 "   }, " +
                                                 "   'line_items': [" +
                                                 "            {" +
                                                 "                'type': 'ecommerce_shopping'," +
                                                 "                'quantity': 2," +
                                                 "                'name': 'Bg Ss Shoshanna Yd Polo Dress'," +
                                                 "                'description': 'Un vestido que podría ser definido como sporty- chic. De cuello blanco y líneas horizontales en varios colores, hace referencia a la vida en altamar.  '," +
                                                 "                'sku': '7500244741187'," +
                                                 "                'unit_price': 59000" +
                                                 "            }," +
                                                 "            {" +
                                                 "                'type': 'ecommerce_shopping'," +
                                                 "                'quantity': 1," +
                                                 "                'name': 'Farris Chino Gmd St'," +
                                                 "                'description': 'Producto Tommy'," +
                                                 "                'sku': '8718771353202'," +
                                                 "                'unit_price': 69000" +
                                                 "            }" +
                                                 "        ]}" +
                                                 "}");
            invalid_payment_method = JObject.Parse("{'description':'Stogies'," +
                                                   "'reference_id':'9839-wolf_pack'," +
                                                   "'amount':10," +
                                                   "'currency':'MXN'}");
            valid_visa_card = JObject.Parse("{'card':'tok_test_visa_4242'}");
        }

		private Charge CardPMCreate(){
			var _params = (JObject)valid_payment_method.DeepClone();
			_params.Remove("bank");
			_params["card"] = valid_visa_card.GetValue("card");
			var charge = Charge.create(_params);
			return charge;	
		}

		[Test]
        public void testSuccesfulCardPMCreate()
        {
			var charge = CardPMCreate();
            Assert.IsTrue(charge.status.Equals("paid"));
            Assert.IsNotNull(charge.details);
            Assert.IsNotNull(charge.details.billing_address);
            Assert.IsNotNull(charge.details.line_items);
            Assert.IsTrue(charge.details.line_items[0] is LineItems);
            Assert.IsTrue(charge.details.name.Equals("Wolverine"));
            Assert.IsTrue(charge.payment_method is CardPayment);
            
        }

		/*
		[Test]
        public void testSuccesfulFindCharge()
        {
			var charge = CardPMCreate();
            charge = Charge.find(charge.id);
            Assert.IsNotNull(charge);
        }

        [Test]
        public void testSuccesfulWhere()
        {
            var charges = Charge.where();
            Assert.IsNotNull(charges);
            Assert.IsTrue(charges[0] is ConektaObject);
        }*/

        [Test]
        public void testSuccesfulBankPMCreate()
        {
            var bank = JObject.Parse("{'bank':{'type':'banorte'}}");
			var _params = (JObject)valid_payment_method.DeepClone();
            _params["bank"] = bank["bank"];
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is BankTransferPayment);
            Assert.IsTrue(charge.status.Equals("pending_payment"));
        }

        [Test]
        public void testSuccesfulSPEIPMCreate()
        {
            var bank = JObject.Parse("{'bank':{'type':'spei'}}");
			var _params = (JObject)valid_payment_method.DeepClone();
            _params["bank"] = bank["bank"];
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is SpeiPayment);
            Assert.IsTrue(charge.status.Equals("pending_payment"));
            Assert.IsFalse(String.IsNullOrWhiteSpace(((SpeiPayment) charge.payment_method).clabe));
        }

        [Test]
        public void testSuccesfulOxxoPMCreate()
        {
            var cash = JObject.Parse("{'cash':{'type':'oxxo'}}");
			var _params = (JObject)valid_payment_method.DeepClone();
            _params["cash"] = cash["cash"];
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is OxxoPayment, "Pago");
            Assert.IsTrue(charge.status.Equals("pending_payment"), "Cargo");
            Assert.IsFalse(String.IsNullOrWhiteSpace(((OxxoPayment) charge.payment_method).barcode));
        }

		/*
		 * Real-time was removed from Conekta.io API
        [Test]
        public void testSuccesfulRealTimePMCreate()
        {
            var cash = JObject.Parse("{'cash':{'type':'real_time'}}");
			var _params = (JObject)valid_payment_method.DeepClone();
            _params["cash"] = cash["cash"];
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is RealTimePayment);
            Assert.IsTrue(charge.status.Equals("pending_payment"));
            Assert.IsFalse(String.IsNullOrWhiteSpace(((RealTimePayment) charge.payment_method).barcode));
        }*/

        [Test]
        public void testUnsuccesfulPMCreate()
        {
			var _params = (JObject)invalid_payment_method.DeepClone();
            _params["card"] = valid_visa_card["card"];
            try
            {
                Charge.create(_params);
                Assert.IsFalse(true);
            }
            catch (ParameterValidationError e)
            {
                Assert.IsNotNull(e);
            }
        }

        [Test]
        public void testSuccesfulRefund()
        {
			var charge = CardPMCreate();
            charge.refund(20000);
            Assert.IsTrue(charge.status.Equals("refunded"));
        }

        [Test]
        public void testUnsuccesfulRefund()
        {
			var charge = CardPMCreate();
            try
            {
                charge.refund(30000);
            }
            catch (ProcessingError e)
            {
                Assert.IsNotNull(e);
            }
        }

        [Test]
        public void testSuccesfulCapture()
        {
			var _params = (JObject)valid_payment_method.DeepClone();
			_params.Remove("bank");
			_params["card"] = (JValue)valid_visa_card["card"].DeepClone();
            _params["capture"] = "false";
            var charge = Charge.create(_params);
            Assert.IsTrue(charge.status.Equals("pre_authorized"));
            charge.capture();
            Assert.IsTrue(charge.status.Equals("paid"));
        }
    }
}