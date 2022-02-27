using SchoolDatabaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class TaskManager
    {
        private TaskRepository tasker = new TaskRepository();
        private FormsManager forms = new FormsManager();
        private AccountsDatabaseRepository accs= new AccountsDatabaseRepository();
        private ResultsDatabaseRepository results = new ResultsDatabaseRepository();
        public bool CreateTaskGr(string name, string FormString)
        {
            if (tasker.IsTaskGroupExists(new TaskGroupTemplate { Name = name }, FormString) == false)
            {
                if (forms.IsFormExists(FormString))
                {
                    tasker.CreateTaskGr(new TaskGroupTemplate { Name = name }, FormString).GetAwaiter();
                    return true;

                }
                return false;


            }
            return false;
        }
        public List<string> GetTaskGrIds(string FormString)
        {
            return tasker.GetAllIds(FormString);
        }
        public List<string> GetTaskGrNames(string FormString)
        {
            return tasker.GetAllNames(FormString);
        }
        public List<string> GetTaskIds(string formString, int TaskGrId)
        {
            return tasker.GetTasksIds(TaskGrId, formString);
        }
        public List<string> GetTaskTitles(string formString, int TaskGrId)
        {
            return tasker.GetTasksTitles(TaskGrId, formString);
        }
        public List<string> GetTaskTextes(string formString, int TaskGrId)
        {
            return tasker.GetTasksTextes(TaskGrId, formString);
        }
        public List<string> GetTaskAnswers(string formString, int TaskGrId)
        {
            return tasker.GetTasksAnswers(TaskGrId, formString);
        }
        public void RemoveTaskGr(int id,string formString)
        {
            List<string> lst= tasker.GetTasksIds(id, formString);
            foreach (var item in lst)
            {
                RemoveTask(new TaskTemplate { TaskGrId = id, formString = formString, Id = Convert.ToInt32(item) });
            }
            tasker.RemoveTaskGr(new TaskGroupTemplate { Id = id }, formString).GetAwaiter();

        }
        public bool ChangeTaskTitle(TaskTemplate task)
        {
            if (!tasker.IsTaskTitleExists(task.formString, task.TaskGrId, task.Title))
            {
                tasker.ChangeTask(new TaskGroupTemplate { Id = task.TaskGrId }, task, task.formString).GetAwaiter();
                return true;
            }
            return false;

            
        }
        public void ChangeTask(TaskTemplate task)
        {
            tasker.ChangeTask(new TaskGroupTemplate { Id = task.TaskGrId }, task, task.formString).GetAwaiter();
        }
        public bool CreateTask(int taskGrId, string FormString, string title, string text, string answer,string login)
        {
            List<string> lst = accs.GetLoginsByForm(new AccountsDto { form = FormString });
            if (lst.Count != 0)
            {
                if (!tasker.IsTaskTitleExists(FormString, taskGrId, title))
                {

                    tasker.AddTask(new TaskGroupTemplate { formString = FormString, Id = taskGrId }, new TaskTemplate { Answer = answer, formString = FormString, TaskGrId = taskGrId, Text = text, Title = title }, FormString).GetAwaiter();
                    int id = Convert.ToInt32(tasker.GetTaskId(FormString, taskGrId, title));
                    foreach (var item in lst)
                    {
                        results.CreateResultTemplates(new ResultDto { login = item, taskGrId = taskGrId, taskId = id,formString=FormString, taskResult = "0" });
                    }
                    return true;

                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public bool Check(TaskTemplate task,string login)
        {
            if (tasker.GetTaskAnswer(task.TaskGrId,task.formString,task.Id)==task.Answer)
            {
                int id = Convert.ToInt32(results.GetIdsByTaskLogin(new ResultDto { taskGrId = task.TaskGrId,taskId=task.Id,login=login,formString= task.formString })[0]);
                results.AddResult(new ResultDto { id = id,taskResult="1" });
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetResult(TaskTemplate task, string login)
        {

            List<string> lst = results.GetResultByLogin(new ResultDto { login = login, taskGrId = task.TaskGrId, taskId = task.Id,formString=task.formString });
            if (lst[0]=="1")
            {
                return true;
            }
            else
            {
                return false;
            }  
            
            
        }
        public void RemoveTask(TaskTemplate task)
        {
            tasker.RemoveTask(new TaskGroupTemplate { Id = task.TaskGrId }, task, task.formString).GetAwaiter();
            List<string> lst = results.GetIdsByTask(new ResultDto { taskGrId = task.TaskGrId, taskId = task.Id ,formString=task.formString});
            foreach (var item in lst)
            {
                results.RemoveResult(new ResultDto { id = Convert.ToInt32(item) });
            }
        }
        public string GetResults(TaskTemplate task,string login)
        {
            List<string> lst = GetTaskIds(task.formString, task.TaskGrId);
            double counter = 0;
            foreach (var item in lst)
            {
                List<string> lst2 = results.GetResultByLogin(new ResultDto { login = login, taskGrId = task.TaskGrId, taskId = Convert.ToInt32(item),formString= task.formString });
                counter += Convert.ToDouble(lst2[0]);
            }
            return ((counter / lst.Count) * 100).ToString();
        }
        public List<string> GetAllResults(TaskTemplate task)
        {
            List<string> lst = accs.GetLoginsByForm(new AccountsDto { form = task.formString });
            List<string> lst2 = new List<string>();
            foreach (var item in lst)
            {
                lst2.Add(GetResults(task, item));

            }
            return lst2;
        }
        public List<string> GetAllLogins(string formString)
        {
            List<string> lst = accs.GetLoginsByForm(new AccountsDto { form = formString });
            return lst;
        }
    }
}
