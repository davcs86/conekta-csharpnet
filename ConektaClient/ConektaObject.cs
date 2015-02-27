using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{

    public class ConektaObject : ArrayList
    {
        protected Dictionary<string, object> values;
        public String id;

        public ConektaObject(String _id = null)
        {
            id = _id;
            values = new Dictionary<string, object>();
        }

        public ConektaObject()
        {
            id = "";
            values = new Dictionary<string, object>();
        }

        public String GetId()
        {
            return id;
        }

        public void SetId(String _id)
        {
            id = _id;
        }

        public Object GetVal(String key)
        {
            return (key==null)?null:values[key];
        }

        public void SetVal(String key, Object value)
        {
            if (key == null) return;
            if (values.ContainsKey(key))
            {
                values[key] = value;
            }
            else
            {
                values.Add(key, value);
            }
        }

        public void LoadFromArray(JArray jsonArray){
            try{
                LoadFromArray(jsonArray, null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void LoadFromArray(JArray jsonArray, String className) {
            try{
                for (var i = 0; i < jsonArray.Count; i++) {
                    ConektaObject conektaObject = aux(jsonArray, className, i);
                
                    Add(conektaObject);
                }
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        private static ConektaObject aux(JArray jsonArray, String className, int i)
        {
            try { 
                String key;
                JObject jsonObject;
                key = className ?? jsonArray[i]["object"].ToString();
                jsonObject = jsonArray[i].ToObject<JObject>();
                ConektaObject conektaObject = ConektaObjectFromJSONFactory.ConektaObjectFactory(jsonObject, key);
                conektaObject.LoadFromObject(jsonObject);
                return conektaObject;
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        public virtual void LoadFromObject(JObject jsonObject)
        {
            try { 
                IEnumerator itr = jsonObject.Properties().GetEnumerator();
                FieldInfo field;
                while (itr.MoveNext()) {
                    String key = ((JProperty)itr.Current).Name;
                    field = this.GetType().GetField(key);
                    // Si el campo no existe, omitir
                    if (field==null) continue;
                    Object obj = jsonObject[key];
                    try {
                        Boolean isConektaObject = (field.FieldType.Namespace ?? "").Equals("ConektaCSharp");

                        String objStr = obj.ToString();

                        if (!String.IsNullOrWhiteSpace(objStr))
                        {
                            switch ((int)objStr[0])
                            {
                                // {
                                case 123:
                                    var o = JObject.Parse(objStr);
                                    if (o["object"] != null)
                                    {
                                        var conektaObject = ConektaObjectFromJSONFactory.ConektaObjectFactory(o, o["object"].ToString());
                                        field.SetValue(this, conektaObject);
                                        this.SetVal(key, conektaObject);
                                    }
                                    else
                                    {
                                        if (isConektaObject)
                                        {

                                            ObjectHandle handle = Activator.CreateInstance(null, field.FieldType.FullName);
                                            var attr = (ConektaObject)handle.Unwrap();

                                            attr.LoadFromObject((JObject)obj);
                                            field.SetValue(this, attr);
                                            this.SetVal(key, attr);
                                        }
                                        else
                                        {
                                            Type tipoCampo = Type.GetType(field.FieldType.FullName);
                                            var valorConvertido = Convert.ChangeType(obj, tipoCampo);
                                            field.SetValue(this, valorConvertido);
                                            this.SetVal(key, valorConvertido);
                                        }
                                    }

                                    break;
                                // [
                                case 91:
                                    JArray jsonArray = JArray.Parse(objStr);
                                    if (jsonArray.Count > 0)
                                    {
                                        var conektaObject = new ConektaObject();
                                        if (isConektaObject)
                                        {
                                            ObjectHandle handle = Activator.CreateInstance(null, field.FieldType.FullName);
                                            conektaObject = (ConektaObject)handle.Unwrap();
                                        }
                                    
                                        for (int i = 0; i < jsonArray.Count; i++)
                                        {
                                            if (jsonArray[0]["object"] != null)
                                            {
                                                conektaObject.Add(ConektaObjectFromJSONFactory.ConektaObjectFactory(jsonArray[i].ToObject<JObject>(), jsonArray[i]["object"].ToString()));
                                            }
                                            else
                                            {
                                                conektaObject.Add(ConektaObjectFromJSONFactory.ConektaObjectFactory(jsonArray[i].ToObject<JObject>(), key));
                                            }
                                        }
                                        field.SetValue(this, conektaObject);
                                        this.SetVal(key, conektaObject);
                                    }
                                    break;
                                default:
                                    if (isConektaObject)
                                    {

                                        ObjectHandle handle = Activator.CreateInstance(null, field.FieldType.FullName);
                                        var attr = (ConektaObject)handle.Unwrap();

                                        attr.LoadFromObject((JObject)obj);
                                        field.SetValue(this, attr);
                                        this.SetVal(key, attr);
                                    }
                                    else
                                    {
                                        Type tipoCampo = Type.GetType(field.FieldType.FullName);
                                        var valorConvertido = Convert.ChangeType(obj, tipoCampo);
                                        field.SetValue(this, valorConvertido);
                                        this.SetVal(key, valorConvertido);
                                    }
                                    break;
                            }
                        }
                    
                    } catch (Exception e) {
                        // No field found
                        //System.out.println(e.toString());
                        throw new Error(e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        public override String ToString()
        {
            return JObject.FromObject(this).ToString();
        }

        public static String toCamelCase(String s) {
            String[] parts = s.Split('_');
            return parts.Aggregate("", (current, part) => current + toProperCase(part));
        }

        static String toProperCase(String s) {
            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

    }
}
