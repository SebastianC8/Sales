namespace Sales.Services
{
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Common.Models;
    using Sales.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
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

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

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

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);

                var url = $"{prefix}{controller}";
                var response = await httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
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

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{prefix}{controller}";
                var response = await httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
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

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);

                var url = $"{prefix}{controller}/{id}";
                var response = await httpClient.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
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

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{prefix}{controller}/{id}";
                var response = await httpClient.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
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

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);

                var url = $"{prefix}{controller}/{id}";
                var response = await httpClient.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                return new Response
                {
                    IsSuccess = true
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

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{prefix}{controller}/{id}";
                var response = await httpClient.DeleteAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                return new Response
                {
                    IsSuccess = true
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

        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);

                var response = await httpClient.PostAsync(
                    "Token",
                    new StringContent(
                        $"grant_type=password&username={username}&password={password}",
                        Encoding.UTF8, "application/x-www-form-urlencoded"
                    )
                );

                var result = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return token;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(urlBase);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{prefix}{controller}";
                var response = await httpClient.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

    }
}
