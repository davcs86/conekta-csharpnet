using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class ConektaObject : ArrayList
    {
        protected Dictionary<string, object> _values;
        public String id;

        public ConektaObject(String _id = null)
        {
            id = _id;
            _values = new Dictionary<string, object>();
        }

        public ConektaObject()
        {
            id = "";
            _values = new Dictionary<string, object>();
        }

        public Dictionary<string, object> values
        {
            get
            {
                var nvalues = new Dictionary<String, Object>();

                foreach (var item in _values)
                {
                    var o = item.Value as ConektaObject;
                    nvalues.Add(item.Key, (o != null) ? o._values : item.Value);
                }

                return nvalues;
            }
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
            return (key == null) ? null : _values[key];
        }

        public void SetVal(String key, Object value)
        {
            if (key == null) return;
            if (_values.ContainsKey(key))
            {
                _values[key] = value;
            }
            else
            {
                _values.Add(key, value);
            }
        }

        public void LoadFromArray(JArray jsonArray)
        {
            try
            {
                LoadFromArray(jsonArray, null);
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
        }

        public void LoadFromArray(JArray jsonArray, String className)
        {
            try
            {
                for (var i = 0; i < jsonArray.Count; i++)
                {
                    var conektaObject = aux(jsonArray, className, i);

                    Add(conektaObject);
                }
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
        }

        private static ConektaObject aux(JArray jsonArray, String className, int i)
        {
            try
            {
                String key;
                JObject jsonObject;
                key = className ?? jsonArray[i]["object"].ToString();
                jsonObject = jsonArray[i].ToObject<JObject>();
                var conektaObject = ConektaObjectFromJSONFactory.ConektaObjectFactory(jsonObject, key);
                conektaObject.LoadFromObject(jsonObject);
                return conektaObject;
            }
            catch (Exception ex)
            {
                throw new Error(ex.Message);
            }
        }

        public virtual void LoadFromObject(JObject jsonObject)
        {
            IEnumerator itr = jsonObject.Properties().GetEnumerator();
            FieldInfo field;
            while (itr.MoveNext())
            {
                var key = ((JProperty) itr.Current).Name;
                field = GetType().GetField(key);
                // Si el campo no existe, omitir
                if (field == null) continue;
                Object obj = jsonObject[key];
                try
                {
                    var isConektaObject = (field.FieldType.Namespace ?? "").Equals("ConektaCSharp");

                    var objStr = obj.ToString();

                    if (!String.IsNullOrWhiteSpace(objStr))
                    {
                        switch ((int) objStr[0])
                        {
                            // {
                            case 123:
                                var o = JObject.Parse(objStr);
                                if (o["object"] != null)
                                {
                                    var conektaObject = ConektaObjectFromJSONFactory.ConektaObjectFactory(o,
                                        o["object"].ToString());
                                    field.SetValue(this, conektaObject);
                                    SetVal(key, conektaObject);
                                }
                                else
                                {
                                    if (isConektaObject)
                                    {
                                        var handle = Activator.CreateInstance(null, field.FieldType.FullName);
                                        var attr = (ConektaObject) handle.Unwrap();

                                        attr.LoadFromObject((JObject) obj);
                                        field.SetValue(this, attr);
                                        SetVal(key, attr);
                                    }
                                    else
                                    {
                                        var tipoCampo = Type.GetType(field.FieldType.FullName);
                                        var valorConvertido = Convert.ChangeType(obj, tipoCampo);
                                        field.SetValue(this, valorConvertido);
                                        SetVal(key, valorConvertido);
                                    }
                                }

                                break;
                            // [
                            case 91:
                                var jsonArray = JArray.Parse(objStr);
                                if (jsonArray.Count > 0)
                                {
                                    var conektaObject = new ConektaObject();
                                    if (isConektaObject)
                                    {
                                        var handle = Activator.CreateInstance(null, field.FieldType.FullName);
                                        conektaObject = (ConektaObject) handle.Unwrap();
                                    }

                                    foreach (var jItem in jsonArray)
                                    {
                                        if (jsonArray[0]["object"] != null)
                                        {
                                            conektaObject.Add(
                                                ConektaObjectFromJSONFactory.ConektaObjectFactory(
                                                    jItem.ToObject<JObject>(),
                                                    jItem["object"].ToString()));
                                        }
                                        else
                                        {
                                            conektaObject.Add(
                                                ConektaObjectFromJSONFactory.ConektaObjectFactory(
                                                    jItem.ToObject<JObject>(), key));
                                        }
                                    }
                                    field.SetValue(this, conektaObject);
                                    SetVal(key, conektaObject);
                                }
                                break;
                            default:
                                if (isConektaObject)
                                {
                                    var handle = Activator.CreateInstance(null, field.FieldType.FullName);
                                    var attr = (ConektaObject) handle.Unwrap();

                                    attr.LoadFromObject((JObject) obj);
                                    field.SetValue(this, attr);
                                    SetVal(key, attr);
                                }
                                else
                                {
                                    var tipoCampo = Type.GetType(field.FieldType.FullName);
                                    var valorConvertido = Convert.ChangeType(obj, tipoCampo);
                                    field.SetValue(this, valorConvertido);
                                    SetVal(key, valorConvertido);
                                }
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    // No field found
                    //System.out.println(e.toString());
                    throw new Error(e.Message);
                }
            }
        }

        public override String ToString()
        {
            return JsonConvert.SerializeObject(values, Formatting.Indented, new KeyValuePairConverter());
        }

        public static String toCamelCase(String s)
        {
            var parts = s.Split('_');
            return parts.Aggregate("", (current, part) => current + toProperCase(part));
        }

        private static String toProperCase(String s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }
    }
}