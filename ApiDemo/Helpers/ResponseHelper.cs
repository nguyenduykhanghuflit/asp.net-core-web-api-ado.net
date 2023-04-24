using System.Dynamic;

namespace ApiDemo.Helpers
{
    public class ResponseHelper
    {
        int ErrCode { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }

        public ResponseHelper(int errCode, string? message, object? data)
        {
            ErrCode = errCode;
            Message = message;
            Data = data;
        }


        public IDictionary<string, object> HandleResponse()
        {
            dynamic obj = new ExpandoObject();
            obj.Error = this.ErrCode;
            obj.Message = this.Message;
            obj.Data = this.Data ?? null;
            IDictionary<string, object> result = (IDictionary<string, object>)obj;
            return result;
        }
    }
}
