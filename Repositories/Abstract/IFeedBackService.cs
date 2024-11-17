using eBookStore.Models;

namespace eBookStore.Repositories.Abstract;

public interface IFeedBackService
{
    bool AddFeedBack(FeedBack feedBack);
    bool UpdateFeedBack(FeedBack feedBack);
    bool DeleteFeedBack(int id);
    FeedBack GetFeedBackById(int id);
    IEnumerable<FeedBack> GetAllFeedBacks();
    IEnumerable<FeedBack> GetFeedBacksByBookId(int id);
}
