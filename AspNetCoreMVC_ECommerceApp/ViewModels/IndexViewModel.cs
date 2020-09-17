﻿using ECommerce_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC_ECommerceApp.ViewModels
{
    public class IndexViewModel
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> NewArrivalProducts { get; set; }
    }
}
