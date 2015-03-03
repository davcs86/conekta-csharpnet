using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Error : Exception
    {
        public String _params;
        public int code;
        public String message;
        public String type;

        public Error(String message) : this(message, "", 0, "")
        {
        }

        public Error(String message, String type, int code, String _params) : base(message)
        {
            this.message = message;
            this.type = type;
            this.code = code;
            this._params = _params;
        }

        public override String ToString()
        {
            return message;
        }

        public static void errorHandler(JObject response, int responseCode = 0)
        {
            String message = null;
            String type = null;
            String __params = null;
            if (response["message"] != null)
            {
                try
                {
                    message = response["message"].ToString();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            if (response["type"] != null)
            {
                try
                {
                    type = response["type"].ToString();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            if (response["param"] != null)
            {
                try
                {
                    __params = response["param"].ToString();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            switch (responseCode)
            {
                case 400:
                    throw new MalformedRequestError(message, type, responseCode, __params);
                case 401:
                    throw new AuthenticationError(message, type, responseCode, __params);
                case 402:
                    throw new ProcessingError(message, type, responseCode, __params);
                case 404:
                    throw new ResourceNotFoundError(message, type, responseCode, __params);
                case 422:
                    throw new ParameterValidationError(message, type, responseCode, __params);
                case 500:
                    throw new ApiError(message, type, responseCode, __params);
                default:
                    throw new Error(message, type, responseCode, __params);
            }
        }
    }

    public class ApiError : Error
    {
        public ApiError(String message, String type, int code, String _params) : base(message, type, code, _params)
        {
        }
    }

    public class NoConnectionError : Error
    {
        public NoConnectionError(String message, String type, int code, String _params)
            : base(message, type, code, _params)
        {
        }
    }

    public class AuthenticationError : Error
    {
        public AuthenticationError(String message, String type, int code, String _params)
            : base(message, type, code, _params)
        {
        }
    }

    public class ParameterValidationError : Error
    {
        public ParameterValidationError(String message, String type, int code, String _params)
            : base(message, type, code, _params)
        {
        }
    }

    public class ProcessingError : Error
    {
        public ProcessingError(String message, String type, int code, String _params)
            : base(message, type, code, _params)
        {
        }
    }

    public class ResourceNotFoundError : Error
    {
        public ResourceNotFoundError(String message, String type, int code, String _params)
            : base(message, type, code, _params)
        {
        }
    }

    public class MalformedRequestError : Error
    {
        public MalformedRequestError(String message, String type, int code, String _params)
            : base(message, type, code, _params)
        {
        }
    }
}