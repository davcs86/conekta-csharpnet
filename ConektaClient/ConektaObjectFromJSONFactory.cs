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
    internal class ConektaObjectFromJSONFactory
    {
        public static ConektaObject ConektaObjectFactory(JObject jsonObject, String attributeName) {
            try { 
                var conektaObject = new ConektaObject();
                if (jsonObject["object"]!=null && isPaymentMethod(jsonObject)) {
                    conektaObject = PaymentMethodFactory(jsonObject);
                    try {
                        conektaObject.LoadFromObject(jsonObject);
                    } catch (Exception e) {
                        throw new Error(e.ToString());
                    }
                } else {
                    try {
                        ObjectHandle handle = Activator.CreateInstance(null, "ConektaCSharp."+ConektaObject.toCamelCase(attributeName));
                        conektaObject = (ConektaObject)handle.Unwrap();
                        conektaObject.LoadFromObject(jsonObject);
                    } catch (Exception e) {
                        throw new Error(e.ToString());
                    }
                }
                return conektaObject;
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        protected static PaymentMethod PaymentMethodFactory(JObject jsonObject) {
            try
            {
                PaymentMethod payment_method = null;
                if (isKindOfPaymentMethod(jsonObject, "card_payment"))
                {
                    payment_method = new CardPayment(jsonObject);
                }
                else if (isKindOfPaymentMethod(jsonObject, "cash_payment"))
                {
                    try
                    {
                        if (jsonObject["type"].ToString().Equals("oxxo"))
                        {
                            payment_method = new OxxoPayment(jsonObject);
                        }
                        else if (jsonObject["type"].ToString().Equals("real_time"))
                        {
                            payment_method = new RealTimePayment(jsonObject);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Error(e.ToString());
                    }
                }
                else if (isKindOfPaymentMethod(jsonObject, "bank_transfer_payment"))
                {
                    try
                    {
                        if (jsonObject["type"].ToString().Equals("banorte"))
                        {
                            payment_method = new BankTransferPayment(jsonObject);
                        }
                        else if (jsonObject["type"].ToString().Equals("spei"))
                        {
                            payment_method = new SpeiPayment(jsonObject);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Error(e.ToString());
                    }

                }
                if (isPaymentMethod(jsonObject))
                {
                    try
                    {
                        payment_method.LoadFromObject(jsonObject);
                    }
                    catch (Exception e)
                    {
                        throw new Error(e.ToString());
                    }
                    return payment_method;
                }
                throw new Error("Invalid PaymentMethod");
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        protected static Boolean isPaymentMethod(JObject jsonObject) {
            try { 
                Boolean card_payment = isKindOfPaymentMethod(jsonObject, "card_payment");
                Boolean cash_payment = isKindOfPaymentMethod(jsonObject, "cash_payment");
                Boolean bank_transfer_payment = isKindOfPaymentMethod(jsonObject, "bank_transfer_payment");
                Boolean is_payment = card_payment || cash_payment || bank_transfer_payment;
                return is_payment;
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        protected static Boolean isKindOfPaymentMethod(JObject jsonObject, String kind) {
            try {
                return jsonObject["object"].ToObject<String>().Equals(kind);
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
        }
    }
}
