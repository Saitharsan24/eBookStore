using eBookStore.Models;
using eBookStore.Repositories.Abstract;

namespace eBookStore.Repositories.Implementation;

public class FeedBackService(AppDbContext context) : IFeedBackService
{
    public bool AddFeedBack(FeedBack feedBack)
    {
        try
        {
            context.Add(feedBack);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    public bool DeleteFeedBack(int id)
    {
        try
        {
            var data = this.GetFeedBackById(id);
            if (data != null)
            {
                return false;
            }

            context.Remove(data);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    public IEnumerable<FeedBack> GetAllFeedBacks()
    {
        return context.FeedBack.ToList();
    }

    public FeedBack GetFeedBackById(int id)
    {
        return context.FeedBack.Find(id);
    }

    public IEnumerable<FeedBack> GetFeedBacksByBookId(int bookID)
    {
        return context.FeedBack.Where(x => x.BookID == bookID);
    }

    public bool UpdateFeedBack(FeedBack feedBack)
    {
        try
        {
            context.FeedBack.Update(feedBack);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
}
