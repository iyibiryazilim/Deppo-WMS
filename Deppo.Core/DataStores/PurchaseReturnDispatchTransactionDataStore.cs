﻿using Deppo.Core.DataResultModel;
using Deppo.Core.DTOs.PurchaseDispatchTransaction;
using Deppo.Core.DTOs.PurchaseReturnDispatchTransaction;
using Deppo.Core.ResponseResultModels;
using Deppo.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deppo.Core.DataStores
{
    public class PurchaseReturnDispatchTransactionDataStore : IPurchaseReturnDispatchTransactionService
    {
        public async Task<DataResult<ResponseModel>> InsertPurchaseReturnDispatchTransaction(HttpClient httpClient, int firmNumber, PurchaseReturnDispatchTransactionInsert dto)
        {
            var postUrl = $"/gateway/purchase/PurchaseReturnDispatchTransaction/Tiger?firmNumber={firmNumber}";
            var result = new DataResult<ResponseModel>();
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");


                var responseMessage = await httpClient.PostAsync($"{postUrl}", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var data = await responseMessage.Content.ReadAsStringAsync();
                    var dtos = data.Trim('"').Replace("\\\"", "\"").Replace("\\", "");
                    var dtos2 = JsonConvert.DeserializeObject<DataResult<ResponseModel>>(dtos);
                    result.Message = dtos2.Message;
                    result.IsSuccess = dtos2.IsSuccess;
                    result.Data = dtos2.Data;
                    return result;

                }
                else
                {

                    var message = await responseMessage.Content.ReadAsStringAsync();
                    result.Message = message;
                    result.IsSuccess = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new DataResult<ResponseModel>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

    }
}