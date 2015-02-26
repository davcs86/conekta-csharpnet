using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    internal class ConektaRequestorUa
    {
        public String publisher { get; set; }
        public String lang_version { get; set; }
        public String bindings_version { get; set; }
        public String lang { get; set; }

        public ConektaRequestorUa ()
        {
            Version ver = Environment.Version;
            bindings_version = Conekta.ApiVersion;
            lang = "java"; // "csharpnet";
            lang_version = "1.7.0_76"; //ver.ToString();
            publisher = "conekta"; //"davcs86@gmail.com";
        }
    }

    internal class ConektaRequestor
    {
        private static String _apiKey;
        private static String _apiBase;

        protected HttpWebRequest Connection;

        public ConektaRequestor()
        {
            _apiKey = Conekta.ApiKey;
            _apiBase = Conekta.ApiBase;
        }
        private static String ApiUrl(String url)
        {
            return _apiBase + url;
        }
        private void SetHeaders()
        {
            try { 
                ConektaRequestorUa userAgent = new ConektaRequestorUa();
                Connection.Accept = "application/vnd.conekta-v" + Conekta.ApiVersion + "+json";
            
                //Connection.UserAgent = "Conekta/v1 CSharpBindings/" + Conekta.Version;
                Connection.UserAgent = "Conekta/v1 JavaBindings/" + Conekta.Version;
            
                Connection.ContentType = "application/x-www-form-urlencoded";

                Connection.Headers.Add("X-Conekta-Client-User-Agent", JsonConvert.SerializeObject(userAgent));
                Connection.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(_apiKey + "")));
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public Object Request(String method, String url)
        {
            return Request(method, url, null);
        }

        public Object Request(String method, String url, JObject _params) 
        {
            try {
                String apiURL = ApiUrl(url);

                X509Certificate x509certificate = new X509Certificate();
                x509certificate.Import(Certificate.ca_bundle);

                ServicePointManager.Expect100Continue = true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                Connection = (HttpWebRequest)WebRequest.Create(apiURL);

                Connection.Method = method;
                Connection.ClientCertificates.Add(x509certificate);
                Connection.Timeout = 60000;
                Connection.ReadWriteTimeout = 60000;
                Connection.PreAuthenticate = true;

                SetHeaders();
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
            if (_params != null) {
                Stream os = null;
                try {
                    os = Connection.GetRequestStream();
                } catch (Exception e) {
                    throw new Error("Could not connect to " + Conekta.ApiBase + " (" + e.ToString() + ").");
                }
                try {
                    String r = getQuery(_params, null);
                    var documentBytes = Encoding.UTF8.GetBytes(r);
                    os.Write(documentBytes, 0, documentBytes.Length);
                    os.Flush();
                    os.Close();
                } catch (Exception e) {
                    throw new Error(e.ToString());
                }
            }
            HttpStatusCode responseCode = HttpStatusCode.OK;
            HttpWebResponse response;
            Object obj = null;
            StreamReader instr = null;
            Stream receiveStream = null;
            String inputLine;
            StringBuilder responseStr = new StringBuilder();
            try
            {
                response = (HttpWebResponse) Connection.GetResponse();
                responseCode = response.StatusCode;

                receiveStream = response.GetResponseStream();
                instr = new StreamReader(receiveStream, Encoding.UTF8);
                while ((inputLine = instr.ReadLine()) != null) {
                    responseStr.Append(inputLine);
                }
                instr.Close();
                switch ((int)responseStr.ToString()[0])
                {
                    // {
                    case 123:
                        obj = JObject.Parse(responseStr.ToString());
                        break;
                    // [
                    case 91:
                        obj = JArray.Parse(responseStr.ToString());//JArray
                        break;
                    default:
                        throw new Error("invalid response: " + responseStr);
                    // Other
                }
                if (responseCode != HttpStatusCode.OK) {
                    Error.errorHandler((JObject) obj, int.Parse(responseCode.ToString()));
                }
            } catch (Exception e) {
                JObject error = null;
                error = JObject.Parse("{'message':'" + HttpUtility.UrlEncode(e.Message, Encoding.UTF8) + "'}");
                var wex = (WebException) e;
                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    var wexr = wex.Response as HttpWebResponse;
                    if (wexr != null)
                    {
                        Error.errorHandler(error, (int) wexr.StatusCode);
                    }
                }
                Error.errorHandler(error);
            }
            return obj;
        }

        private static String getQuery(JObject jsonObject, String index) {
            try { 
                StringBuilder result = new StringBuilder();
                IEnumerator itr = jsonObject.Properties().GetEnumerator();
                Boolean first = true;
                while (itr.MoveNext()) {
                    if (first) {
                        first = false;
                    } else {
                        result.Append("&");
                    }
                    String key = ((JProperty) itr.Current).Name;
                    Object value = jsonObject[key];
                    var o = value as JObject;
                    if (o != null) {
                        if (index != null) {
                            key = index + "[" + key + "]";
                        }
                        result.Append(getQuery(o, key));
                    } else
                    {
                        var jArray = value as JArray;
                        if (jArray != null) {
                            JArray array = jArray;
                            for (int i = 0; i < array.Count; i++) {
                                if (array[i] is JObject) {
                                    if (index != null && i == 0) {
                                        key = index + "[" + key + "][]";
                                    }
                                    result.Append(getQuery(array[i].ToObject<JObject>(), key));
                                } else {
                                    result.Append(index != null
                                        ? HttpUtility.UrlEncode(index + "[" + key + "]" + "[]", Encoding.UTF8)
                                        : HttpUtility.UrlEncode(key + "[]", Encoding.UTF8));
                                    result.Append("=");
                                    result.Append(HttpUtility.UrlEncode(array[i].ToString(), Encoding.UTF8));
                                }
                                result.Append("&");
                            }
                
                        } else {
                            if (index != null) {
                                result.Append(HttpUtility.UrlEncode(index + "[" + key + "]", Encoding.UTF8));
                            } else {
                                result.Append(key);

                            }
                            result.Append("=");
                            result.Append(HttpUtility.UrlEncode(value.ToString(), Encoding.UTF8));
                        }
                    }
                }
                return result.ToString();
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }
    }
}
