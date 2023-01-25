﻿using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order:EntityBase
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string EmailAddress { get; set; }
        [MaxLength(200)]
        public string AddressLine { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string? State { get; set; }
        [MaxLength(50)]
        public string? ZipCode { get; set; }

        [MaxLength(30)]
        public string? CardName { get; set; }
        [MaxLength(30)]
        public string? CardNumber { get; set; }
        [MaxLength(20)]
        public string? Expiration { get; set; }
        [MaxLength(10)]
        public string? CVV { get; set; }
        [MaxLength(200)]
        public string? PaymentMethod { get; set; }
    }
}
