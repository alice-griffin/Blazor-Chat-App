using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface ITodoListService
    {
        Task<IEnumerable<TodoListModel>> GetTodos();

        Task<TodoListModel> GetTodoById(int id);

        Task<HttpResponseMessage> CreateTodoItem(TodoListModel item);
    }
}
