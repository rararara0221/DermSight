
namespace DermSight.Models
{
    public class User
    {
        public int userId { get; set; }
        public required string Name { get; set; } // nvarchar(10)
        public string? Photo { get; set; } // varchar(MAX)
        public required string Account { get; set; } // varchar(30)
        public required string Password { get; set; } // varchar(MAX)
        public required string Mail { get; set; } // varchar(50)
        public string? AuthCode { get; set; }
        public int Role { get; set; }
    }
}