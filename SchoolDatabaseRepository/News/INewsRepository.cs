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
        string GetTitleById(NewsDto news);
        string GetTextById(NewsDto news);
        void RemoveNew(NewsDto news);
        void ChangeNewTitle(NewsDto news);
        void ChangeNewText(NewsDto news);
        string GetNewsIdByTitle(NewsDto news);      

    }
}
