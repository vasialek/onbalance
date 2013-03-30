using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

namespace OnBalance.Models
{
    [Table(Name = "user_organization")]
    public class UserOrganization
    {

        protected int _id;
        protected string _username;
        protected int _organizationId;

        [Column(Name = "id", Storage = "_id", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert, DbType = "Int not null identity")]
        public int Id { get; set; }

        [Column(Name = "username", Storage = "_username", DbType = "VarChar(128)")]
        public string Username { get; set; }

        [Column(Name = "organization_id", Storage = "_organizationId", DbType = "Int not null")]
        public int OrganizationId { get; set; }
    }
}
