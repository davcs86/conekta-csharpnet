using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Resource:ConektaObject
    {
        public Resource(String id = null):base(id) {
        }

        public static String classUrl(String className) {
            try { 
                String _base = "/" + className.ToLower() + "s";
                return _base;
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public virtual String instanceUrl() {
            try { 
                if (string.IsNullOrEmpty(id)) {
                    throw new Error("Could not get the id of Resource instance.");
                }
                String className = GetType().Name;
                String _base = classUrl(className);
                return _base + "/" + id;
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        protected static ConektaObject scpFind(String className, String id){
            try { 
                //Constructor c;
                ConektaObject resource;
                try {
                    ObjectHandle handle = Activator.CreateInstance(null, "ConektaCSharp."+toCamelCase(className));
                    resource = (ConektaObject)handle.Unwrap();
                    resource.SetId(id);
                } catch (Exception e) {
                    throw new Error(e.ToString());
                }
                ConektaRequestor requestor = new ConektaRequestor();
                String url = ((Resource) resource).instanceUrl();
                JObject jsonObject = (JObject) requestor.Request("GET", url, null);
                try {
                    resource.LoadFromObject(jsonObject);
                } catch (Exception e) {
                    throw new Error(e.ToString());
                }
                return resource;
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        protected static ConektaObject scpCreate(String className, JObject _params) {
            ConektaRequestor requestor = new ConektaRequestor();
            String url = classUrl(className);
            JObject jsonObject = (JObject) requestor.Request("POST", url, _params);
            ConektaObject resource;
            try {
                ObjectHandle handle = Activator.CreateInstance(null, "ConektaCSharp."+toCamelCase(className));
                resource = (ConektaObject)handle.Unwrap();
                resource.LoadFromObject(jsonObject);
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
            return resource;
        }

        protected static ConektaObject scpWhere(String className, JObject _params) {
            ConektaRequestor requestor = new ConektaRequestor();
            String url = classUrl(className);
            JArray jsonArray = (JArray) requestor.Request("GET", url, _params);
            ConektaObject resource = new ConektaObject();
            resource.LoadFromArray(jsonArray);
            return resource;
        }

        protected ConektaObject delete(String parent, String member) {
            customAction("DELETE", null, null);
            return this;
        }

        protected void update(JObject _params) {
            ConektaRequestor requestor = new ConektaRequestor();
            String url = this.instanceUrl();
            JObject jsonObject = (JObject) requestor.Request("PUT", url, _params);
            try {
                LoadFromObject(jsonObject);
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
        }

        protected ConektaObject customAction(String method, String action, JObject _params) {
            if (method == null) {
                method = "POST";
            }
            ConektaRequestor requestor = new ConektaRequestor();
            String url = this.instanceUrl();
            if (action != null) {
                url = url + "/" + action;
            }
            JObject jsonObject = (JObject) requestor.Request(method, url, _params);
            try {
                this.LoadFromObject(jsonObject);
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
            return this;
        }

        protected ConektaObject createMember(String member, JObject _params) {
            ConektaRequestor requestor = new ConektaRequestor();
            String url = this.instanceUrl() + "/" + member;
            JObject jsonObject = (JObject) requestor.Request("POST", url, _params);
            FieldInfo field;
            ConektaObject conektaObject = null;
            field = GetType().GetField(member);
            String className;
            String parentClassName = this.GetType().Name.Substring(0, 1).ToLower() + this.GetType().Name.Substring(1);
            if (field.GetValue(this).GetType().Name.Equals("ConektaObject"))
            {
                className = member.Substring(0, 1).ToUpper() + member.Substring(1, member.Length - 2).ToLower();
                ObjectHandle handle = Activator.CreateInstance(null, "ConektaCSharp." + className);
                conektaObject = (ConektaObject)handle.Unwrap();
                conektaObject.LoadFromObject(jsonObject);

                conektaObject.GetType().GetField(parentClassName).SetValue(conektaObject, this);

                ConektaObject objects = ((ConektaObject) field.GetValue(this));
                objects.Add(conektaObject);
                field.SetValue(this, objects);
            } else {
                className = member.Substring(0, 1).ToUpper() + member.Substring(1).ToLower();
                ObjectHandle handle = Activator.CreateInstance(null, "ConektaCSharp." + className);
                conektaObject = (ConektaObject)handle.Unwrap();

                conektaObject.LoadFromObject(jsonObject);
                conektaObject.GetType().GetField(parentClassName).SetValue(conektaObject, this);

                this.SetVal(member, conektaObject);
                field.SetValue(this, conektaObject);
                this.LoadFromObject(null);
            }
            return conektaObject;
        }
    }
}
