using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Classes
{
    public class Student
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public double Grade { get; }
        public List<string> Subjects = new List<string>();
        public Student(int id, string name, int age, double grade, List<string> subjects)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
            Subjects = subjects;
        }
        public override string ToString()
        {
            return $"id: {Id}, Name: {Name} Age: {Age} Grade: {Grade}  \nSubject: {string.Join(", ", Subjects)}";
        }
    }
}
