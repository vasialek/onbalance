﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.ViewModels.Balance
{
    public class OrganizationSchemaViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryStructureViewModel[] Categories { get; set; }

        /// <summary>
        /// List of POS (shops) for current user
        /// </summary>
        public IList<OrganizationViewModel> Shops { get; set; }

        public double ReceivedAt { get; set; }

    }

    public class OrganizationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryStructureViewModel[] Categories { get; set; }
    }

    public class CategoryStructureViewModel
    {
        public string Name { get; set; }
        
        public int Id { get; set; }

        /// <summary>
        /// Gets/sets available sizes for this category
        /// </summary>
        public string[] Sizes { get; set; }
    }
}