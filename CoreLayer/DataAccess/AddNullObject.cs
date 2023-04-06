using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess
{
    public static class AddNullObject<TEntity> where TEntity : class, new()
    {
        public static List<TEntity> GetNullValuesForBefore(int page, int take)
        {
            var values = new List<TEntity>();
            for (int i = 0; i < take * (page - 1); i++)
            {
                values.Add(null);
            }

            return values;
        }

        public static List<TEntity> GetNullValuesForAfter(int page, int take, int count)
        {
            var values = new List<TEntity>();
            for (int i = 0; i < count - take * page; i++)
            {
                values.Add(null);
            }

            return values;
        }
    }
}
