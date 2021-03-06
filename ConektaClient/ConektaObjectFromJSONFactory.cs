﻿using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    internal class ConektaObjectFromJSONFactory
    {
        public static ConektaObject ConektaObjectFactory(JObject jsonObject, String attributeName)
        {
            var conektaObject = new ConektaObject();
            if (jsonObject["object"] != null && isPaymentMethod(jsonObject))
            {
                conektaObject = PaymentMethodFactory(jsonObject);
                try
                {
                    conektaObject.LoadFromObject(jsonObject);
                }
                catch (Exception e)
                {
                    throw new Error(e.Message);
                }
            }
            else
            {
                try
                {
                    var handle = Activator.CreateInstance(null,
                        "ConektaCSharp." + ConektaObject.toCamelCase(attributeName));
                    conektaObject = (ConektaObject) handle.Unwrap();
                    conektaObject.LoadFromObject(jsonObject);
                }
                catch (Exception e)
                {
                    throw new Error(e.Message);
                }
            }
            return conektaObject;
        }

        protected static PaymentMethod PaymentMethodFactory(JObject jsonObject)
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
                    throw new Error(e.Message);
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
                    throw new Error(e.Message);
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
                    throw new Error(e.Message);
                }
                return payment_method;
            }
            throw new Error("Invalid PaymentMethod");
        }

        protected static Boolean isPaymentMethod(JObject jsonObject)
        {
            var card_payment = isKindOfPaymentMethod(jsonObject, "card_payment");
            var cash_payment = isKindOfPaymentMethod(jsonObject, "cash_payment");
            var bank_transfer_payment = isKindOfPaymentMethod(jsonObject, "bank_transfer_payment");
            var is_payment = card_payment || cash_payment || bank_transfer_payment;
            return is_payment;
        }

        protected static Boolean isKindOfPaymentMethod(JObject jsonObject, String kind)
        {
            return jsonObject["object"].ToObject<String>().Equals(kind);
        }
    }
}