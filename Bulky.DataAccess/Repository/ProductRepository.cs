using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            //_db.Products.Update(obj);
            var objfromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objfromDb != null) 
            { 
                objfromDb.Title = obj.Title;
                objfromDb.ISBN = obj.ISBN;
                objfromDb.Price = obj.Price;
                objfromDb.Price50 = obj.Price50;
                objfromDb.Price100 = obj.Price100;
                objfromDb.ListPrice = obj.ListPrice;
                objfromDb.Description = obj.Description;
                objfromDb.CategoryId = obj.CategoryId;
                objfromDb.Author = obj.Author;
            }
            if (obj.ImageUrl != null) 
            {
                objfromDb.ImageUrl = obj.ImageUrl;
            }
        }
    }
}
