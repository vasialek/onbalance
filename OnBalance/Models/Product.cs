using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace OnBalance.Models
{

    partial class Category : BaseModel
    {
        protected Category _parent = null;
        protected CategoryType _categoryType = null;

        partial void OnCreated()
        {
            this._statusId = (byte)Status.Unknown;
        }

        public CategoryStructure[] Strucutre
        {
            get
            {
                return null;
            }
        }

        public Category Parent
        {
            get
            {
                if(ParentId == 0)
                {
                    return null;
                }

                if(_parent == null)
                {
                    _parent = new ProductRepository().GetCategory(ParentId);
                }

                return _parent;
            }
        }

        public string CategoryTypeName
        {
            get
            {
                if( _categoryType == null )
                {
                    _categoryType = new CategoryTypeRepository().Items.SingleOrDefault(x => x.Id.Equals(_categoryTypeId));
                }

                return _categoryType == null ? "" : _categoryType.Name;
            }
        }
    }

    partial class ProductDetail
    {
        partial void OnCreated()
        {
            this.updated_at = DateTime.Now;
            this.created_at = DateTime.Now;
        }

        partial void OnidChanged()
        {
            this.updated_at = DateTime.Now;
        }
    }

    partial class Product
    {
        protected Organization _organization = null;

        public Organization Pos
        {
            get
            {
                if(_organization == null)
                {
                    _organization = PosId > 0 ? new OrganizationRepository().GetById(PosId) : null;
                }
                return _organization;
            }
        }

        public Uri PhotosUri
        {
            get
            {
                return Pos.Configuration.PhotosUri;
            }
        }

        public string StatusName
        {
            get
            {
                return ((Status)StatusId).ToString();
            }
        }


        public IList<ProductDetail> Details
        {
            get
            {
                if(ProductDetails == null)
                {
                    return new List<ProductDetail>();
                }
                return ProductDetails.ToList<ProductDetail>();
            }
        }

        public string[] DetailsNames
        {
            get
            {
                string[] names = null;
                int total = ProductDetails == null ? 0 : ProductDetails.Count;

                if(total > 0)
                {
                    names = new string[total];
                    for(int i = 0; i < total; i++)
                    {
                        names[i] = ProductDetails[i].parameter_name;
                    }
                }

                return names;
            }
        }

        public Dictionary<string, int> GetQuantityForAllSizes()
        {
            Dictionary<string, int> ar = new Dictionary<string, int>();
            var sizes = new ProductRepository().GetAvailableSizes(this.CategoryId);
            sizes.ToList().ForEach(x => ar.Add(x, 0));
            return ar;
        }

    }
}