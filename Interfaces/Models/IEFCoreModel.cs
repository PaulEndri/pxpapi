using System;

namespace PixelPubApi.Interfaces
{
    public interface IModel
    {
        long getPrimaryKey();
        string getDateTimeCol();
        string getPrimaryKeyCol();

        string getDateFilterSubQuery(DateTime? before, DateTime? after);
    }
}
