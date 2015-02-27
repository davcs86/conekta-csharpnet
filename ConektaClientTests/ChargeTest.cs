using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConektaCSharp;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    
    [TestClass]
    public class ChargeTest
    {
        JObject valid_payment_method;
        JObject invalid_payment_method;
        JObject valid_visa_card;

        public ChargeTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
            valid_payment_method = JObject.Parse("{'description':'Stogies'," +
                "'reference_id':'9839-wolf_pack'," +
                "'amount':20000," +
                "'currency':'MXN'," +
                "'details':{" +
                "   'name':'Wolverine', " +
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

        public Charge testSuccesfulCardPMCreate()
        {
            JObject _params = valid_payment_method;
            _params["card"] = valid_visa_card.GetValue("card");
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.status.Equals("paid"));
            Assert.IsNotNull(charge.details);
            Assert.IsNotNull(charge.details.billing_address);
            Assert.IsNotNull(charge.details.line_items);
            Assert.IsTrue(charge.details.line_items[0] is LineItems);
            Assert.IsTrue(charge.details.name.Equals("Wolverine"));
            Assert.IsTrue(charge.payment_method is CardPayment);
            return charge;
        }

        [TestMethod]
        public void testSuccesfulFindCharge()
        {
            var charge = testSuccesfulCardPMCreate();
            charge = Charge.find(charge.id);
            Assert.IsNotNull(charge);
        }

        [TestMethod]
        public void testSuccesfulWhere()
        {
            ConektaObject charges = Charge.where();
            Assert.IsNotNull(charges);
            Assert.IsTrue(charges[0] is ConektaObject);
        }

        [TestMethod]
        public void testSuccesfulBankPMCreate()
        {
            JObject bank = JObject.Parse("{'bank':{'type':'banorte'}}");
            JObject _params = valid_payment_method;
            _params["bank"] = bank["bank"];
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is BankTransferPayment);
            Assert.IsTrue(charge.status.Equals("pending_payment"));
        }

        [TestMethod]
        public void testSuccesfulSPEIPMCreate()
        {
            JObject bank = JObject.Parse("{'bank':{'type':'spei'}}");
            JObject _params = valid_payment_method;
            _params["bank"] = bank["bank"];
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is SpeiPayment);
            Assert.IsTrue(charge.status.Equals("pending_payment"));
            Assert.IsFalse(String.IsNullOrWhiteSpace(((SpeiPayment)charge.payment_method).clabe));
        }

        [TestMethod]
        public void testSuccesfulOxxoPMCreate()
        {
            JObject cash = JObject.Parse("{'cash':{'type':'oxxo'}}");
            JObject _params = valid_payment_method;
            _params["cash"] = cash["cash"];
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is OxxoPayment, "Pago");
            Assert.IsTrue(charge.status.Equals("pending_payment"), "Cargo");
            Assert.IsFalse(String.IsNullOrWhiteSpace(((OxxoPayment)charge.payment_method).barcode));
        }

        [TestMethod]
        public void testSuccesfulRealTimePMCreate()
        {
            JObject cash = JObject.Parse("{'cash':{'type':'real_time'}}");
            JObject _params = valid_payment_method;
            _params["cash"] = cash["cash"];
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.payment_method is RealTimePayment);
            Assert.IsTrue(charge.status.Equals("pending_payment"));
            Assert.IsFalse(String.IsNullOrWhiteSpace(((RealTimePayment)charge.payment_method).barcode));
        }

        [TestMethod]
        public void testUnsuccesfulPMCreate()
        {
            JObject _params = invalid_payment_method;
            _params["card"] = valid_visa_card["card"];
            try {
                Charge.create(_params);
                Assert.IsFalse(true);
            } catch (ParameterValidationError e) {
                Assert.IsNotNull(e);
            }
        }

        [TestMethod]
        public void testSuccesfulRefund() {
            Charge charge = testSuccesfulCardPMCreate();
            charge.refund(20000);
            Assert.IsTrue(charge.status.Equals("refunded"));
        }

        [TestMethod]
        public void testUnsuccesfulRefund() {
            Charge charge = testSuccesfulCardPMCreate();
            try {
                charge.refund(30000);
            }
            catch (ProcessingError e)
            {
                Assert.IsNotNull(e);
            }
        }

        [TestMethod]
        public void testSuccesfulCapture() {
            JObject _params = valid_payment_method;
            _params["card"] = valid_visa_card["card"];
            _params["capture"] = "false";
            Charge charge = Charge.create(_params);
            Assert.IsTrue(charge.status.Equals("pre_authorized"));
            charge.capture();
            Assert.IsTrue(charge.status.Equals("paid"));
        }
    }
}
