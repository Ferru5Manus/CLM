using System;
using System.Collections.Generic;
namespace SimpleWebApp.Repository
{
    public interface INewsRepository
    {
        void SaveNew(NewsDto news);
        List<string> GetAllTitles();
        List<string> GetAllNewsText();
        List<string> GetAllIds();
        void RemoveNew(NewsDto news);
        void ChangeNewTitle(NewsDto news);
        void ChangeNewText(NewsDto news);
       

    }
}
