using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void IProductRepository.Update(Product pdt)
        {
            var pdtFromDb = _db.Products.FirstOrDefault(u => u.Id == pdt.Id);
            if(pdtFromDb !=null)
            {
                pdtFromDb.Title = pdt.Title;
                pdtFromDb.Description = pdt.Description;
                pdtFromDb.ISBN = pdt.ISBN;
                pdtFromDb.ListPrice = pdt.ListPrice;
                pdtFromDb.Price = pdt.Price;
                pdtFromDb.Price50 = pdt.Price50;
                pdtFromDb.Price100 = pdt.Price100;
                pdtFromDb.CategoryId = pdt.CategoryId;
                if(pdt.ImageUrl != null)
                {
                    pdtFromDb.ImageUrl = pdt.ImageUrl;
                }
            }
                
        }
    }
}
