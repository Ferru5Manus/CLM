using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabaseRepository
{
    public interface IFormsRepository
    {
        void SaveForms(FormDto forms);
        List<string> GetAllForms();
        List<string> GetAllIds();
        void RemoveForm(FormDto forms);
        void ChangeFormText(FormDto forms);
    }
}
