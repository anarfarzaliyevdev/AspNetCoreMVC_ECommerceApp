﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMVC_ECommerceApp.Abstractions;
using AspNetCoreMVC_ECommerceApp.Areas.Admin.ViewModels;
using AspNetCoreMVC_ECommerceApp.Extensions;
using AutoMapper;
using ECommerce_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_ECommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment webHost;
        private readonly IMapper mapper;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductsController(IWebHostEnvironment webHost,IMapper mapper,
            IProductService productService,ICategoryService categoryService)
        {
            this.webHost = webHost;
            this.mapper = mapper;
            this.productService = productService;
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            AddProductViewModel model = new AddProductViewModel();
            model.Categories = await categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(
        [Bind("Name","FormFile","Price","Details", "CategoryId","Categories", "IsFeatured", "IsNewArrival")]AddProductViewModel model)
        {         
            if (ModelState.IsValid)
            {
                Product product = new Product();
                //Create fileName for Photo by using SaveAsync() extension method 
                var fileName = await model.FormFile.SaveAsync(webHost.WebRootPath, "ProductImages");
                model.PhotoPath = fileName;
                // Map properties of viewModel to product
                mapper.Map(model,product);
                //After mapping create product
                var result= await productService.Create(product);
                if (result != null)
                {
                    return RedirectToAction("ManageProducts", "Products");
                }
            }
            model.Categories = await categoryService.GetAll();            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(string id)
        {
            //Get category list
            List<Category> categories = await categoryService.GetAll();
            //Get product by id which come from view 
            Product productById = await productService.GetById(id);
            //Find category of product
            productById.Category = categories.FirstOrDefault(c=>c.CategoryId==productById.CategoryId);
            //Create corresponding view model
            EditProductViewModel model = new EditProductViewModel();
            //Map data
            mapper.Map(productById, model);
            //Fill categories of view model
            model.Categories = categories;
            model.ProductId = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                mapper.Map(model, product);

                var result= await productService.Edit(product);
                if (result)
                {
                    return RedirectToAction("ManageProducts", "Products");
                }
            }
            model.Categories = await categoryService.GetAll();
            return View(model);
        }
        public async Task<IActionResult> ManageProducts()
        {
            ManageProductsViewModel model = new ManageProductsViewModel();
            model.Products = await productService.GetAllProductWithCategory();
            return View(model);
        }
        [HttpPost]
        public JsonResult DeleteProduct(string id)
        {

            try
            {
                productService.Delete(id);
            }
            catch (Exception e)
            {
                return Json(400);
            }

            return Json(200);
        }
    }
}