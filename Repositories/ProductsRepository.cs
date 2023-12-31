﻿using Microsoft.EntityFrameworkCore;
using Sklepix.Data;
using Sklepix.Data.Entities;

namespace Sklepix.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _context;

        public ProductsRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<ProductEntity> List()
        {
            return _context.Products
                .Select(n => n)
                .ToList();
        }

        public List<ProductEntity> ListInclude()
        {
            return _context.Products
                .Include(m => m.Category)
                .Include(m => m.Aisle)
                .ToList();
        }

        public ProductEntity? One(int id)
        {
            return _context.Products
                .FirstOrDefault(n => n.Id == id);
        }

        public ProductEntity? OneInclude(int id)
        {
            return _context.Products
                .Include(m => m.Category)
                .Include(m => m.Aisle)
                .Include(m => m.Row)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Add(ProductEntity model)
        {
            _context.Products.Add(model);
        }

        public void Edit(int id, ProductEntity model)
        {
            model.Id = id;
            Delete(id);
            Add(model);
        }

        public bool Delete(int id)
        {
            ProductEntity? entity = One(id);
            if(entity == null)
            {
                return false;
            }
            _context.Products.Remove(entity);
            return true;
        }

        public bool Delete(ProductEntity model)
        {
            _context.Products.Remove(model);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public AppDbContext GetRaw()
        {
            return _context;
        }

    }

}
