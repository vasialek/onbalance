﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
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
	public partial class ObLogDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ObLogDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["vasialek_onbalanceConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ObLogDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ObLogDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ObLogDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ObLogDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ObLog> ObLogs
		{
			get
			{
				return this.GetTable<ObLog>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="vasialek_onbalance_user.ob_log")]
	public partial class ObLog
	{
		
		private int _Id;
		
		private System.DateTime _Date;
		
		private string _Thread;
		
		private string _Level;
		
		private string _Logger;
		
		private string _Message;
		
		private string _Exception;
		
		public ObLog()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.Always, DbType="Int NOT NULL IDENTITY", IsDbGenerated=true)]
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
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this._Date = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Thread", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		public string Thread
		{
			get
			{
				return this._Thread;
			}
			set
			{
				if ((this._Thread != value))
				{
					this._Thread = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Level]", Storage="_Level", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				if ((this._Level != value))
				{
					this._Level = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Logger", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		public string Logger
		{
			get
			{
				return this._Logger;
			}
			set
			{
				if ((this._Logger != value))
				{
					this._Logger = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="VarChar(4000) NOT NULL", CanBeNull=false)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this._Message = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Exception", DbType="VarChar(2000)")]
		public string Exception
		{
			get
			{
				return this._Exception;
			}
			set
			{
				if ((this._Exception != value))
				{
					this._Exception = value;
				}
			}
		}
	}
}
#pragma warning restore 1591