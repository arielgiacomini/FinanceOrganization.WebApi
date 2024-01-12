﻿using App.Forms.Services.Output;
using App.Forms.ViewModel;
using Newtonsoft.Json;
using System.Text;

namespace App.Forms.Services
{
    public static class BillToPayServices
    {
        public static async Task<CreateBillToPayOutput> CreateBillToPay(CreateBillToPayViewModel createBillToPayViewModel)
        {
            using var client = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(createBillToPayViewModel), Encoding.UTF8, "application/json");

            var result = client.PostAsync($"http://api.financeiro.arielgiacomini.com.br/v1/bills-to-pay/register", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                return new CreateBillToPayOutput();
            }

            var response = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CreateBillToPayOutput>(response) ?? new CreateBillToPayOutput();
        }

        public static async Task<SearchBillToPayOutput> SearchBillToPay(SearchBillToPayViewModel searchBillToPayViewModel)
        {
            using var client = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(searchBillToPayViewModel), Encoding.UTF8, "application/json");

            var result = client.PostAsync($"http://api.financeiro.arielgiacomini.com.br/v1/bills-to-pay/search", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                return new SearchBillToPayOutput();
            }

            var response = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SearchBillToPayOutput>(response) ?? new SearchBillToPayOutput();
        }

        public static async Task<PayBillToPayOutput> PayBillToPay(PayBillToPayViewModel payBillToPayViewModel)
        {
            using var client = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(payBillToPayViewModel), Encoding.UTF8, "application/json");

            var result = client.PatchAsync($"http://api.financeiro.arielgiacomini.com.br/v1/bills-to-pay/pay", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                return new PayBillToPayOutput();
            }

            var response = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PayBillToPayOutput>(response) ?? new PayBillToPayOutput();
        }
    }
}