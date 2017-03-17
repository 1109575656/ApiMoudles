using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using DataLayer.Model;

namespace BusinessLayer.BaseRepository
{

    public interface IGenericRepository<T> where T : class
    {
        ModelContext Context { get; }

        Table<T> GetTable();

        IQueryable<T> AsQuerable();

        /// <summary>
        /// Find all records
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<T> FindAll();


        /// <summary>
        /// Get single record by condition with sort.
        /// </summary>
        /// <typeparam name="TSort"></typeparam>
        /// <param name="order"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        T FindOne<TSort>(Expression<Func<T, TSort>> order, Expression<Func<T, bool>> condition);
        T FindOne<TSort>(string order, string condition, object[] parameters);

        /// <summary>
        /// Get one record by condition
        /// </summary>
        /// <param name="condition">Filter condition</param>
        /// <returns>T</returns>
        T FindOne(Expression<Func<T, bool>> condition);

        T FindOne(string condition, object[] parameters);

        #region Dynamic Query
        IQueryable<T> Query(string filter, object[] parameters = null);
        IQueryable<T> Query(string filter, string order, object[] parameters = null);
        IQueryable<T> Query(string filter, string order, int skip, int take, object[] parameters = null);
        IQueryable<T> Query(string filter, Expression<Func<T, bool>> whereClause, string order, object[] parameters = null);
        IQueryable<T> Query(string filter, Expression<Func<T, bool>> whereClause, string order, int skip, int take, object[] parameters = null);
        #endregion

