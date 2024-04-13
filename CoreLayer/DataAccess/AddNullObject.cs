using System.Collections.Generic;

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
            if (page == 0)
            {
                page = 1;
            }
            var values = new List<TEntity>();
            for (int i = 0; i < count - take * page; i++)
            {
                values.Add(null);
            }

            return values;
        }

        public static List<TEntity> GetListByPaging(List<TEntity> entities, int take, int page, int count)
        {
            var values = new List<TEntity>();
            if (page > 1)
            {
                values.AddRange(GetNullValuesForBefore(page, take));
            }

            values.AddRange(entities);

            values.AddRange(GetNullValuesForAfter(page, take, count));

            return values;
        }
    }
}
