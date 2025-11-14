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
                {
                    Log.Warning("Add failed: Title is empty");
                    throw new ArgumentException("Title is empty!");
                }
                if (item.title.Length > 50)
                {
                    Log.Warning("Add failed: Title too long ({Length} chars)", item.title.Length);
                    throw new ArgumentException("Title exceeds max length 50!");
                }
                Log.Information("Adding ToDoItem: {Title}", item.title);
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
                    Log.Warning("Delete failed: Item with ID {Id} not found", id);
                    return false;
                }
                Log.Information("Deleting ToDoItem ID {Id}", id);
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
                Log.Information("Retrieving all ToDo items");
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
                var item = _repo.GetById(id);

                if (item == null)
                {
                    Log.Warning("GetById: Item with ID {Id} not found", id);
                }
                else
                {
                    Log.Information("GetById: Found item with ID {Id}", id);
                }
                return item;
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
                    Log.Warning("Update failed: Item with ID {Id} not found", id);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(item.title))
                {
                    Log.Warning("Update failed: Title is empty");
                    throw new ArgumentException("Title cannot be empty!");
                }

                if (item.title.Length > 50)
                {
                    Log.Warning("Update failed: Title too long ({Length} chars)", item.title.Length);
                    throw new ArgumentException("Title exceeds max length 50!");
                }

                Log.Information("Updating ToDoItem ID {Id}: Title='{Title}', IsDone={IsDone}", id, item.title, item.isDone);
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
