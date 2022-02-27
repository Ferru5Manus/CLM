using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class TaskRepository
    {
        private static string path = @"C:\Tasks";
        public async Task CreateTaskGr(TaskGroupTemplate hw,string FormString)
        {

            DirectoryInfo newTaskGroup = new DirectoryInfo(path);
            if (!newTaskGroup.Exists)
            {
                newTaskGroup.Create();
            }

            try
            {
                using (StreamWriter sw = new StreamWriter($"{path}/{FormString}/TaskGroupInfo.txt", true, System.Text.Encoding.Default))
                {
                    try
                    {
                        var lastLine = File.ReadLines($"{path}/{FormString}/TaskGroupInfo.txt").Last();
                        string[] data = lastLine.Split('|');
                        string x = hw.Id + "-_-" + hw.Name;
                        Directory.CreateDirectory($"{path}/{FormString}/{x}");
                        int id = Convert.ToInt32(data[0]) + 1;
                        await sw.WriteLineAsync(id.ToString() + "|" + hw.Name);
                        sw.Close();
                      
                     }
                    catch(Exception)
                    {
                        
                        
                        string x = '1' + "-_-" + hw.Name;
                        Directory.CreateDirectory($"{path}/{FormString}/{x}");
                        int id = 1;
                        await sw.WriteLineAsync(id.ToString() + "|" + hw.Name);
                        sw.Close();
                        

                    }
                    
                }
                
                
               

                
            }
            catch(Exception)
            {

            }
            

        }
        public bool IsTaskGroupExists(TaskGroupTemplate hw, string FormString)
        {
            try
            {
                var myList = File.ReadAllLines($"{path}/{FormString}/TaskGroupInfo.txt");
                foreach (var item in myList)
                {
                    string[] data = item.Split("|");
                    if (data[1] == hw.Name.ToString())
                    {
                        return true;

                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task RemoveTaskGr(TaskGroupTemplate hw,string FormString)
        {
            string x = GetTaskGrName(hw.Id, FormString);
            try
            {
                
                var myList = File.ReadAllLines($"{path}/{FormString}/TaskGroupInfo.txt");

                using (StreamWriter sw = new StreamWriter($"{path}/{FormString}/TaskGroupInfo.txt", false, System.Text.Encoding.Default))
                {
                    foreach (var item in myList)
                    {
                        string[] data = item.Split("|");
                        if (data.Length > 1)
                        {
                            if (data[0] != hw.Id.ToString())
                            {
                                await sw.WriteLineAsync(data[0] + "|" + data[1]);

                            }
                        }

                    }
                    Directory.Delete($"{path}/{FormString}/{x}");
                    sw.Close();
                }
               
                try
                {

                    
                    DirectoryInfo dirInfo = new DirectoryInfo($"{path}/{FormString}/{x}");

                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch (Exception)
                {
                    Directory.Delete($"{path}/{FormString}/{x}");
                }
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        
        public List<string> GetTaskGroups(string FormString)
        {
            var myList = File.ReadAllLines($"{path}/{FormString}/TaskGroupInfo.txt");         
            return myList.ToList();
        }
        public async Task AddTask(TaskGroupTemplate hw,TaskTemplate task,string FormString)
        {

            string x = GetTaskGrName(hw.Id,FormString); ;
            
            
            
            if (!IsTaskTitleExists(FormString,hw.Id ,task.Title))
            {
                try
                {

                    try
                    {
                       
                        var lastLine = File.ReadLines($"{path}/{FormString}/{x}/TasksInfo.txt").Last();
                        string[] data = lastLine.Split('|');
                        int id = Convert.ToInt32(data[0]) + 1;  
                        using (StreamWriter sw = new StreamWriter($"{path}/{FormString}/{x}/TasksInfo.txt", true, System.Text.Encoding.Default))
                        {
                            await sw.WriteLineAsync(id.ToString() + "|" + task.Title + "|" + task.Text + "|" + task.Answer);
                            sw.Close();
                        }
                    }
                    catch
                    {
                        int id = 1;                     
                        using (StreamWriter sw = new StreamWriter($"{path}/{FormString}/{x}/TasksInfo.txt", true, System.Text.Encoding.Default))
                        {
                            
                            await sw.WriteLineAsync(id.ToString() + "|" + task.Title + "|" + task.Text + "|" + task.Answer);
                            sw.Close();
                        }
                    }

                      

                    
                    

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private string GetTaskGrName(int id,string FormString)
        {
            string x = "";
            var myList = File.ReadAllLines($"{path}/{FormString}/TaskGroupInfo.txt");
            foreach (var item in myList)
            {
                string[] data = item.Split("|");
                if (data[0] == id.ToString())
                {
                    x = id.ToString() + "-_-" + data[1];
                    break;
                }
            }
            return x;
        }
        public List<string> GetAllIds(string FormString)
        {
            List<string> s = new List<string>();
            var myList = File.ReadAllLines($"{path}/{FormString}/TaskGroupInfo.txt");
            foreach (var item in myList)
            {
                string[] data = item.Split("|");
                s.Add(data[0]);
                    
            }
            return s;
        }

        public List<string> GetAllNames(string FormString)
        {
            List<string> s = new List<string>();
            var myList = File.ReadAllLines($"{path}/{FormString}/TaskGroupInfo.txt");
            foreach (var item in myList)
            {
                string[] data = item.Split("|");
                s.Add(data[1]);

            }
            return s;
        }
        public bool IsTaskExists(TaskGroupTemplate hw, TaskTemplate task,string FormString)
        {
            string x = GetTaskGrName(hw.Id,FormString);
            try
            {
                
                FileInfo fileInfo = new FileInfo($"{path}/{FormString}/{x}/{task.Id}.csv");
                return fileInfo.Exists;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsTaskTitleExists(string FormString, int TaskGrId, string TaskTitle)
        {
            try
            {
                string x = GetTaskGrName(TaskGrId, FormString);
                var myList = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
                foreach (var item in myList)
                {
                    string[] data = item.Split("|");
                    if (data[1] == TaskTitle.ToString())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetTaskId(string FormString, int TaskGrId, string TaskTitle)
        {
            string x = GetTaskGrName(TaskGrId, FormString);
            var myList = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            foreach (var item in myList)
            {
                string[] data = item.Split("|");
                if (data[1] == TaskTitle.ToString())
                {
                    return data[0];
                }
            }
            return "";
        }
        public List<string> GetTasksIds(int TaskGrId, string FormString)
        {
            List<string> lst = new List<string>();
            string x = GetTaskGrName(TaskGrId, FormString);
            
            
            var myList2 = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split("|");
                lst.Add(data[0]);
                
            }
            return lst;
           
        }
        public List<string> GetTasksTitles(int TaskGrId, string FormString)
        {
            List<string> lst = new List<string>();
            string x = GetTaskGrName(TaskGrId, FormString);


            var myList2 = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split("|");
                lst.Add(data[1]);
                
            }
            return lst;

        }
        public List<string> GetTasksTextes(int TaskGrId, string FormString)
        {
            List<string> lst = new List<string>();
            string x = GetTaskGrName(TaskGrId, FormString);


            var myList2 = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split("|");
                lst.Add(data[2]);
                
            }
            return lst;

        }
        public List<string> GetTasksAnswers(int TaskGrId, string FormString)
        {
            List<string> lst = new List<string>();
            string x = GetTaskGrName(TaskGrId, FormString);


            var myList2 = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split("|");
                lst.Add(data[3]);
               
            }
            return lst;

        }
        public string GetTaskAnswer(int TaskGrId, string FormString, int TaskId)
        {
            string ans = "";
            string x = GetTaskGrName(TaskGrId, FormString);


            var myList2 = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split("|");
                if (data[0] == TaskId.ToString())
                {
                    ans = data[data.Length-1];
                }

            }
            return ans;

        }
        public async Task RemoveTask(TaskGroupTemplate hw, TaskTemplate task, string FormString)
        {
            try
            {
                string x = GetTaskGrName(hw.Id,FormString);

                var myList = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
                try
                {
                    using (StreamWriter sw = new StreamWriter($"{path}/{FormString}/{x}/TasksInfo.txt", false, System.Text.Encoding.Default))
                    {
                        foreach (var item in myList)
                        {
                            string[] data = item.Split("|");
                            if (data.Length > 1)
                            {
                                if (data[0] != task.Id.ToString())
                                {
                                    await sw.WriteLineAsync(data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3]);

                                }
                            }

                        }
                        
                    }
                    
                }
                catch (Exception)
                {
                    
                }
                
            }
            catch (Exception)
            {
                throw ;
            }
           
        }
        
        public async Task ChangeTask(TaskGroupTemplate hw, TaskTemplate task, string FormString)
        {
            string x = GetTaskGrName(hw.Id,FormString);

            
            var myList2 = File.ReadAllLines($"{path}/{FormString}/{x}/TasksInfo.txt");
            
            using (StreamWriter sw = new StreamWriter($"{path}/{FormString}/{x}/TasksInfo.txt", false, System.Text.Encoding.Default))
            {
                foreach (var item in myList2)
                {
                    string[] data = item.Split("|");
                    if (data.Length > 1)
                    {
                        if (data[0] == task.Id.ToString())
                        {
                            await sw.WriteLineAsync(task.Id.ToString() + "|" + task.Title + "|" + task.Text + "|" + task.Answer);
                        }
                        else
                        {
                            await sw.WriteLineAsync(data[0]+ "|" + data[1] + "|" + data[2] + "|" + data[3]);
                        }
                    }

                }

                sw.Close();
            }
            
        }
    }
}
