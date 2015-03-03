using System;

namespace ConektaCSharp
{
    public abstract class Conekta
    {
        public const String ApiVersion = "1.0.0";
        public const String Version = "1.0.7";
        public const String ApiBase = "https://api.conekta.io";
        public static string ApiKey { get; set; }
    }
}