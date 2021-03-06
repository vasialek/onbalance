﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace OnBalance.Models
{

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "vasialek_onbalance_user.product_detail")]
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

        private string _data_json;

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
        partial void Ondata_jsonChanging(string value);
        partial void Ondata_jsonChanged();
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
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

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_data_json", DbType = "VarChar(512) NOT NULL", CanBeNull = false)]
        public string DataJson
        {
            get
            {
                return this._data_json;
            }
            set
            {
                if ((this._data_json != value))
                {
                    this.Ondata_jsonChanging(value);
                    this.SendPropertyChanging();
                    this._data_json = value;
                    this.SendPropertyChanged("data_json");
                    this.Ondata_jsonChanged();
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

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Product_ProductDetail", Storage = "_Product", ThisKey = "product_id", OtherKey = "Id", IsForeignKey = true, DeleteOnNull = true)]
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
                        this._product_id = value.Id;
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

}