using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string IsComleted { get; set; }
    }
}
