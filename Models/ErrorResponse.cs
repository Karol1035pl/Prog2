using System;

namespace Prog2.Models
{
    public class ErrorResponse
    {
        public string status { set; get; }
        public string message { set; get; }
        public ErrorResponse(string errorMessage)
        {
            this.status = "error";
            this.message = errorMessage;
        }
        public ErrorResponse(Exception errorMessage)
        {
            this.status = "error";
            this.message = errorMessage.Message;
        }
    }
}