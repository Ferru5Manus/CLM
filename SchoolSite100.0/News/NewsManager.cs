using SimpleWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolSite100._0
{
    public class NewsManager
    {
        List<NewTemplate> news = new List<NewTemplate>();
        NewsDatabaseRepository repository = new NewsDatabaseRepository();
        public void AddNews(string Title, string Text)
        {
            repository.SaveNew(new NewsDto() { TitleString=Title,TextString=Text});
            
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
        public void ChangeTitle(string newTitle,int id)
        {
            repository.ChangeNewTitle(new NewsDto() { newTitleString = newTitle, Id=id});
            

        }

        public void ChangeText(string newText,int id)
        {
            repository.ChangeNewText(new NewsDto() {  newTextString=newText,Id=id});
        }
    }
}
