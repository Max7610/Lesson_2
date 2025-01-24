using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Neuron
{
    public class OutputNeuron : Neuron
    {

        public OutputNeuron(int countInput) : base(countInput) { }
        /// <param name="weightedAmount">Ввести разнику между выходны и требуемым значением</param>
        /// <param name="learningRate">Шаг сходимости </param>
        public override void LearnSigmoid(double weightedAmount, double learningRate)
        {
            this.LocalGradient = weightedAmount * Output * (1 - Output);
            FixWeights(learningRate);
        }
    }
}
