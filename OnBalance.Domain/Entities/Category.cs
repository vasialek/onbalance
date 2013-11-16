using System;

namespace OnBalance.Domain.Entities
{

    public class Category : BaseModel
    {
        public int Id { get; set; }

        public byte StatusId { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int PosId { get; set; }

        public int CategoryTypeId { get; set; }

        //public EntitySet<Product> _Products { get; set; }

        protected Category Parent { get; set; }

        public string CategoryTypeName { get; set; }

        //protected CategoryType _categoryType = null { get; set; }
        
        //protected CategoryStructure[] _categoryStructure = null { get; set; }

    }
}
