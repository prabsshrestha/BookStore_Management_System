﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
       //T - Category 
       IEnumerable<T> GetAll(string? includeProperties = null);
       T Get(Expression<Func<T,bool>> filter, string? includeProperties = null);
       void Add(T entity);
       void Remove(T entity);
       void RemoveRanger(IEnumerable<T> entitiy);
       bool Any(Expression<Func<T, bool>> entitiy);
    }
}
