using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabaseRepository
{
    public class AccountsDto
    {
        public int id { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string form { get; set; }
    }
}
