using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBalance.Domain.Entities
{

    public class Category// : BaseModel
    {
        public int Id { get; set; }

        public byte StatusId { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int OrganizationId { get; set; }

        public int CategoryTypeId { get; set; }

        //public EntitySet<Product> _Products { get; set; }

        //[NotMapped]
        //protected Category Parent { get; set; }

        //[NotMapped]
        //public string CategoryTypeName { get; set; }

        //protected CategoryType _categoryType = null { get; set; }
        
        //protected CategoryStructure[] _categoryStructure = null { get; set; }


        public Category()
        {

        }
    }
}
