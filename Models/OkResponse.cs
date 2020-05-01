using System;

namespace Prog2.Models
{
    public class OkResponse<T>
    {
        public string status { set; get; }
        public T result { set; get; }

        public OkResponse(T result)
        {
            this.status = "ok";
            this.result = result;
        }
    }
}