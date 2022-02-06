using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class NewsManager
    {
        List<NewTemplate> news = new List<NewTemplate>();
        public void AddNews(string Title, string Text)
        {
            
            news.Add(new NewTemplate { Id = news.Count + 1, newTitle = Title, newString = Text }) ;
        }

        public List<string> GetNewsTit()
        {

            List<string> s = new List<string>();
            foreach (var item in news)
            {
                s.Add(item.newTitle);
            }
            return s;
        }

        public List<string> GetNewsText()
        {

            List<string> s = new List<string>();
            foreach (var item in news)
            {
                s.Add(item.newString);
            }
            return s;
        }

        public List<string> GetNewsId()
        {

            List<string> s = new List<string>();
            foreach (var item in news)
            {
                s.Add(item.Id.ToString());
            }
            return s;
        }

        public void RemoveNew(int id)
        {
            news.Remove(news[id - 1]);
        }
        public void ChangeTitle(string lastTitle,string newTitle,int id)
        {
            news[id - 1].newTitle = newTitle;
            

        }

        public void ChangeText(string lastNew, string newNew,int id)
        {
            news[id - 1].newTitle = newNew;
        }
    }
}
