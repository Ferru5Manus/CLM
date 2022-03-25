using SchoolDatabaseRepository;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class FormsManager
    {
        private static string path = @"C:\Tasks";
        FormsDatabaseRepository repository = new FormsDatabaseRepository();
        private ResultsDatabaseRepository res = new ResultsDatabaseRepository();
        private AccountsDatabaseRepository acc = new AccountsDatabaseRepository();
        public bool AddForm(string formString)
        {
            if (!IsFormExists(formString))
            {
                repository.SaveForms(new FormDto() { FormString = formString });
                CreateDirectory(formString).GetAwaiter();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetForms()
        {

            List<string> s = repository.GetAllForms();

            return s;
        }
        public string GetForm(int id)
        {
            return repository.GetForms(new FormDto { Id = id })[0];
        }
        public List<string> GetFormsId()
        {

            List<string> s = repository.GetAllIds();

            return s;
        }
        public bool IsFormExists(string formString)
        {
            List<string> s = repository.GetAllForms();
            foreach (var item in s)
            {
                if (formString == item)
                {
                    return true;
                }
            }
            return false;
        }
        public bool RemoveForm(int id)
        {
            
            RemoveFormFolder(repository.GetForms(new FormDto() { Id = id })[0]).GetAwaiter();
            List<string> s = repository.GetForms(new FormDto() { Id = id });
            acc.UpdateForms(new AccountsDto { form = repository.GetForms(new FormDto() { Id = id })[0] });
            repository.RemoveForm(new FormDto() { Id = id });
            res.RemoveResultByForm(new ResultDto { formString = s[0] });
            
            return true;
        }
        

        
        public async Task CreateDirectory(string FormString)
        {
            DirectoryInfo newTaskGroup = new DirectoryInfo(path);
            if (!newTaskGroup.Exists)
            {
                newTaskGroup.Create();
            }
            try
            {
                using (StreamWriter sw = new StreamWriter($"{path}/FormsInfo.txt", true, System.Text.Encoding.Default))
                {
                    try
                    {   
                        
                        Directory.CreateDirectory($"{path}/{FormString}");
                        await sw.WriteLineAsync(FormString);
                        sw.Close();
                        
                    }
                    catch (Exception)
                    {
                        Directory.CreateDirectory($"{path}/{FormString}");                        
                        await sw.WriteLineAsync(FormString);
                        sw.Close();                    
                    }

                }
            }
            catch (Exception)
            {

            }
        }
        public async Task RemoveFormFolder(string FormString)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo($"{path}/{FormString}");

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo directory in dirInfo.GetDirectories())
                {
                    foreach(FileInfo f in directory.GetFiles())
                    {
                        f.Delete();
                    }
                    directory.Delete();
                }
                try
                {
                    var myList = File.ReadAllLines($"{path}/FormsInfo.txt");
                    using (StreamWriter sw = new StreamWriter($"{path}/FormsInfo.txt", false, System.Text.Encoding.Default))
                    {
                        foreach (var item in myList)
                        {


                            if (item != FormString)
                            {
                                await sw.WriteLineAsync(item);

                            }


                        }

                        sw.Close();
                    }
                    Directory.Delete($"{path}/{FormString}");
                }
                catch (Exception)
                {
                    Directory.Delete($"{path}/{FormString}");
                }
            }
            catch(Exception)
            {

                throw;
                
            }
           

            
            
        }
    }
}
