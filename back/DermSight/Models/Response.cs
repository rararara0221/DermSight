
namespace DermSight.Models
{
    public class Response
    {
        public int status_code { get; set; }
        public string? message { get; set; }
        public object? data {get;set;}
    }
}