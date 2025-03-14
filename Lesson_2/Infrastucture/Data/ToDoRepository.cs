using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson_2.Application.Services;
using Lesson_2.Domain.Entities;
using Lesson_2.Domain.Interfaces;
namespace Lesson_2.Infrastucture.Data
{
    public class ToDoRepository:IToDoRepository
    {
        List<TaskItem> taskItems;
        public void Add(TaskItem task)
        {
            taskItems.Add(task);
        }
        public TaskItem GetById(int id)
        {
            /// TaskItem res = taskItems.Where(x => x.Id == id);
            TaskItem res;
            foreach (TaskItem item in taskItems)
            {
                if (item.Id == id)return item;
            }
            return null ;
        }
        public void Update(TaskItem task)
        {
            for (int i = 0; i < taskItems.Count; i++)
            {
                if (taskItems[i].Id == task.Id)
                {
                    taskItems[i] = task;
                }
            }
        }
        public IEnumerable<TaskItem>  GetAll()
        {
            return taskItems;
        }
        public void Delete(int id)
        {
            taskItems = taskItems.Where(item => item.Id != id).ToList();
        }
    }
}
