using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Resource : ConektaObject
    {
        public Resource(String id = null) : base(id)
        {
        }

        public static String classUrl(String className)
        {
            return "/" + className.ToLower() + "s";
        }

        public virtual String instanceUrl()
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Error("Could not get the id of Resource instance.");
            }
            var className = GetType().Name;
            var _base = classUrl(className);
            return _base + "/" + id;
        }

        protected static ConektaObject scpFind(String className, String id)
        {
            ConektaObject resource;
            try
            {
                var handle = Activator.CreateInstance(null, "ConektaCSharp." + toCamelCase(className));
                resource = (ConektaObject) handle.Unwrap();
                resource.SetId(id);
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
            var requestor = new ConektaRequestor();
            var url = ((Resource) resource).instanceUrl();
            var jsonObject = (JObject) requestor.Request("GET", url, null);
            try
            {
                resource.LoadFromObject(jsonObject);
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
            return resource;
        }

        protected static ConektaObject scpCreate(String className, JObject _params)
        {
            var requestor = new ConektaRequestor();
            var url = classUrl(className);
            var jsonObject = (JObject) requestor.Request("POST", url, _params);
            ConektaObject resource;
            try
            {
                var handle = Activator.CreateInstance(null, "ConektaCSharp." + toCamelCase(className));
                resource = (ConektaObject) handle.Unwrap();
                resource.LoadFromObject(jsonObject);
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
            return resource;
        }

        protected static ConektaObject scpWhere(String className, JObject _params)
        {
            var requestor = new ConektaRequestor();
            var url = classUrl(className);
            var jsonArray = (JArray) requestor.Request("GET", url, _params);
            var resource = new ConektaObject();
            resource.LoadFromArray(jsonArray);
            return resource;
        }

        protected ConektaObject delete(String parent, String member)
        {
            customAction("DELETE", null, null);
            return this;
        }

        protected void update(JObject _params)
        {
            var requestor = new ConektaRequestor();
            var url = instanceUrl();
            var jsonObject = (JObject) requestor.Request("PUT", url, _params);
            try
            {
                LoadFromObject(jsonObject);
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
        }

        protected ConektaObject customAction(String method, String action, JObject _params)
        {
            if (method == null)
            {
                method = "POST";
            }
            var requestor = new ConektaRequestor();
            var url = instanceUrl();
            if (action != null)
            {
                url = url + "/" + action;
            }
            var jsonObject = (JObject) requestor.Request(method, url, _params);
            try
            {
                LoadFromObject(jsonObject);
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
            return this;
        }

        protected ConektaObject createMember(String member, JObject _params)
        {
            var requestor = new ConektaRequestor();
            var url = instanceUrl() + "/" + member;
            var jsonObject = (JObject) requestor.Request("POST", url, _params);
            FieldInfo field;
            ConektaObject conektaObject = null;
            field = GetType().GetField(member);
            String className;
            var parentClassName = GetType().Name.Substring(0, 1).ToLower() + GetType().Name.Substring(1);
            if (field.GetValue(this).GetType().Name.Equals("ConektaObject"))
            {
                className = member.Substring(0, 1).ToUpper() + member.Substring(1, member.Length - 2).ToLower();
                var handle = Activator.CreateInstance(null, "ConektaCSharp." + className);
                conektaObject = (ConektaObject) handle.Unwrap();
                conektaObject.LoadFromObject(jsonObject);

                conektaObject.GetType().GetField(parentClassName).SetValue(conektaObject, this);

                var objects = ((ConektaObject) field.GetValue(this));
                objects.Add(conektaObject);
                field.SetValue(this, objects);
            }
            else
            {
                className = member.Substring(0, 1).ToUpper() + member.Substring(1).ToLower();
                var handle = Activator.CreateInstance(null, "ConektaCSharp." + className);
                conektaObject = (ConektaObject) handle.Unwrap();

                conektaObject.LoadFromObject(jsonObject);
                conektaObject.GetType().GetField(parentClassName).SetValue(conektaObject, this);

                SetVal(member, conektaObject);
                field.SetValue(this, conektaObject);
                LoadFromObject(null);
            }
            return conektaObject;
        }
    }
}