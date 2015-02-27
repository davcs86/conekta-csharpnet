using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConektaCSharp;
using Newtonsoft.Json.Linq;

namespace ConektaCSharpTests
{
    /// <summary>
    /// Summary description for PlanTest
    /// </summary>
    [TestClass]
    public class PlanTest
    {
        JObject _params;
        int id;

        public PlanTest()
        {
            Conekta.ApiKey = "key_eYvWV7gSDkNYXsmr";
            ConektaObject plans = Plan.where();
            id = (new Random()).Next(1000);
            _params = JObject.Parse("{'id' : 'gold-plan"+ id +"','name' : 'Gold Plan','amount' : 10000,'currency' : 'MXN','interval' : 'month','frequency' : 10,'trial_period_days' : 15,'expiry_count' : 12}");
        }

        [TestMethod]
        public void testSuccesfulPlanCreate() {
           ConektaObject plans = Plan.where();
           Plan plan = Plan.create(_params);
           Assert.IsTrue(plan.id.Equals("gold-plan"+id));
        }

        [TestMethod]
        public void testUpdatePlan() {
           ConektaObject plans = Plan.where();
           Plan plan = Plan.create(_params);
           plan.update(JObject.Parse("{'name':'Silver Plan'}"));
           Assert.IsTrue(plan.name.Equals("Silver Plan"));
        }

        [TestMethod]
        public void testDeletePlan() {
           ConektaObject plans = Plan.where();
           Plan plan = Plan.create(_params);
           plan.delete();
           Assert.IsTrue(plan.deleted);
        }
    }
}
