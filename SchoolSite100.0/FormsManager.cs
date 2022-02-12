using SchoolDatabaseRepository;
using SimpleWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class FormsManager
    {
        FormsDatabaseRepository repository = new FormsDatabaseRepository();
        public void AddForm(string formString)
        {
            repository.SaveForms(new FormDto() { FormString = formString });

        }

        public List<string> GetForms()
        {

            List<string> s = repository.GetAllForms();

            return s;
        }

        public List<string> GetFormsId()
        {

            List<string> s = repository.GetAllIds();

            return s;
        }

        public void RemoveForm(int id)
        {
            repository.RemoveForm(new FormDto() { Id = id });
        }
        

        public void ChangeForm(string FormString, int id)
        {
            repository.ChangeFormText(new FormDto() { FormString = FormString, Id = id });
        }
    }
}
