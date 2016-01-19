using System;
using ConektaCSharp;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    [TestFixture]
    public class CustomerTest
    {
        private readonly JObject valid_visa_card;

		public void setApiKey() {
			string apiFromEnvironment = Environment.GetEnvironmentVariable("CONEKTA_APIKEY");
			if (string.IsNullOrWhiteSpace(apiFromEnvironment))
				apiFromEnvironment = "key_eYvWV7gSDkNYXsmr"; // use your public key
			Conekta.ApiKey = apiFromEnvironment;
		}

        public CustomerTest()
        {
			setApiKey();
            valid_visa_card = JObject.Parse("{'name': 'test', 'cards':['tok_test_visa_4242']}");
        }

        [Test]
        public void testSuccesfulCustomerFind()
        {
            var customer = testSuccesfulCustomerCreate();
            customer = Customer.find(customer.id);
            Assert.IsNotNull(customer);
        }

        public Customer testSuccesfulCustomerCreate()
        {
            var customer = Customer.create(valid_visa_card);
            Assert.IsNotNull(customer);
            Assert.IsTrue(customer.cards[0] is Card);
            Assert.IsTrue(((Card) customer.cards[0]).last4.Equals("4242"));
            Assert.IsTrue(((Card) customer.cards[0]).customer.Equals(customer));
            return customer;
        }

        [Test]
        public void testSuccesfulCustomerWhere()
        {
            var customers = Customer.where();
            Assert.IsNotNull(customers);
            Assert.IsTrue(customers[0] is Customer);
        }

        [Test]
        public void testSuccesfulDeleteCustomer()
        {
            var customer = testSuccesfulCustomerCreate();
            customer.delete();
            Assert.IsTrue(customer.deleted);
        }

        [Test]
        public void testSuccesfulCustomerUpdate()
        {
            var customer = testSuccesfulCustomerCreate();
            var _params = JObject.Parse("{'name':'Logan', 'email':'logan@x-men.org'}");
            customer.update(_params);
            Assert.IsTrue(customer.name.Equals("Logan"));
        }

        [Test]
        public void testAddCardToCustomer()
        {
            var customer = testSuccesfulCustomerCreate();
            var _params = JObject.Parse("{'token':'tok_test_visa_1881'}");
            customer.createCard(_params);
            Assert.IsTrue(((Card) customer.cards[0]).last4.Equals("4242"));
            Assert.IsTrue(((Card) customer.cards[1]).last4.Equals("1881"));
            Assert.IsTrue(((Card) customer.cards[1]).customer == customer);
        }

        [Test]
        public void testDeleteCard()
        {
            var customer = testSuccesfulCustomerCreate();
            ((Card) customer.cards[0]).delete();
            Assert.IsTrue(((Card) customer.cards[0]).deleted);
        }

        [Test]
        public void testUpdateCard()
        {
            var customer = testSuccesfulCustomerCreate();
            var _params = JObject.Parse("{'token':'tok_test_mastercard_4444', 'active':false}");
            ((Card) customer.cards[0]).update(_params);
            Assert.IsTrue(((Card) customer.cards[0]).last4.Equals("4444"));
            Assert.IsTrue(((Card) customer.cards[0]).customer == customer);
        }

        public Customer testSuccesfulSubscriptionCreate()
        {
            var customer = testSuccesfulCustomerCreate();
			var planId = (new Random()).Next(1000);
			var _params =
				JObject.Parse("{'id' : 'gold-plan" + planId +
					"','name' : 'Gold Plan','amount' : 10000,'currency' : 'MXN','interval' : 'month','frequency' : 10,'trial_period_days' : 15,'expiry_count' : 12}");
			var plan = Plan.create(_params);
			_params = JObject.Parse("{'plan':'"+plan.id+"'}");
            customer.createSubscription(_params);
            Assert.IsNotNull(customer.subscription);
            Assert.IsTrue(customer.subscription.status.Equals("in_trial"));
			Assert.IsTrue(customer.subscription.plan_id.Equals(plan.id));
            Assert.IsTrue(customer.subscription.card_id.Equals(customer.default_card_id));
            return customer;
        }

        [Test]
        public void testSuccesfulSubscriptionUpdate()
        {
            var customer = testSuccesfulSubscriptionCreate();
            Plan plan = null;
            try
            {
                plan = Plan.find("gold-plan2");
            }
            catch
            {
                var _params =
                    JObject.Parse(
                        "{'id':'gold-plan2','name':'Gold plan', 'amount':1000, 'currency':'MXN','interval':'month','frequency':1,'trial_period_days':15,'expiry_count':12}");
                plan = Plan.create(_params);
            }
            customer.subscription.update(JObject.Parse(("{'plan':'" + plan.id + "'}")));
            Assert.IsTrue(customer.subscription.plan_id.Equals(plan.id));
        }

        [Test]
        public void testUnSuccesfulSubscriptionCreate()
        {
            var customer = testSuccesfulCustomerCreate();
            var _params = JObject.Parse("{'plan':'unexistent-plan'}");
            try
            {
                customer.createSubscription(_params);
                Assert.IsTrue(false);
            }
            catch (ResourceNotFoundError e)
            {
                Assert.IsNotNull(e);
            }
        }

        [Test]
        public void testSuccesfulSubscriptionPause()
        {
            var customer = testSuccesfulSubscriptionCreate();
            customer.subscription.pause();
            Assert.IsTrue(customer.subscription.status.Equals("paused"));
        }

        [Test]
        public void testSuccesfulSubscriptionResume()
        {
            var customer = testSuccesfulSubscriptionCreate();
			var initialStatus = customer.subscription.status;
            customer.subscription.pause();
            Assert.IsTrue(customer.subscription.status.Equals("paused"));
            customer.subscription.resume();
			Assert.IsTrue(customer.subscription.status.Equals(initialStatus));
        }

        [Test]
        public void testSuccesfulSubscriptionCancel()
        {
            var customer = testSuccesfulSubscriptionCreate();
            customer.subscription.cancel();
            Assert.IsTrue(customer.subscription.status.Equals("canceled"));
        }
    }
}