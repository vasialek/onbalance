using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Abstract
{
    public interface IProductRepository
    {

        IQueryable<Product> Products { get; }

    }
}
