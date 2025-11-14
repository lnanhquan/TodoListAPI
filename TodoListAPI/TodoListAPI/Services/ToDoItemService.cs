using Serilog;
using TodoListAPI.Models;
using TodoListAPI.Repository;

namespace TodoListAPI.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _repo;
        public ToDoItemService(IToDoItemRepository repo)
        {
            _repo = repo;
        }

        public ToDoItem Add(ToDoItem item)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(item.title))
                    throw new ArgumentException("Title is empty!");
                _repo.Add(item);
                return item;
            }
            catch (Exception ex)
            {
                Log.Error("{ExceptionType} - {Message}", ex.GetType().Name, ex.Message);
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var item = _repo.GetById(id);
                if (item == null)
                {
                    return false;
                }
                _repo.Delete(item);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("{ExceptionType} - {Message}", ex.GetType().Name, ex.Message);
                throw;
            }
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            try
            {
                return _repo.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error("{ExceptionType} - {Message}", ex.GetType().Name, ex.Message);
                throw;
            }
        }

        public ToDoItem? GetById(int id)
        {
            try
            {
                return _repo.GetById(id);
            }
            catch (Exception ex)
            {
                Log.Error("{ExceptionType} - {Message}", ex.GetType().Name, ex.Message);
                throw;
            }
        }

        public bool Update(int id, ToDoItem item)
        {
            try
            {
                var existingItem = _repo.GetById(id);
                if (existingItem == null)
                {
                    return false;
                }
                existingItem.title = item.title;
                existingItem.isDone = item.isDone;
                _repo.Update(existingItem);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("{ExceptionType} - {Message}", ex.GetType().Name, ex.Message);
                throw;
            }
        }
    }
}
