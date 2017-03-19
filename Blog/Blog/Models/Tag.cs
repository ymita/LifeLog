namespace Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tag
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? Post_ID { get; set; }

        public virtual Post Post { get; set; }
    }
}
