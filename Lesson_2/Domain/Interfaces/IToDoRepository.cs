using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson_2.Domain.Entities;
using Lesson_2.Domain.Interfaces;

namespace Lesson_2.Domain.Interfaces
{
    interface IToDoRepository
    {
        void Add(TaskItem task);
        TaskItem GetById(int id);
        IEnumerable<TaskItem> GetAll();
        void Update(TaskItem task);
        void Delete(int id);
    }
}
