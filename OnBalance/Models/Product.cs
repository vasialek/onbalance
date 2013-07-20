using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
namespace OnBalance.Models
{

    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "vasialek_onbalance")]
    public partial class ProductDataContext : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertProduct(Product instance);
        partial void UpdateProduct(Product instance);
        partial void DeleteProduct(Product instance);
        partial void InsertProductDetail(ProductDetail instance);
        partial void UpdateProductDetail(ProductDetail instance);
        partial void DeleteProductDetail(ProductDetail instance);
        partial void InsertCategory(Category instance);
        partial void UpdateCategory(Category instance);
        partial void DeleteCategory(Category instance);
        #endregion

        public ProductDataContext(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public ProductDataContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public ProductDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public ProductDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<Product> Products
        {
            get
            {
                return this.GetTable<Product>();
            }
        }

        public System.Data.Linq.Table<ProductDetail> ProductDetails
        {
            get
            {
                return this.GetTable<ProductDetail>();
            }
        }

        public System.Data.Linq.Table<Category> Categories
        {
            get
            {
                return this.GetTable<Category>();
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "vasialek_onbalance_user.product")]
    public partial class Product
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _id;

        private byte _status_id;

        private int _pos_id;

        private string _internal_code;

        private string _uid;

        private string _user_id;

        private string _name;

        private decimal _price;

        private System.DateTime _created_at;

        private int _category_id;

        private EntitySet<ProductDetail> _ProductDetails;

        private EntityRef<Category> _Category;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnStatusIdChanging(byte value);
        partial void OnStatusIdChanged();
        partial void OnPosIdChanging(int value);
        partial void OnPosIdChanged();
        partial void OnInternalCodeChanging(string value);
        partial void OnInternalCodeChanged();
        partial void OnUidChanging(string value);
        partial void OnUidChanged();
        partial void OnUserIdChanging(string value);
        partial void OnUserIdChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnPriceChanging(decimal value);
        partial void OnPriceChanged();
        partial void OnCreatedAtChanging(System.DateTime value);
        partial void OnCreatedAtChanged();
        partial void OnCategoryIdChanging(int value);
        partial void OnCategoryIdChanged();
        #endregion

        public Product()
        {
            this._ProductDetails = new EntitySet<ProductDetail>(new Action<ProductDetail>(this.attach_ProductDetails), new Action<ProductDetail>(this.detach_ProductDetails));
            this._Category = default(EntityRef<Category>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "id", Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if((this._id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "status_id", Storage = "_status_id", DbType = "TinyInt NOT NULL")]
        public byte StatusId
        {
            get
            {
                return this._status_id;
            }
            set
            {
                if((this._status_id != value))
                {
                    this.OnStatusIdChanging(value);
                    this.SendPropertyChanging();
                    this._status_id = value;
                    this.SendPropertyChanged("StatusId");
                    this.OnStatusIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "pos_id", Storage = "_pos_id", DbType = "Int NOT NULL")]
        public int PosId
        {
            get
            {
                return this._pos_id;
            }
            set
            {
                if((this._pos_id != value))
                {
                    this.OnPosIdChanging(value);
                    this.SendPropertyChanging();
                    this._pos_id = value;
                    this.SendPropertyChanged("PosId");
                    this.OnPosIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "internal_code", Storage = "_internal_code", DbType = "VarChar(64) NOT NULL", CanBeNull = false)]
        public string InternalCode
        {
            get
            {
                return this._internal_code;
            }
            set
            {
                if((this._internal_code != value))
                {
                    this.OnInternalCodeChanging(value);
                    this.SendPropertyChanging();
                    this._internal_code = value;
                    this.SendPropertyChanged("InternalCode");
                    this.OnInternalCodeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "uid", Storage = "_uid", DbType = "VarChar(64) NOT NULL", CanBeNull = false)]
        public string Uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                if((this._uid != value))
                {
                    this.OnUidChanging(value);
                    this.SendPropertyChanging();
                    this._uid = value;
                    this.SendPropertyChanged("Uid");
                    this.OnUidChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "user_id", Storage = "_user_id", DbType = "VarChar(128) NOT NULL", CanBeNull = false)]
        public string UserId
        {
            get
            {
                return this._user_id;
            }
            set
            {
                if((this._user_id != value))
                {
                    this.OnUserIdChanging(value);
                    this.SendPropertyChanging();
                    this._user_id = value;
                    this.SendPropertyChanged("UserId");
                    this.OnUserIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "name", Storage = "_name", DbType = "VarChar(128) NOT NULL", CanBeNull = false)]
        [StringLength(256, MinimumLength = 3)]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if((this._name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "price", Storage = "_price", DbType = "Decimal(8,4) NOT NULL")]
        public decimal Price
        {
            get
            {
                return this._price;
            }
            set
            {
                if((this._price != value))
                {
                    this.OnPriceChanging(value);
                    this.SendPropertyChanging();
                    this._price = value;
                    this.SendPropertyChanged("Price");
                    this.OnPriceChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "created_at", Storage = "_created_at", DbType = "DateTime NOT NULL")]
        public System.DateTime CreatedAt
        {
            get
            {
                return this._created_at;
            }
            set
            {
                if((this._created_at != value))
                {
                    this.OnCreatedAtChanging(value);
                    this.SendPropertyChanging();
                    this._created_at = value;
                    this.SendPropertyChanged("CreatedAt");
                    this.OnCreatedAtChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "category_id", Storage = "_category_id", DbType = "Int NOT NULL")]
        public int CategoryId
        {
            get
            {
                return this._category_id;
            }
            set
            {
                if((this._category_id != value))
                {
                    this.OnCategoryIdChanging(value);
                    this.SendPropertyChanging();
                    this._category_id = value;
                    this.SendPropertyChanged("CategoryId");
                    this.OnCategoryIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Product_ProductDetail", Storage = "_ProductDetails", ThisKey = "Id", OtherKey = "product_id")]
        public EntitySet<ProductDetail> ProductDetails
        {
            get
            {
                return this._ProductDetails;
            }
            set
            {
                this._ProductDetails.Assign(value);
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Category_Product", Storage = "_Category", ThisKey = "CategoryId", OtherKey = "Id", IsForeignKey = true)]
        public Category Category
        {
            get
            {
                return this._Category.Entity;
            }
            set
            {
                Category previousValue = this._Category.Entity;
                if(((previousValue != value)
                            || (this._Category.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if((previousValue != null))
                    {
                        this._Category.Entity = null;
                        previousValue.Products.Remove(this);
                    }
                    this._Category.Entity = value;
                    if((value != null))
                    {
                        value.Products.Add(this);
                        this._category_id = value.Id;
                    } else
                    {
                        this._category_id = default(int);
                    }
                    this.SendPropertyChanged("Category");
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void attach_ProductDetails(ProductDetail entity)
        {
            this.SendPropertyChanging();
            entity.Product = this;
        }

        private void detach_ProductDetails(ProductDetail entity)
        {
            this.SendPropertyChanging();
            entity.Product = null;
        }
        
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

        public string PhotosUri
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

        [ScaffoldColumn(false)]
        public string StatusCssStyle
        {
            get
            {
                switch(StatusId)
                {
                    case (byte)Status.Approved:
                        return "label-success";
                    case (byte)Status.Deleted:
                        return "label-inverse";
                }
                return "";
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