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

        [TestMethod]
        public void testSuccesfulCustomerFind() {
            Customer customer = testSuccesfulCustomerCreate();
            customer = Customer.find(customer.id);
            Assert.IsNotNull(customer);
        }

        public Customer testSuccesfulCustomerCreate() {
            Customer customer = Customer.create(valid_visa_card);
            Assert.IsNotNull(customer);
            Assert.IsTrue(customer.cards[0] is Card);
            Assert.IsTrue(((Card)customer.cards[0]).last4.Equals("4242"));
            Assert.IsTrue(((Card)customer.cards[0]).customer.Equals(customer));
            return customer;
        }

        [TestMethod]
        public void testSuccesfulCustomerWhere() {
            ConektaObject customers = Customer.where();
            Assert.IsNotNull(customers);
            Assert.IsTrue(customers[0] is Customer);
        }

        [TestMethod]
        public void testSuccesfulDeleteCustomer() {
            Customer customer = testSuccesfulCustomerCreate();
            customer.delete();
            Assert.IsTrue(customer.deleted);
        }

        [TestMethod]
        public void testSuccesfulCustomerUpdate() {
            Customer customer = testSuccesfulCustomerCreate();
            JObject _params = JObject.Parse("{'name':'Logan', 'email':'logan@x-men.org'}");
            customer.update(_params);
            Assert.IsTrue(customer.name.Equals("Logan"));
        }

        [TestMethod]
        public void testAddCardToCustomer() {
            Customer customer = testSuccesfulCustomerCreate();
            JObject _params = JObject.Parse("{'token':'tok_test_visa_1881'}");
            customer.createCard(_params);
            Assert.IsTrue(((Card)customer.cards[0]).last4.Equals("4242"));
            Assert.IsTrue(((Card)customer.cards[1]).last4.Equals("1881"));
            Assert.IsTrue(((Card) customer.cards[1]).customer ==  customer);
        }

        [TestMethod]
        public void testDeleteCard() {
            Customer customer = testSuccesfulCustomerCreate();
            ((Card) customer.cards[0]).delete();
            Assert.IsTrue(((Card)customer.cards[0]).deleted);
        }

        [TestMethod]
        public void testUpdateCard() {
            Customer customer = testSuccesfulCustomerCreate();
            JObject _params = JObject.Parse("{'token':'tok_test_mastercard_4444', 'active':false}");
            ((Card) customer.cards[0]).update(_params);
            Assert.IsTrue(((Card) customer.cards[0]).last4.Equals("4444"));
            Assert.IsTrue(((Card) customer.cards[0]).customer ==  customer);
        }
    
        public Customer testSuccesfulSubscriptionCreate() {
            Customer customer = testSuccesfulCustomerCreate();
            JObject _params = JObject.Parse("{'plan':'gold-plan'}");
            customer.createSubscription(_params);
            Assert.IsNotNull(customer.subscription);
            Assert.IsTrue(customer.subscription.status.Equals("in_trial"));
            Assert.IsTrue(customer.subscription.plan_id.Equals("gold-plan"));
            Assert.IsTrue(customer.subscription.card_id.Equals(customer.default_card_id));
            return customer;
        }

        [TestMethod]
        public void testSuccesfulSubscriptionUpdate() {
            Customer customer = testSuccesfulSubscriptionCreate();
            Plan plan = null;
            try {
                plan = Plan.find("gold-plan2");
            } catch(Exception e) {
                JObject _params = JObject.Parse("{'id':'gold-plan2','name':'Gold plan', 'amount':1000, 'currency':'MXN','interval':'month','frequency':1,'trial_period_days':15,'expiry_count':12}");
                plan = Plan.create(_params);
            }
            customer.subscription.update(JObject.Parse(("{'plan':'"+plan.id+"'}")));
            Assert.IsTrue(customer.subscription.plan_id.Equals(plan.id));
        }

        [TestMethod]
        public void testUnSuccesfulSubscriptionCreate() {
            Customer customer = testSuccesfulCustomerCreate();
            JObject _params = JObject.Parse("{'plan':'unexistent-plan'}");
            try {
                customer.createSubscription(_params);
                Assert.IsTrue(false);
            }
            catch (ResourceNotFoundError e)
            {
                Assert.IsNotNull(e);
            }
        }

        [TestMethod]
        public void testSuccesfulSubscriptionPause() {
            Customer customer = testSuccesfulSubscriptionCreate();
            customer.subscription.pause();
            Assert.IsTrue(customer.subscription.status.Equals("paused"));
        }

        [TestMethod]
        public void testSuccesfulSubscriptionResume() {
            Customer customer = testSuccesfulSubscriptionCreate();
            customer.subscription.pause();
            Assert.IsTrue(customer.subscription.status.Equals("paused"));
            customer.subscription.resume();
            Assert.IsTrue(customer.subscription.status.Equals("active"));
        }

        [TestMethod]
        public void testSuccesfulSubscriptionCancel() {
            Customer customer = testSuccesfulSubscriptionCreate();
            customer.subscription.cancel();
            Assert.IsTrue(customer.subscription.status.Equals("canceled"));
        }

    }
}
