using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnBalance.Models
{
    //[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="vasialek_onbalance")]
    public class ProductDataContext : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        void OnCreated(){}
        void InsertProduct(Product instance) { }
        void UpdateProduct(Product instance) { }
        void DeleteProduct(Product instance) { }
        void InsertProductDetail(ProductDetail instance) { }
        void UpdateProductDetail(ProductDetail instance) { }
        void DeleteProductDetail(ProductDetail instance) { }
        #endregion

        public ProductDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ConnectionString, mappingSource)
        {
            OnCreated();
        }

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
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "product")]
    public class Product : INotifyPropertyChanging, INotifyPropertyChanged
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

        private EntitySet<ProductDetail> _ProductDetails;

        #region Extensibility Method Definitions
        void OnLoaded(){}
        void OnValidate(System.Data.Linq.ChangeAction action) { }
        void OnCreated(){}
        void OnidChanging(int value) { }
        void OnidChanged(){}
        void Onstatus_idChanging(byte value) { }
        void Onstatus_idChanged(){}
        void Onpos_idChanging(int value) { }
        void Onpos_idChanged(){}
        void Oninternal_codeChanging(string value) { }
        void Oninternal_codeChanged(){}
        void OnuidChanging(string value) { }
        void OnuidChanged(){}
        void Onuser_idChanging(string value) { }
        void Onuser_idChanged(){}
        void OnnameChanging(string value) { }
        void OnnameChanged(){}
        void OnpriceChanging(decimal value) { }
        void OnpriceChanged(){}
        void Oncreated_atChanging(System.DateTime value) { }
        void Oncreated_atChanged(){}
        #endregion

        public Product()
        {
            this._ProductDetails = new EntitySet<ProductDetail>(new Action<ProductDetail>(this.attach_ProductDetails), new Action<ProductDetail>(this.detach_ProductDetails));
            OnCreated();
        }

        [ScaffoldColumn(false)]
        public string StatusName
        {
            get
            {
                return ((Status)status_id).ToString();
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        [HiddenInput(DisplayValue = false)]
        public int id
        {
            get
            {
                return this._id;
            }
            set
            {
                if((this._id != value))
                {
                    this.OnidChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("id");
                    this.OnidChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_status_id", DbType = "TinyInt NOT NULL")]
        public byte status_id
        {
            get
            {
                return this._status_id;
            }
            set
            {
                if((this._status_id != value))
                {
                    this.Onstatus_idChanging(value);
                    this.SendPropertyChanging();
                    this._status_id = value;
                    this.SendPropertyChanged("status_id");
                    this.Onstatus_idChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_pos_id", DbType = "Int NOT NULL")]
        [HiddenInput(DisplayValue = false)]
        public int pos_id
        {
            get
            {
                return this._pos_id;
            }
            set
            {
                if((this._pos_id != value))
                {
                    this.Onpos_idChanging(value);
                    this.SendPropertyChanging();
                    this._pos_id = value;
                    this.SendPropertyChanged("pos_id");
                    this.Onpos_idChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_internal_code", DbType = "VarChar(64) NOT NULL", CanBeNull = false)]
        public string internal_code
        {
            get
            {
                return this._internal_code;
            }
            set
            {
                if((this._internal_code != value))
                {
                    this.Oninternal_codeChanging(value);
                    this.SendPropertyChanging();
                    this._internal_code = value;
                    this.SendPropertyChanged("internal_code");
                    this.Oninternal_codeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_uid", DbType = "VarChar(64) NOT NULL", CanBeNull = false)]
        [ScaffoldColumn(false)]
        public string uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                if((this._uid != value))
                {
                    this.OnuidChanging(value);
                    this.SendPropertyChanging();
                    this._uid = value;
                    this.SendPropertyChanged("uid");
                    this.OnuidChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_user_id", DbType = "VarChar(128) NOT NULL", CanBeNull = false)]
        public string user_id
        {
            get
            {
                return this._user_id;
            }
            set
            {
                if((this._user_id != value))
                {
                    this.Onuser_idChanging(value);
                    this.SendPropertyChanging();
                    this._user_id = value;
                    this.SendPropertyChanged("user_id");
                    this.Onuser_idChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_name", DbType = "VarChar(128) NOT NULL", CanBeNull = false)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                if((this._name != value))
                {
                    this.OnnameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("name");
                    this.OnnameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_price", DbType = "Decimal(8,4) NOT NULL")]
        public decimal price
        {
            get
            {
                return this._price;
            }
            set
            {
                if((this._price != value))
                {
                    this.OnpriceChanging(value);
                    this.SendPropertyChanging();
                    this._price = value;
                    this.SendPropertyChanged("price");
                    this.OnpriceChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_created_at", DbType = "DateTime NOT NULL")]
        [DataType(DataType.Date)]
        public System.DateTime created_at
        {
            get
            {
                return this._created_at;
            }
            set
            {
                if((this._created_at != value))
                {
                    this.Oncreated_atChanging(value);
                    this.SendPropertyChanging();
                    this._created_at = value;
                    this.SendPropertyChanged("created_at");
                    this.Oncreated_atChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Product_ProductDetail", Storage = "_ProductDetails", ThisKey = "id", OtherKey = "product_id")]
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

            foreach(var size in ProductRepository.GetAvailableNames("size"))
            {
                ar[size] = GetQuantityForSize(size);
            }

            return ar;
        }

        public decimal GetPriceForSize(string size)
        {
            decimal price = 0m;
            var p = ProductDetails.SingleOrDefault(x => x.product_id == this.id && x.parameter_name.Equals("size") && x.parameter_value.Equals(size));
            if(p != null)
            {
                price = p.price_release_minor;
            }

            return price;
        }

        public int GetQuantityForSize(string size)
        {
            //Log.DebugFormat("Searching for product #{0} quantity for size {1}...", this.id, size);
            int qnt = 0;
            var p = ProductDetails.SingleOrDefault(x => x.product_id == this.id && x.parameter_name.Equals("size") && x.parameter_value.Equals(size));
            if(p != null)
            {
                qnt = p.quantity;
            }

            //Log.DebugFormat("Got quantity (probably): {0}", qnt);
            return qnt;
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "product_detail")]
    public partial class ProductDetail : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _id;

        private byte _status_id;

        private int _product_id;

        private string _parameter_name;

        private string _parameter_value;

        private decimal _price_minor;

        private decimal _price_release_minor;

        private int _quantity;

        private System.DateTime _updated_at;

        private System.DateTime _created_at;

        private EntityRef<Product> _Product;

        #region Extensibility Method Definitions
        void OnLoaded() { }
        void OnValidate(System.Data.Linq.ChangeAction action) { }
        void OnCreated(){}
        void OnidChanging(int value){}
        void OnidChanged(){}
        void Onstatus_idChanging(byte value){}
        void Onstatus_idChanged(){}
        void Onproduct_idChanging(int value){}
        void Onproduct_idChanged(){}
        void Onparameter_nameChanging(string value){}
        void Onparameter_nameChanged(){}
        void Onparameter_valueChanging(string value){}
        void Onparameter_valueChanged(){}
        void Onprice_minorChanging(decimal value){}
        void Onprice_minorChanged(){}
        void Onprice_release_minorChanging(decimal value){}
        void Onprice_release_minorChanged(){}
        void OnquantityChanging(int value){}
        void OnquantityChanged(){}
        void Onupdated_atChanging(System.DateTime value){}
        void Onupdated_atChanged(){}
        void Oncreated_atChanging(System.DateTime value){}
        void Oncreated_atChanged(){}
        #endregion

        public ProductDetail()
        {
            this._Product = default(EntityRef<Product>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        [HiddenInput(DisplayValue = false)]
        public int id
        {
            get
            {
                return this._id;
            }
            set
            {
                if((this._id != value))
                {
                    this.OnidChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("id");
                    this.OnidChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_status_id", DbType = "TinyInt NOT NULL")]
        public byte status_id
        {
            get
            {
                return this._status_id;
            }
            set
            {
                if((this._status_id != value))
                {
                    this.Onstatus_idChanging(value);
                    this.SendPropertyChanging();
                    this._status_id = value;
                    this.SendPropertyChanged("status_id");
                    this.Onstatus_idChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_product_id", DbType = "Int NOT NULL")]
        [HiddenInput(DisplayValue = false)]
        public int product_id
        {
            get
            {
                return this._product_id;
            }
            set
            {
                if((this._product_id != value))
                {
                    if(this._Product.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.Onproduct_idChanging(value);
                    this.SendPropertyChanging();
                    this._product_id = value;
                    this.SendPropertyChanged("product_id");
                    this.Onproduct_idChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_parameter_name", DbType = "VarChar(128) NOT NULL", CanBeNull = false)]
        public string parameter_name
        {
            get
            {
                return this._parameter_name;
            }
            set
            {
                if((this._parameter_name != value))
                {
                    this.Onparameter_nameChanging(value);
                    this.SendPropertyChanging();
                    this._parameter_name = value;
                    this.SendPropertyChanged("parameter_name");
                    this.Onparameter_nameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_parameter_value", DbType = "VarChar(256) NOT NULL", CanBeNull = false)]
        public string parameter_value
        {
            get
            {
                return this._parameter_value;
            }
            set
            {
                if((this._parameter_value != value))
                {
                    this.Onparameter_valueChanging(value);
                    this.SendPropertyChanging();
                    this._parameter_value = value;
                    this.SendPropertyChanged("parameter_value");
                    this.Onparameter_valueChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_price_minor", DbType = "Decimal(10,4) NOT NULL")]
        public decimal price_minor
        {
            get
            {
                return this._price_minor;
            }
            set
            {
                if((this._price_minor != value))
                {
                    this.Onprice_minorChanging(value);
                    this.SendPropertyChanging();
                    this._price_minor = value;
                    this.SendPropertyChanged("price_minor");
                    this.Onprice_minorChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_price_release_minor", DbType = "Decimal(10,4) NOT NULL")]
        public decimal price_release_minor
        {
            get
            {
                return this._price_release_minor;
            }
            set
            {
                if((this._price_release_minor != value))
                {
                    this.Onprice_release_minorChanging(value);
                    this.SendPropertyChanging();
                    this._price_release_minor = value;
                    this.SendPropertyChanged("price_release_minor");
                    this.Onprice_release_minorChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_quantity", DbType = "Int NOT NULL")]
        public int quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                if((this._quantity != value))
                {
                    this.OnquantityChanging(value);
                    this.SendPropertyChanging();
                    this._quantity = value;
                    this.SendPropertyChanged("quantity");
                    this.OnquantityChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_updated_at", DbType = "DateTime NOT NULL")]
        public System.DateTime updated_at
        {
            get
            {
                return this._updated_at;
            }
            set
            {
                if((this._updated_at != value))
                {
                    this.Onupdated_atChanging(value);
                    this.SendPropertyChanging();
                    this._updated_at = value;
                    this.SendPropertyChanged("updated_at");
                    this.Onupdated_atChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_created_at", DbType = "DateTime NOT NULL")]
        public System.DateTime created_at
        {
            get
            {
                return this._created_at;
            }
            set
            {
                if((this._created_at != value))
                {
                    this.Oncreated_atChanging(value);
                    this.SendPropertyChanging();
                    this._created_at = value;
                    this.SendPropertyChanged("created_at");
                    this.Oncreated_atChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Product_ProductDetail", Storage = "_Product", ThisKey = "product_id", OtherKey = "id", IsForeignKey = true)]
        public Product Product
        {
            get
            {
                return this._Product.Entity;
            }
            set
            {
                Product previousValue = this._Product.Entity;
                if(((previousValue != value)
                            || (this._Product.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if((previousValue != null))
                    {
                        this._Product.Entity = null;
                        previousValue.ProductDetails.Remove(this);
                    }
                    this._Product.Entity = value;
                    if((value != null))
                    {
                        value.ProductDetails.Add(this);
                        this._product_id = value.id;
                    } else
                    {
                        this._product_id = default(int);
                    }
                    this.SendPropertyChanged("Product");
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
    }
}
