﻿using AutoMapper;
using CoreShop.Data;
using CoreShop.DTOs;
using CoreShop.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Controllers
{
    [Route("api/[controller]")] // api/products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICoreShopRepository _repository;
        private readonly IMapper _mapper;
        public ProductsController(ICoreShopRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet] // GET api/products
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProducts()
        {
            var products = await _repository.GetAllProductsAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductReadDTO>>(products);

            return Ok(productDtos);
        }

        [HttpGet("{id}", Name = nameof(GetProductById))] // GET api/products/5
        public async Task<ActionResult<ProductReadDTO>> GetProductById(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product is null) return NotFound();

            var productReadDto = _mapper.Map<ProductReadDTO>(product);
            return Ok(productReadDto);
        }

        [HttpPost] // POST api/products
        public async Task<ActionResult<ProductReadDTO>> CreateProduct(ProductCreateDTO productCreateDTO)
        {
            if (productCreateDTO is null) throw new ArgumentNullException(nameof(productCreateDTO));

            var productModel = _mapper.Map<Product>(productCreateDTO);
            _repository.CreateProduct(productModel);

            await _repository.SaveChangesAsync();

            var productReadDto = _mapper.Map<ProductReadDTO>(productModel);

            return CreatedAtRoute(nameof(GetProductById), new { Id = productReadDto.ProductId }, productReadDto);
        }

        [HttpPut("{id}")] // PUT api/products/{id}
        public async Task<IActionResult> UpdateProduct(int id, ProductCreateDTO productCreateDTO)
        {
            var productModelFromRepo = await _repository.GetProductByIdAsync(id);
            if (productModelFromRepo is null)
            {
                return NotFound();
            }

            _mapper.Map(productCreateDTO, productModelFromRepo);

            _repository.UpdateProduct(productModelFromRepo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")] // DELETE api/products/{id}
        public async Task<IActionResult> DeleteProductById(int id)
        {
            var productModelFromRepo = await _repository.GetProductByIdAsync(id);
            if(productModelFromRepo is null)
            {
                return NotFound();
            }

            _repository.DeleteProduct(productModelFromRepo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // PATCH api/products/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialProductUpdate(int id, JsonPatchDocument<ProductCreateDTO> patchDocument)
        {
            var productModelFromRepo = await _repository.GetProductByIdAsync(id);
            if (productModelFromRepo is null)
            {
                return NotFound();
            }

            var productToPatch = _mapper.Map<ProductCreateDTO>(productModelFromRepo);
            patchDocument.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(productToPatch, productModelFromRepo);
            _repository.UpdateProduct(productModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
