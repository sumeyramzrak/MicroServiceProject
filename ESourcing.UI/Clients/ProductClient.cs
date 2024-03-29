﻿using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;
using Newtonsoft.Json;

namespace ESourcing.UI.Clients
{
    public class ProductClient
    {
        public HttpClient _client { get; } //readonly == { get; } 

        public ProductClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<List<ProductViewModel>>> GetProducts()
        {
            var response = await _client.GetAsync("/Product"); //HttpGet olduğundan GetAsync dedik.
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductViewModel>>(responseData);
                if (result.Any())
                    return new Result<List<ProductViewModel>>(true, ResultConstant.RecordFound, result.ToList());
                return new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);
            }
            return new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);
        }
    }
}
