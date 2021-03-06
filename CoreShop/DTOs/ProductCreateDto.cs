﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public string SecretDistributor { get; set; }
    }
}
