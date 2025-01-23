using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Neuron
{
    public class InputNeuron : Neuron
    {
        public InputNeuron(int countInput) : base(countInput) { }
        protected override void RandomWeights(int countInput)
        {// входной нейрон нейрон имеет 1 синапс с весом в 1 
            for (int i = 0; i < countInput; i++)
            {
                Weights[i] = 1;
            }
        }
        /// <param name="inputs">Значение принимаемое входным нейроном</param>

        public override double FeedForwardInput(double inputs)
        {//выводит просто одно значение, поэтому функция активации ненужна 
            Output = inputs;
            return Output;
        }
    }
}
