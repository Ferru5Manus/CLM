using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabaseRepository
{
    public interface IResultRepository
    {
        void CreateResultTemplates(ResultDto result);
        List<string> GetAllResults();
        List<string> GetResultByLogin(ResultDto result);
        void AddResult(ResultDto result);
        void RemoveResult(ResultDto result);
        void RemoveResultByForm(ResultDto result);
        List<string> GetIdsByTask(ResultDto result);
        List<string> GetIdsByTaskLogin(ResultDto result);
        
    }
}
