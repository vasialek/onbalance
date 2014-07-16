using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OnBalance.Core;

namespace OnBalance.Models
{

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "vasialek_onbalance_user.category")]
    public partial class Category : BaseModel, INotifyPropertyChanging, INotifyPropertyChanged
    {


        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private byte _StatusId;

        private int _ParentId;

        private string _Name;

        private int _OrganizationId;

        private int _categoryTypeId = -1;

        private EntitySet<Product> _Products;

        protected Category _parent = null;

        protected CategoryType _categoryType = null;
        
        protected CategoryStructure[] _categoryStructure = null;

        partial void OnCreated()
        {
            this._statusId = (byte)Status.Unknown;
        }

        public CategoryStructure[] Structure
        {
            get
            {
                if(_categoryStructure == null)
                {
                    _categoryStructure = new CategoryStructureRepository().GetStructure(Id);
                }
                return _categoryStructure;
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

        [ScaffoldColumn(false)]
        [ScriptIgnore]
        public string CategoryTypeName
        {
            get
            {
                if(_categoryType == null)
                {
                    _categoryType = new CategoryTypeRepository().Items.SingleOrDefault(x => x.Id.Equals(_categoryTypeId));
                }

                return _categoryType == null ? "" : _categoryType.Name;
            }
        }

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
        partial void OnCategoryTypeIdChanging(int value);
        partial void OnCategoryTypeIdChanged();
        #endregion

        public Category()
        {
            this._Products = new EntitySet<Product>(new Action<Product>(this.attach_Products), new Action<Product>(this.detach_Products));
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "id", Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        [Required]
        [HiddenInput]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "status_id", Storage = "_StatusId", DbType = "TinyInt NOT NULL")]
        [UIHint("MyStatus")]
        public byte StatusId
        {
            get
            {
                return this._StatusId;
            }
            set
            {
                if((this._StatusId != value))
                {
                    this.OnStatusIdChanging(value);
                    this.SendPropertyChanging();
                    this._StatusId = value;
                    this.SendPropertyChanged("StatusId");
                    this.OnStatusIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.Column(Name = "category_type_id", Storage = "_categoryTypeId", DbType = "Int NOT NULL")]
        [Range(1, 4, ErrorMessage = "Please choose correct Category type!")]
        [UIHint("CategoryTypeId")]
        public int CategoryTypeId
        {
            get
            {
                return _categoryTypeId;
            }

            set
            {
                if(_categoryTypeId != value)
                {
                    this.OnCategoryTypeIdChanging(value);
                    this.SendPropertyChanging();
                    _categoryTypeId = value;
                    this.SendPropertyChanged("CategoryId");
                    this.OnCategoryTypeIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "parent_id", Storage = "_ParentId", DbType = "Int NOT NULL")]
        [Required]
        public int ParentId
        {
            get
            {
                return this._ParentId;
            }
            set
            {
                if((this._ParentId != value))
                {
                    this.OnParentIdChanging(value);
                    this.SendPropertyChanging();
                    this._ParentId = value;
                    this.SendPropertyChanged("ParentId");
                    this.OnParentIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "name", Storage = "_Name", DbType = "NVarChar(128) NOT NULL", CanBeNull = false)]
        [Required, StringLength(256, MinimumLength = 5)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "organization_id", Storage = "_OrganizationId", DbType = "Int NOT NULL")]
        [Required]
        public int OrganizationId
        {
            get
            {
                return this._OrganizationId;
            }
            set
            {
                if((this._OrganizationId != value))
                {
                    this.OnOrganizationIdChanging(value);
                    this.SendPropertyChanging();
                    this._OrganizationId = value;
                    this.SendPropertyChanged("OrganizationId");
                    this.OnOrganizationIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Category_Product", Storage = "_Products", ThisKey = "Id", OtherKey = "CategoryId")]
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