using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Neuron
{
    public class Topologi
    {
        public int InputCount { get; }//количество входных нейронов 
        public int OutputCount { get; } //количество выходных нейронов(но у меня будет 1)
        public int[] HiddenLayerd { get; }// хранит количество нейронов в каждом скрытом слое 
        public double LearningRate;//шаг сходимости 
        /// <param name="inputCount">Количество входных нейронов</param>
        /// <param name="outputCount">Количество выходных нейронов</param>
        /// <param name="learningRate">Шаг сходимости</param>
        /// <param name="lauers">Архитектура внутренних слоев</param>
        public Topologi(int inputCount, int outputCount, double learningRate, int[] lauers)
        {//заполняем базовыми значениями
            LearningRate = learningRate;
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayerd = new int[lauers.Length];
            lauers.CopyTo(HiddenLayerd, 0);
        }
        public Topologi(string s)//Создание топологии по строке
        {
            InputCount= Convert.ToInt32(s.Split('_')[0]);
            OutputCount= Convert.ToInt32(s.Split('_')[s.Split('_').Length]);
            List<int> layers = new List<int>();
            for (int i = 1; i < s.Split('_').Length - 1; i++)
            {
                layers.Add(Convert.ToInt32(s.Split('_')[i]));
            }
            HiddenLayerd = layers.ToArray();
            LearningRate = 0.1;
        }
        public int CountLayers()
        {// количество слоев равно количеству невидимых плюс 1 входной и 1 выходной 
            return HiddenLayerd.Length + 2;
        }
        /// <param name="top">Топология, соответсвие с которой я проверяю</param>
        public bool CheckAccordance(Topologi top)
        {
            return (top.InputCount == InputCount) && (top.OutputCount == OutputCount);
        }
        public override string ToString()
        {
            string res = InputCount.ToString() + "_";
            foreach (var i in HiddenLayerd) { res += i.ToString() + "_"; }
            res += OutputCount.ToString();
            return res;
        }

    }
}
