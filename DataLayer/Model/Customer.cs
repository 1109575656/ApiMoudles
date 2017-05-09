namespace DataLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string LoginName { get; set; }

        [StringLength(16)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(11)]
        public string PhoneNum { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        public int? Age { get; set; }

        [StringLength(3)]
        public string Sex { get; set; }

        [StringLength(100)]
        public string Author { get; set; }
    }
}
