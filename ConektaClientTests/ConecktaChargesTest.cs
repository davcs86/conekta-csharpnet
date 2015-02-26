using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConektaCSharp;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    public class PaymentTest {
        private readonly Double _amount;
        private readonly String _currency;
        private readonly String _description;
        public PaymentTest(Double amount, String currency, String description)
        {
            this._amount = amount;
            this._currency = currency;
            this._description = description;
        }
        public Double GetAmount()
        {
            return this._amount;
        }
        public String GetCurrency()
        {
            return this._currency;
        }
        public String GetDescription()
        {
            return this._description;
        }
    }
    public class CardTest
    {
        private readonly String _card;
        public CardTest(String card)
        {
            this._card = card;
        }
        public String GetCardToken()
        {
            return this._card;
        }
    }
    [TestClass]
    public class ConektaChargesTest
    {
        JObject valid_payment_method;
        JObject invalid_payment_method;
        JObject valid_visa_card;
        JObject _params;

        public Charge testSuccesfulCardPMCreate() {
            valid_payment_method.Add("card",valid_visa_card.GetValue("card"));
            Charge charge = Charge.create(valid_payment_method);
            Assert.IsTrue(charge.status.Equals("paid"),"Cargo con estatus pagado");
            Assert.IsNotNull(charge.details, "Detalles del cargo");
            Assert.IsNotNull(charge.details.billing_address, "Dirección es correcta");
            Assert.IsNotNull(charge.details.line_items, "Items es ConektaObject");
            Assert.IsTrue(charge.details.line_items[0] is LineItems, "Items[0] es LineItems");
            Assert.IsTrue(charge.details.name.Equals("Wolverine"),"Nombre en Detalles es correcto");
            Assert.IsTrue(charge.payment_method is CardPayment, "Método de pago fue tarjeta");
            return charge;
        }


        [TestMethod]
        public void TestSuccesfulFindCharge()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
            valid_payment_method = JObject.Parse("{'description':'Stogies'," +
                "'reference_id':'9839-wolf_pack'," +
                "'amount':20000," +
                "'currency':'MXN'," +
                "'details':{'name':'Wolverine', 'billing_address': {'street1':'tamesis'}, line_items: [{'name':'Box of Cohiba S1s', 'sku':'cohb_s1','unit_price': 20000,'description':'Imported from Mex.','quantity':1,'type':'other_human_consumption'}]}" +
                "}");
            valid_visa_card = JObject.Parse("{'card':'tok_test_visa_4242'}");
            _params = JObject.Parse("{" +
            "    'currency': 'MXN',"+
            "    'details': {"+
            "        'billing_address': {"+
            "            'zip': '53422',"+
            "            'state': 'Estado de Mexico',"+
            "            'country': 'MX',"+
            "            'city': 'Naucalpan',"+
            "            'email': 'oscar.jimenez+loctommy@edgebound.com',"+
            "            'phone': '5514945290',"+
            "            'street1': 'Los Remedios 17, Loma Colorada 2a Seccion'"+
            "        },"+
            "        'line_items': ["+
            "            {"+
            "                'type': 'ecommerce_shopping',"+
            "                'quantity': 2,"+
            "                'name': 'Bg Ss Shoshanna Yd Polo Dress',"+
            "                'description': 'Un vestido que podría ser definido como sporty- chic. De cuello blanco y líneas horizontales en varios colores, hace referencia a la vida en altamar.  ',"+
            "                'sku': '7500244741187',"+
            "                'unit_price': 59000"+
            "            },"+
            "            {"+
            "                'type': 'ecommerce_shopping',"+
            "                'quantity': 1,"+
            "                'name': 'Farris Chino Gmd St',"+
            "                'description': 'Producto Tommy',"+
            "                'sku': '8718771353202',"+
            "                'unit_price': 69000"+
            "            }"+
            "        ],"+
            "        'name': 'Oscar',"+
            "        'email': 'oscar.jimenez+loctommy@edgebound.com',"+
            "        'phone': '5514945290'"+
            "    },"+
            "    'amount': 187000,"+
            "    'bank': {"+
            "        'type': 'banorte'"+
            "    },"+
            "    'description': 'Stogies',"+
            "    'reference_id': 'conekta.tommy_12014_11019'"+
            "}");
            
            var charge = testSuccesfulCardPMCreate();
            var nuevoCharge = Charge.find(charge.GetId());
            Assert.IsNotNull(charge, "Conekta charge found correctly");
            Assert.IsTrue(nuevoCharge.GetId()==charge.GetId(), "Conekta charge equals");
        }
    }
}
