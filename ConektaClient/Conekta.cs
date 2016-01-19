using System;
using System.Net;

namespace ConektaCSharp
{
    public abstract class Conekta
    {
        public const String ApiVersion = "1.0.0";
        public const String Version = "1.0.7";
        public const String ApiBase = "https://api.conekta.io";
        public static string ApiKey { get; set; }
        private static SecurityProtocolType _securityProtocol = SecurityProtocolType.Tls;
        public static SecurityProtocolType SecurityProtocol
        {
            get { return _securityProtocol; }
            set { _securityProtocol = value; }
        }
    }
}