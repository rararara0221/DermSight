using DermSight.Models;
using DermSight.Service;

namespace DermSight.ViewModels
{
    public class UserViewModel
    {
        public string? Search {get;set;}
        public Forpaging Forpaging {get;set;} = new();
        public List<User> User {get;set;} = [];
        
    }
}