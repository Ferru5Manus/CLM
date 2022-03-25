using System;
using System.Collections.Generic;
namespace SchoolDatabaseRepository
{
    public interface INewsRepository
    {
        void SaveNew(NewsDto news);
        List<string> GetAllTitles();
        List<string> GetAllNewsText();
        List<string> GetAllIds();
        List<string> GetTitleById(NewsDto news);
        List<string> GetTextById(NewsDto news);
        void RemoveNew(NewsDto news);
        void ChangeNewTitle(NewsDto news);
        void ChangeNewText(NewsDto news);
        List<string> GetNewsByTitle(NewsDto news);      

    }
}
