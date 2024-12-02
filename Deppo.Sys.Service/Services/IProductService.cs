﻿using Deppo.Sys.Service.DTOs;
using Deppo.Sys.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deppo.Sys.Service.Services;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetAllAsync(HttpClient httpClient);

    public Task<IEnumerable<Product>> GetAllAsync(HttpClient httpClient, string filter);

    public Task<Product> CreateAsync(HttpClient httpClient, ProductDto dto);

}
