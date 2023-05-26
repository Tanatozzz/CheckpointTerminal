using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CheckpointTerminal.Models;
using CheckpointTerminal.Singletons;
using Newtonsoft.Json;

namespace CheckpointTerminal.Http
{
    internal class HttpQuery
    {
        private readonly HttpClient _httpClient;

        public HttpQuery()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7213/api/DataBase/");
        }

        public async Task<HttpEmployee> Login(string username, string password)
        {
            var loginData = new { Username = username, Password = password };
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Login_check", loginData);
                if (response.IsSuccessStatusCode)
                {
                    // Вход выполнен успешно, получаем информацию о сотруднике
                    var employee = await response.Content.ReadAsAsync<HttpEmployee>();
                    return employee;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Ошибка аутентификации
                    return null;
                }
                else
                {
                    // Обработка других ошибок, если необходимо
                    throw new Exception($"Ошибка при выполнении запроса: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка отправки запроса на сервер(Login Query). Обратитесь к администратору системы.", ex.ToString());
                return null;
            }
        }

        public async Task<List<CheckpointWithOfficeName>> GetAllCheckpoints()
        {
            var response = await _httpClient.GetAsync("Checkpoint_DataList");

            if (response.IsSuccessStatusCode)
            {
                var checkpoints = await response.Content.ReadAsAsync<List<CheckpointWithOfficeName>>();
                return checkpoints;
            }
            else
            {
                throw new Exception($"Ошибка при выполнении запроса: {response.StatusCode}");
            }
        }
    }
}
