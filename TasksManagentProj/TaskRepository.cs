using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagentProj
{
    public class TaskRepository
    {
        private string path = @"C:\Tasks";
        public void CreateTask(TaskGroupTemplate hw)
        {
            
            DirectoryInfo newTaskGroup = new DirectoryInfo(path);
            if (!newTaskGroup.Exists)
            {
                newTaskGroup.Create();
            }
            Directory.CreateDirectory($"{path}/{hw.Name}");
        }
    }
}
