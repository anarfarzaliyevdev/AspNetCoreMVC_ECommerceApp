﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce_Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }
        public string Name { get; set; }
    }
}
