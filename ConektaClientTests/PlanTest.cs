using System;
using ConektaCSharp;
using NUnit.Framework;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    /// <summary>
    ///     Summary description for PlanTest
    /// </summary>
    [TestFixture]
    public class PlanTest
    {
        private readonly JObject _params;
        private readonly int id;

        public PlanTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
            var plans = Plan.where();
            id = (new Random()).Next(1000);
            _params =
                JObject.Parse("{'id' : 'gold-plan" + id +
                              "','name' : 'Gold Plan','amount' : 10000,'currency' : 'MXN','interval' : 'month','frequency' : 10,'trial_period_days' : 15,'expiry_count' : 12}");
        }

        [Test]
        public void testSuccesfulPlanCreate()
        {
            var plans = Plan.where();
            var plan = Plan.create(_params);
            Assert.IsTrue(plan.id.Equals("gold-plan" + id));
        }

        [Test]
        public void testUpdatePlan()
        {
            var plans = Plan.where();
            var plan = Plan.create(_params);
            plan.update(JObject.Parse("{'name':'Silver Plan'}"));
            Assert.IsTrue(plan.name.Equals("Silver Plan"));
        }

        [Test]
        public void testDeletePlan()
        {
            var plans = Plan.where();
            var plan = Plan.create(_params);
            plan.delete();
            Assert.IsTrue(plan.deleted);
        }
    }
}