using SchoolDatabaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class NewsManager
    {
       
        private NewsDatabaseRepository m_NewsRepository = new NewsDatabaseRepository();
        public bool AddNews(string Title, string Text)
        {
            if(!CheckNews(Title, Text))
            {
               m_NewsRepository.SaveNew(new NewsDto() { TitleString = Title, TextString = Text });
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

            List<string> s =m_NewsRepository.GetAllTitles();
            
            return s;
        }

        public List<string> GetNewsText()
        {

            List<string> s =m_NewsRepository.GetAllNewsText();
            
            return s;
        }

        public List<string> GetNewsId()
        {

            List<string> s =m_NewsRepository.GetAllIds();
            
            return s;
        }

        public void RemoveNew(int id)
        {
           m_NewsRepository.RemoveNew(new NewsDto() { Id=id});
        }
        public bool ChangeTitle(string newTitle,int id)
        {
            if (!CheckNews(newTitle,GetTextById(id)))
            {
               m_NewsRepository.ChangeNewTitle(new NewsDto() { newTitleString = newTitle, Id = id });
                return true;
            }
            else
            {
                return false;
            }
            

        }
        public string GetTitleById(int id)
        {
            return m_NewsRepository.GetTitleById(new NewsDto() { Id = id });
        }
        public string GetTextById(int id)
        {
            return m_NewsRepository.GetTextById(new NewsDto() { Id = id });
        }
        public bool ChangeText(string newText,int id)
        {
            if (!CheckNews(GetTitleById(id), newText))
            {
               m_NewsRepository.ChangeNewText(new NewsDto() { newTextString = newText, Id = id });
                return true;
            }
            else
            {
                return false;
            }
           
        }
    }
}
