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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="vasialek_onbalance")]
	public partial class OrganizationDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertOrganization(Organization instance);
    partial void UpdateOrganization(Organization instance);
    partial void DeleteOrganization(Organization instance);
    #endregion
		
		public OrganizationDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["vasialek_onbalanceConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public OrganizationDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OrganizationDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OrganizationDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public OrganizationDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Organization> Organizations
		{
			get
			{
				return this.GetTable<Organization>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="vasialek_onbalance_user.organization")]
	public partial class Organization : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private byte _StatusId;
		
		private string _Name;
		
		private System.DateTime _CreatedAt;
		
		private System.Nullable<System.DateTime> _UpdatedAt;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnStatusIdChanging(byte value);
    partial void OnStatusIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnCreatedAtChanging(System.DateTime value);
    partial void OnCreatedAtChanged();
    partial void OnUpdatedAtChanging(System.Nullable<System.DateTime> value);
    partial void OnUpdatedAtChanged();
    #endregion
		
		public Organization()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="name", Storage="_Name", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="created_at", Storage="_CreatedAt", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedAt
		{
			get
			{
				return this._CreatedAt;
			}
			set
			{
				if ((this._CreatedAt != value))
				{
					this.OnCreatedAtChanging(value);
					this.SendPropertyChanging();
					this._CreatedAt = value;
					this.SendPropertyChanged("CreatedAt");
					this.OnCreatedAtChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="updated_at", Storage="_UpdatedAt", DbType="DateTime")]
		public System.Nullable<System.DateTime> UpdatedAt
		{
			get
			{
				return this._UpdatedAt;
			}
			set
			{
				if ((this._UpdatedAt != value))
				{
					this.OnUpdatedAtChanging(value);
					this.SendPropertyChanging();
					this._UpdatedAt = value;
					this.SendPropertyChanged("UpdatedAt");
					this.OnUpdatedAtChanged();
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
}
#pragma warning restore 1591
