using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabaseRepository
{
    public class ResultDto
    {
        public int id { get; set; }
        public string login { get; set; }
        public int taskGrId { get; set; }
        public int taskId { get; set; }
        public string formString { get; set; }
        public string taskResult { get; set; }
    }
}
