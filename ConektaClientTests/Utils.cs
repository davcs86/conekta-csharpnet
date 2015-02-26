using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConektaCSharp;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    [TestClass]
    public class Utils
    {
        [TestMethod]
        public void ProbarLoadFromObject()
        {
            var nuevo = new Charge();
            var nuevoObj = (ConektaObject)nuevo;
            JObject jsonObj = JObject.Parse("{\"id\":\"54ed6d1219ce88ac7b0028e6\",\"livemode\":false,\"created_at\":1424846098,\"status\":\"paid\",\"currency\":\"MXN\",\"description\":\"Stogies\",\"reference_id\":\"9839-wolf_pack\",\"failure_code\":null,\"failure_message\":null,\"monthly_installments\":null,\"object\":\"charge\",\"amount\":20000,\"paid_at\":1424846102,\"fee\":963,\"customer_id\":\"\",\"refunds\":[],\"payment_method\":{\"name\":\"Jorge Lopez\",\"exp_month\":\"12\",\"exp_year\":\"19\",\"auth_code\":\"000000\",\"object\":\"card_payment\",\"type\":\"credit\",\"last4\":\"4242\",\"brand\":\"visa\",\"issuer\":\"\",\"account_type\":\"\",\"country\":\"US\",\"fraud_score\":29,\"fraud_indicators\":[{\"description\":\"En las \u00FAltimas 6 horas la persona ha intentado realizar compras con m\u00E1s de una tarjeta distinta; por seguridad se ha bloqueado la compra.\",\"weight\":null},{\"description\":\"El dispositivo con el que se hizo la compra se encuentra fuera de M\u00E9xico y la persona tiene asociada a ella distintas tarjetas, correos, direcciones IP, etc. Este patr\u00F3n es muy com\u00FAn en ataques; por seguridad se ha bloqueado la compra.\",\"weight\":null},{\"description\":\"La transacci\u00F3n se ha aprobado a trav\u00E9s de un pago v\u00EDa la modalidad Paga M\u00F3vil de Banorte.\",\"weight\":null}]},\"details\":{\"name\":\"Wolverine\",\"phone\":null,\"email\":null,\"line_items\":[{\"name\":\"Box of Cohiba S1s\",\"description\":\"Imported from Mex.\",\"unit_price\":20000,\"quantity\":1,\"sku\":\"cohb_s1\",\"category\":\"other_human_consumption\"}],\"billing_address\":{\"street1\":\"tamesis\",\"street2\":null,\"street3\":null,\"city\":null,\"state\":null,\"zip\":null,\"country\":\"MX\",\"tax_id\":null,\"company_name\":null,\"phone\":null,\"email\":null}}}");
            nuevoObj.LoadFromObject(jsonObj);
            Assert.IsTrue(nuevoObj is Charge, "LoadFromObject funciona correcto");
        }
    }
}
