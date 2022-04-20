using SchoolDatabaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class NewsManager
    {
       
        NewsDatabaseRepository repository = new NewsDatabaseRepository();
        public bool AddNews(string Title, string Text)
        {
            if(!CheckNews(Title, Text))
            {
                repository.SaveNew(new NewsDto() { TitleString = Title, TextString = Text });
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool CheckNews(string Title, string Text)
        {
            List<string> list = GetNewsTit();
            
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i]==Title)
                {
                    return true;
                }
            }
            return false;
        }
        public List<string> GetNewsTit()
        {

            List<string> s = repository.GetAllTitles();
            
            return s;
        }

        public List<string> GetNewsText()
        {

            List<string> s = repository.GetAllNewsText();
            
            return s;
        }

        public List<string> GetNewsId()
        {

            List<string> s = repository.GetAllIds();
            
            return s;
        }

        public void RemoveNew(int id)
        {
            repository.RemoveNew(new NewsDto() { Id=id});
        }
        public bool ChangeTitle(string newTitle,int id)
        {
            if (!CheckNews(newTitle,GetTextById(id)))
            {
                repository.ChangeNewTitle(new NewsDto() { newTitleString = newTitle, Id = id });
                return true;
            }
            else
            {
                return false;
            }
            

        }
        public string GetTitleById(int id)
        { 
            return repository.GetTitleById(new NewsDto() { Id = id })[0];
        }
        public string GetTextById(int id)
        {
            return repository.GetTextById(new NewsDto() { Id = id })[0];
        }
        public bool ChangeText(string newText,int id)
        {
                repository.ChangeNewText(new NewsDto() { newTextString = newText, Id = id });
                return true;    
        }
    }
}
