using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Web.Mvc;

namespace OnBalance.Models
{

    [Table(Name = "organization")]
    [Bind]
    public class Organization
    {

        [Column(Name = "id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id;

        [Column(Name = "status_id")]
        public byte StatusId;

        [Column(Name = "name")]
        public string Name;

        [Column(Name = "created_at")]
        public DateTime CreatedAt;

        [Column(Name = "updated_at")]
        public DateTime? UpdatedAt;
    }
}