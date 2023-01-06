namespace Sales.Services
{
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Common.Models;
    using Sales.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiService
    { 
        public async Task<Response> CheckConnection()
        {
            /* Verifica si hay conexión a internet*/
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet
                };
            }

            /* Realiza una conexión a internet */
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");

            /* Verifica si se puede establecer una conexión de internet */
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.InternetConnection
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok"
            };
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);

                var url = $"{prefix}{controller}";
                var response = await httpClient.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
