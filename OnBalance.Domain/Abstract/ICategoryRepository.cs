using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Abstract
{

    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
    }

}
