using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly HttpClient httpClient;

        public TodoListService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<TodoListModel>> GetTodos()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<TodoListModel>>("TodoList");
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<TodoListModel> GetTodoById(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<TodoListModel>($"TodoList/{id}");
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<HttpResponseMessage> CreateTodoItem(TodoListModel todoItem)
        {
            return await httpClient.PostAsJsonAsync("TodoList", todoItem);
        }

        public async Task<HttpResponseMessage> DeleteTodoItem(int id)
        {
            return await httpClient.DeleteAsync($"TodoList/{id}");
        }

        public async Task<HttpResponseMessage> EditTodoItem(TodoListModel todoItem, int id)
        {
            return await httpClient.PutAsJsonAsync($"TodoList/{id}", todoItem);
        }


    }
}
