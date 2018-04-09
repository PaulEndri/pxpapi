#region

using System;
using System.Collections.Generic;
using PixelPubApi.Interfaces;

#endregion

namespace PixelPubApi.Models
{
    public abstract class Model : IModel
    {
        public abstract long getPrimaryKey();
        public virtual string getDateTimeCol()
        {
            return "updated_at";
        }
        public virtual string getPrimaryKeyCol()
        {
            return "id";
        }

        public virtual string getDateFilterSubQuery(DateTime? before, DateTime? after)
        {
            var field  = getDateTimeCol();
            var fields = new List<string>();
            // If we have no field possible to search on, or no search params just auto true
            if(field == null || (before == null && after == null)) {
                return "";
            }

            if (before != null) {
                fields.Add($"{field} <= {before}");
            }

            if (after != null) {
                fields.Add($"{field} >= {after}");
            }

            return string.Join(" AND ", fields);
        }
    }
}