        #region LINQ Query
        IQueryable<T> Query(Expression<Func<T, bool>> filter);
        IQueryable<T> Query<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order);
        IQueryable<T> Query(Expression<Func<T, bool>> filter, string order);
        IQueryable<T> Query<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order, int skip, int take);
        IQueryable<T> Query(Expression<Func<T, bool>> filter, string order, int skip, int take);
        IQueryable<T> QueryDescending<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order, int skip, int take);
        IQueryable<T> QueryDescending<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order);
        #endregion



        /// <summary>
        /// Records count
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>IQueryable</returns>
        long TotalRecords(Expression<Func<T, bool>> filter);
        long TotalRecords(string filter, object[] parameters);
        long TotalRecords(string filter, Expression<Func<T, bool>> wherClause, object[] parameters);

        bool IsExist(Expression<Func<T, bool>> condition);

        int? MaxId(Expression<Func<T, bool>> filter, Expression<Func<T, int?>> column);
        int? MaxId(Expression<Func<T, int?>> column);



        /// <summary>
        /// Add record
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>T</returns>
        T Insert(T entity);

        IEnumerable<T> InsertMulti(IEnumerable<T> entities);

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Deleted or not</returns>
        bool Delete(T entity);

        /// <summary>
        /// Delete record by condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>Deleted or not</returns>
        bool Delete(Expression<Func<T, bool>> condition);
        bool Delete(string condition, object[] parameters);

        /// <summary>
        /// Delete records
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <returns>Deleted or not</returns>
        bool DeleteMulti(Expression<Func<T, bool>> condition);
        bool DeleteMulti(string condition, object[] parameters);
        bool DeleteMulti(IEnumerable<T> entities);


        #region 需要单独实现
        /// <summary>
        /// Update record and submit
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Updated or not</returns>
        bool Update(T entity);

        /// <summary>
        /// reset value
        /// </summary>
        void Update(T from, T to);

        /// <summary>
        /// Update record and submit
        /// </summary>
        /// <param name="filter">query filter</param>
        /// <param name="entity">entity</param>
        /// <returns>updated</returns>
        bool Update(Expression<Func<T, bool>> filter, T entity);

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        Tuple<bool, string> IsValid(T entity);


        /// <summary>
        /// 此条记录是否重复
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        bool IsRepeat(T entity);
        #endregion

        void Save();
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public ModelContext Context { get; private set; }
        private T Entity { get; set; }

        public GenericRepository()
        {
            this.Context = new ModelContext();
            this.Entity = default(T);
        }

        public GenericRepository(ModelContext context)
        {
            this.Context = context;
            this.Entity = default(T);
        }

        public Table<T> GetTable()
        {
            return this.Context.GetTable<T>();
        }
        public void Save()
        {
            this.Context.SubmitChanges();
        }

        public IQueryable<T> AsQuerable()
        {
            return this.GetTable();
        }

        public IQueryable<T> FindAll()
        {
            return this.AsQuerable();
        }


        public T FindOne<TSort>(Expression<Func<T, TSort>> order, Expression<Func<T, bool>> condition)
        {
            return this.QueryDescending(condition, order).FirstOrDefault();
        }

        public T FindOne<TSort>(string order, string condition, object[] parameters)
        {
            return this.Query(condition, order, parameters).FirstOrDefault();
        }

        public T FindOne(string condition, object[] parameters)
        {
            return this.Query(condition, parameters).FirstOrDefault();
        }

        public T FindOne(Expression<Func<T, bool>> condition)
        {
            return this.AsQuerable().FirstOrDefault(condition);
        }

        #region Delete
        public bool Delete(T entity)
        {
            this.GetTable().DeleteOnSubmit(entity);
            return true;
        }

        public bool Delete(Expression<Func<T, bool>> condition)
        {
            var entity = this.FindOne(condition);
            if (entity == null) return false;
            this.GetTable().DeleteOnSubmit(entity);
            return true;
        }

        public bool DeleteMulti(Expression<Func<T, bool>> condition)
        {
            var entities = this.Query(condition);
            this.GetTable().DeleteAllOnSubmit(entities);
            return true;
        }

        public bool DeleteMulti(IEnumerable<T> entities)
        {
            this.GetTable().DeleteAllOnSubmit(entities);
            return true;
        }

        public bool Delete(string condition, object[] parameters)
        {
            var entity = this.FindOne(condition, parameters);
            if (entity == null) return false;
            this.GetTable().DeleteOnSubmit(entity);
            return true;
        }

        public bool DeleteMulti(string condition, object[] parameters)
        {

            var entities = this.Query(condition, parameters);
            this.GetTable().DeleteAllOnSubmit(entities);
            return true;
        }
        #endregion

        public T Insert(T entity)
        {
            this.GetTable().InsertOnSubmit(entity);
            return entity;
        }

        public IEnumerable<T> InsertMulti(IEnumerable<T> entities)
        {
            this.GetTable().InsertAllOnSubmit(entities);
            return entities;
        }

        #region 需要单独实现

        public bool Update(Expression<Func<T, bool>> filter, T entity)
        {
            var query = this.FindOne(filter);
            if (query == null) return false;
            this.Update(entity, query);
            this.Save();
            return true;
        }
        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T from, T to)
        {
            //Bug 借助ValueInject赋值实现
            throw new NotImplementedException();
        }

        public Tuple<bool, string> IsValid(T entity)
        {
            return Tuple.Create(true, "");
        }


        public bool IsRepeat(T entity)
        {
            return false;
        }
        #endregion

        #region LINQ Query
        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            return this.AsQuerable().Where(filter);
        }

        public IQueryable<T> Query<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order)
        {
            return this.Query(filter).OrderBy(order);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> filter, string order)
        {
            return this.Query(filter).OrderBy(order);
        }

        public IQueryable<T> QueryDescending<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order)
        {
            return this.Query(filter).OrderByDescending(order);
        }
        public IQueryable<T> Query(Expression<Func<T, bool>> filter, string order, int skip, int take)
        {
            return this.Query(filter, order).Skip(skip).Take(take);
        }

        public IQueryable<T> Query<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order, int skip, int take)
        {
            return this.Query(filter).OrderBy(order).Skip(skip).Take(take);
        }
        public IQueryable<T> QueryDescending<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> order, int skip, int take)
        {
            return this.Query(filter).OrderByDescending(order).Skip(skip).Take(take);
        }

        #endregion


        #region Dynamic Query
        public IQueryable<T> Query(string filter, object[] parameters)
        {
            return this.AsQuerable().Where(filter, parameters);
        }



        public IQueryable<T> Query(string filter, string order, object[] parameters = null)
        {
            return this.Query(filter, parameters).OrderBy(order);

        }



        public IQueryable<T> Query(string filter, Expression<Func<T, bool>> whereClause, string order,
            object[] parameters = null)
        {
            return this.Query(filter, order, parameters).Where(whereClause);
        }

        public IQueryable<T> Query(string filter, string order, int skip, int take, object[] parameters = null)
        {
            return this.Query(filter, order, parameters).Skip(skip).Take(take).OrderBy(order);
        }

        public IQueryable<T> Query(string filter, Expression<Func<T, bool>> whereClause, string order, int skip, int take,
            object[] parameters = null)
        {
            return this.Query(filter, whereClause, order, parameters).Skip(skip).Take(take);
        }



        #endregion


        #region Utils
        public bool IsExist(Expression<Func<T, bool>> condition)
        {
            return this.AsQuerable().Any(condition);
        }

        public long TotalRecords(Expression<Func<T, bool>> filter)
        {
            return this.AsQuerable().Where(filter).LongCount();
        }

        public long TotalRecords(string filter, object[] parameters)
        {
            return this.AsQuerable().Where(filter, parameters).LongCount();
        }

        public long TotalRecords(string filter, Expression<Func<T, bool>> whereClause, object[] parameters)
        {
            return this.AsQuerable().Where(filter, parameters).Where(whereClause).LongCount();
        }

        public int? MaxId(Expression<Func<T, bool>> filter, Expression<Func<T, int?>> column)
        {
            return this.Query(filter).Max(column);
        }
        public int? MaxId(Expression<Func<T, int?>> column)
        {
            return this.AsQuerable().Max(column);
        }

        #endregion


    }

}
