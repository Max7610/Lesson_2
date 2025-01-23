using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Neuron
{
    public class Layers
    {
        public Neuron[] Neurons { get; }
        public int NeuronCount => Neurons?.Length ?? 0;
        public double[] Output { get; }
        /// <param name="neurons">Массив нейронов формирующих слой</param>
        public Layers(Neuron[] neurons)
        {//заполняем слой нейронами
            Neurons = new Neuron[neurons.Length];
            neurons.CopyTo(Neurons, 0);
            Output = new double[NeuronCount];
        }
        
        public void LoadNeuronsWeightToString(string s)
        {//проверяем количество нейронов в слое          
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].LoadWeightToString(s.Split('$')[i]);
            }          
        }
        /// <param name="input">Массив входых значений, получаемых с прошлого слоя</param>
        public double[] GetSignal(double[] input)
        {//принимаем массив значений 

            for (int i = 0; i < NeuronCount; i++)
            {   //прогоняем массив значений через каждый массив 
                Output[i] = Neurons[i].FeedForwardSigmoid(input);
            }
            return Output;
        }
        /// <param name="n">Номер нейрона, для которого будет считаться взвешенная сумма</param>
        public double WeightedAmount(int n)
        {//взвешанная сумма расчитается для слоя ниже, каждому нейрону индивидуально 
            double resault = 0.0;
            for (int i = 0; i < NeuronCount; i++)
            {
                resault += Neurons[i].MultiLocalGradientAndWeight(n);
            }
            return resault;
        }
        /// <param name="input">Массив значений для входного слоя</param>
        public void SetInputLayers(double[] input)
        {//вводим значения в первый слой поштучно в каждый нейрон каждое значение 
            for (int i = 0; i < input.Length; i++)
            {
                Output[i] = Neurons[i].FeedForwardInput(input[i]);
            }
            Output[Output.Length - 1] = Neurons[Neurons.Length - 1].Output;//последний нейрон это пороговое смещение
        }
        public void LearnOutputLayer(double[] outputDelta, double learningRate)
        {//Ввожу разнику между массив разниц полеченного и требуемого
            for (int i = 0; i < NeuronCount; i++)
            {
                Neurons[i].LearnSigmoid(outputDelta[i], learningRate);
            }
        }
        public void DeterminalTypeActivF(int n)//меняем функцию активации
        {
            foreach (var i in Neurons) i.DeterminalType(n);
           
        }
        public string TypeActivF
        {
            get { return Neurons[0].TypeActivF; }
        }
        
        public override string ToString()
        {//создаем из весов нейронов слой с раздилителем $
            string res = "";
            foreach (var i in Neurons)
            {
                res += i.ToString() + "$";
            }
            return res;
        }
    }
}
