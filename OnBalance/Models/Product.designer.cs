﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnBalance.Models
{
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="vasialek_onbalance")]
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
		
		public ProductDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["vasialek_onbalanceConnectionString"].ConnectionString, mappingSource)
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
		
		public System.Data.Linq.Table<Category> Categories
		{
			get
			{
				return this.GetTable<Category>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="vasialek_onbalance_user.product")]
	public partial class Product : INotifyPropertyChanging, INotifyPropertyChanged
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="id", Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="status_id", Storage="_status_id", DbType="TinyInt NOT NULL")]
		public byte StatusId
		{
			get
			{
				return this._status_id;
			}
			set
			{
				if ((this._status_id != value))
				{
					this.OnStatusIdChanging(value);
					this.SendPropertyChanging();
					this._status_id = value;
					this.SendPropertyChanged("StatusId");
					this.OnStatusIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="pos_id", Storage="_pos_id", DbType="Int NOT NULL")]
		public int PosId
		{
			get
			{
				return this._pos_id;
			}
			set
			{
				if ((this._pos_id != value))
				{
					this.OnPosIdChanging(value);
					this.SendPropertyChanging();
					this._pos_id = value;
					this.SendPropertyChanged("PosId");
					this.OnPosIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="internal_code", Storage="_internal_code", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string InternalCode
		{
			get
			{
				return this._internal_code;
			}
			set
			{
				if ((this._internal_code != value))
				{
					this.OnInternalCodeChanging(value);
					this.SendPropertyChanging();
					this._internal_code = value;
					this.SendPropertyChanged("InternalCode");
					this.OnInternalCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="uid", Storage="_uid", DbType="VarChar(64) NOT NULL", CanBeNull=false)]
		public string Uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				if ((this._uid != value))
				{
					this.OnUidChanging(value);
					this.SendPropertyChanging();
					this._uid = value;
					this.SendPropertyChanged("Uid");
					this.OnUidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="user_id", Storage="_user_id", DbType="VarChar(128) NOT NULL", CanBeNull=false)]
		public string UserId
		{
			get
			{
				return this._user_id;
			}
			set
			{
				if ((this._user_id != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._user_id = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="name", Storage="_name", DbType="VarChar(128) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="price", Storage="_price", DbType="Decimal(8,4) NOT NULL")]
		public decimal Price
		{
			get
			{
				return this._price;
			}
			set
			{
				if ((this._price != value))
				{
					this.OnPriceChanging(value);
					this.SendPropertyChanging();
					this._price = value;
					this.SendPropertyChanged("Price");
					this.OnPriceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="created_at", Storage="_created_at", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedAt
		{
			get
			{
				return this._created_at;
			}
			set
			{
				if ((this._created_at != value))
				{
					this.OnCreatedAtChanging(value);
					this.SendPropertyChanging();
					this._created_at = value;
					this.SendPropertyChanged("CreatedAt");
					this.OnCreatedAtChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="category_id", Storage="_category_id", DbType="Int NOT NULL")]
		public int CategoryId
		{
			get
			{
				return this._category_id;
			}
			set
			{
				if ((this._category_id != value))
				{
					this.OnCategoryIdChanging(value);
					this.SendPropertyChanging();
					this._category_id = value;
					this.SendPropertyChanged("CategoryId");
					this.OnCategoryIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Product_ProductDetail", Storage="_ProductDetails", ThisKey="Id", OtherKey="product_id")]
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
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Category_Product", Storage="_Category", ThisKey="CategoryId", OtherKey="Id", IsForeignKey=true)]
		public Category Category
		{
			get
			{
				return this._Category.Entity;
			}
			set
			{
				Category previousValue = this._Category.Entity;
				if (((previousValue != value) 
							|| (this._Category.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Category.Entity = null;
						previousValue.Products.Remove(this);
					}
					this._Category.Entity = value;
					if ((value != null))
					{
						value.Products.Add(this);
						this._category_id = value.Id;
					}
					else
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
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
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
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="vasialek_onbalance_user.product_detail")]
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
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onstatus_idChanging(byte value);
    partial void Onstatus_idChanged();
    partial void Onproduct_idChanging(int value);
    partial void Onproduct_idChanged();
    partial void Onparameter_nameChanging(string value);
    partial void Onparameter_nameChanged();
    partial void Onparameter_valueChanging(string value);
    partial void Onparameter_valueChanged();
    partial void Onprice_minorChanging(decimal value);
    partial void Onprice_minorChanged();
    partial void Onprice_release_minorChanging(decimal value);
    partial void Onprice_release_minorChanged();
    partial void OnquantityChanging(int value);
    partial void OnquantityChanged();
    partial void Onupdated_atChanging(System.DateTime value);
    partial void Onupdated_atChanged();
    partial void Oncreated_atChanging(System.DateTime value);
    partial void Oncreated_atChanged();
    #endregion
		
		public ProductDetail()
		{
			this._Product = default(EntityRef<Product>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_status_id", DbType="TinyInt NOT NULL")]
		public byte status_id
		{
			get
			{
				return this._status_id;
			}
			set
			{
				if ((this._status_id != value))
				{
					this.Onstatus_idChanging(value);
					this.SendPropertyChanging();
					this._status_id = value;
					this.SendPropertyChanged("status_id");
					this.Onstatus_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_product_id", DbType="Int NOT NULL")]
		public int product_id
		{
			get
			{
				return this._product_id;
			}
			set
			{
				if ((this._product_id != value))
				{
					if (this._Product.HasLoadedOrAssignedValue)
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_parameter_name", DbType="VarChar(128) NOT NULL", CanBeNull=false)]
		public string parameter_name
		{
			get
			{
				return this._parameter_name;
			}
			set
			{
				if ((this._parameter_name != value))
				{
					this.Onparameter_nameChanging(value);
					this.SendPropertyChanging();
					this._parameter_name = value;
					this.SendPropertyChanged("parameter_name");
					this.Onparameter_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_parameter_value", DbType="VarChar(256) NOT NULL", CanBeNull=false)]
		public string parameter_value
		{
			get
			{
				return this._parameter_value;
			}
			set
			{
				if ((this._parameter_value != value))
				{
					this.Onparameter_valueChanging(value);
					this.SendPropertyChanging();
					this._parameter_value = value;
					this.SendPropertyChanged("parameter_value");
					this.Onparameter_valueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_price_minor", DbType="Decimal(10,4) NOT NULL")]
		public decimal price_minor
		{
			get
			{
				return this._price_minor;
			}
			set
			{
				if ((this._price_minor != value))
				{
					this.Onprice_minorChanging(value);
					this.SendPropertyChanging();
					this._price_minor = value;
					this.SendPropertyChanged("price_minor");
					this.Onprice_minorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_price_release_minor", DbType="Decimal(10,4) NOT NULL")]
		public decimal price_release_minor
		{
			get
			{
				return this._price_release_minor;
			}
			set
			{
				if ((this._price_release_minor != value))
				{
					this.Onprice_release_minorChanging(value);
					this.SendPropertyChanging();
					this._price_release_minor = value;
					this.SendPropertyChanged("price_release_minor");
					this.Onprice_release_minorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_quantity", DbType="Int NOT NULL")]
		public int quantity
		{
			get
			{
				return this._quantity;
			}
			set
			{
				if ((this._quantity != value))
				{
					this.OnquantityChanging(value);
					this.SendPropertyChanging();
					this._quantity = value;
					this.SendPropertyChanged("quantity");
					this.OnquantityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_updated_at", DbType="DateTime NOT NULL")]
		public System.DateTime updated_at
		{
			get
			{
				return this._updated_at;
			}
			set
			{
				if ((this._updated_at != value))
				{
					this.Onupdated_atChanging(value);
					this.SendPropertyChanging();
					this._updated_at = value;
					this.SendPropertyChanged("updated_at");
					this.Onupdated_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_created_at", DbType="DateTime NOT NULL")]
		public System.DateTime created_at
		{
			get
			{
				return this._created_at;
			}
			set
			{
				if ((this._created_at != value))
				{
					this.Oncreated_atChanging(value);
					this.SendPropertyChanging();
					this._created_at = value;
					this.SendPropertyChanged("created_at");
					this.Oncreated_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Product_ProductDetail", Storage="_Product", ThisKey="product_id", OtherKey="Id", IsForeignKey=true, DeleteOnNull=true)]
		public Product Product
		{
			get
			{
				return this._Product.Entity;
			}
			set
			{
				Product previousValue = this._Product.Entity;
				if (((previousValue != value) 
							|| (this._Product.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Product.Entity = null;
						previousValue.ProductDetails.Remove(this);
					}
					this._Product.Entity = value;
					if ((value != null))
					{
						value.ProductDetails.Add(this);
						this._product_id = value.Id;
					}
					else
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
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="vasialek_onbalance_user.category")]
	public partial class Category : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private byte _StatusId;
		
		private int _ParentId;
		
		private string _Name;
		
		private int _OrganizationId;
		
		private EntitySet<Product> _Products;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnStatusIdChanging(byte value);
    partial void OnStatusIdChanged();
    partial void OnParentIdChanging(int value);
    partial void OnParentIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnOrganizationIdChanging(int value);
    partial void OnOrganizationIdChanged();
    #endregion
		
		public Category()
		{
			this._Products = new EntitySet<Product>(new Action<Product>(this.attach_Products), new Action<Product>(this.detach_Products));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
        [HiddenInput(DisplayValue = false)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="status_id", Storage="_StatusId", DbType="TinyInt NOT NULL")]
        [UIHint("MyStatus")]
		public byte StatusId
		{
			get
			{
				return this._StatusId;
			}
			set
			{
				if ((this._StatusId != value))
				{
					this.OnStatusIdChanging(value);
					this.SendPropertyChanging();
					this._StatusId = value;
					this.SendPropertyChanged("StatusId");
					this.OnStatusIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="parent_id", Storage="_ParentId", DbType="Int NOT NULL")]
		public int ParentId
		{
			get
			{
				return this._ParentId;
			}
			set
			{
				if ((this._ParentId != value))
				{
					this.OnParentIdChanging(value);
					this.SendPropertyChanging();
					this._ParentId = value;
					this.SendPropertyChanged("ParentId");
					this.OnParentIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="name", Storage="_Name", DbType="NVarChar(128) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="organization_id", Storage="_OrganizationId", DbType="Int NOT NULL")]
		public int OrganizationId
		{
			get
			{
				return this._OrganizationId;
			}
			set
			{
				if ((this._OrganizationId != value))
				{
					this.OnOrganizationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganizationId = value;
					this.SendPropertyChanged("OrganizationId");
					this.OnOrganizationIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Category_Product", Storage="_Products", ThisKey="Id", OtherKey="CategoryId")]
		public EntitySet<Product> Products
		{
			get
			{
				return this._Products;
			}
			set
			{
				this._Products.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Products(Product entity)
		{
			this.SendPropertyChanging();
			entity.Category = this;
		}
		
		private void detach_Products(Product entity)
		{
			this.SendPropertyChanging();
			entity.Category = null;
		}
	}
}
#pragma warning restore 1591
