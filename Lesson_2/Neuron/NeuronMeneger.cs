using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson_2.Classes;

namespace Lesson_2.Neuron
{
    public class NeuronMeneger : ICloneable
    {
        public Layers[] Layers { get; }
        public Topologi Topologi { get; }
        double _errorStatistic { get; set; }
        public double ErrorStatistic { get { return _errorStatistic / _countEpoch; } }
        int _countEpoch { get; set; }
        public
        Random Rand = new Random();

        public NeuronMeneger(Topologi topologi)//Инициализация нейросети по топологии
        {
            _errorStatistic = 0;
            _countEpoch = 0;
            Rand = new Random();
            Topologi = topologi;
            Layers = new Layers[topologi.CountLayers()];//создаю нужное количество слоев 
            CreateInputLauer();//создаю входной слой
            CreateHidenLauers();//создаю скрытые слои
            CreateOutputLauer();//создаю выходной слой 
        }
        public NeuronMeneger(string s)
        {
            _errorStatistic = 0;
            _countEpoch = 0;
            Rand = new Random();
            Topologi = new Topologi(s.Split()[0]);
            Layers = new Layers[Topologi.CountLayers()];
            CreateInputLauer();//создаю входной слой
            CreateHidenLauers();//создаю скрытые слои
            CreateOutputLauer();//создаю выходной слой 
            FilingInLayers(s.Split()[1]);
        }
        //Загрузка нейросети
        void FilingInLayers(string s)
        {
            CreateInputLauer();
            for (int i = 1;i<Topologi.CountLayers();i++)
            {
                Layers[i].LoadNeuronsWeightToString(s.Split('*')[i]);
            }
        }
        public void DeterminalNeuronActivaitFunction(int n)// переопределение функции активации
        {
            foreach (var i in Layers) i.DeterminalTypeActivF(n);
           
        }

        
        /// <param name="input">Массив входных значений для обучения</param>
        /// <param name="output">Массив выходных, для сопоставления с результатом полученным после проходки</param>
        /// <param name="epoch">Количество циклов повторения</param>
        public void Learn(double[,] input, double[,] output, int epoch)
        {// функция обучения, проверяю топологию входных выходных данных 
            if (input.GetLength(1) != Topologi.InputCount || output.GetLength(1) != Topologi.OutputCount || input.GetLength(0) != output.GetLength(0))
            {
                Console.WriteLine ($"Ошибка количества входных значений и выходных значений \n" +
                $"inp={input.Length} inpTopol={Topologi.InputCount} out={output.Length} outTop={Topologi.OutputCount} \n" +
                $"{input.GetLength(0)}   {output.GetLength(0)}");
                return;
            }
            for (int i = 0; i < epoch; i++)
            {
                int r = Rand.Next(0, input.GetLength(0));
                BackPropagation(ConvertRowArray(input, r), ConvertRowArray(output, r));
            }

        }
        /// <param name="data">Массив входных значений</param>
        /// <param name="epoch">Количество циклов повторения</param>
        public void LearnDataBase(List<DataLearn> data, int epoch)
        {
            _countEpoch = 0;//обнуление статистики
            _errorStatistic = 0;//обнуление статистики
            if (Topologi.InputCount == data[0].input.Length && Topologi.OutputCount == data[0].output.Length)
            {//проверка соответсвия топологий
                for (int i = 0; i < epoch; i++)
                {
                    int r = Rand.Next(0, data.Count);//берем случайный компонент из массива значений 
                    BackPropagation(data[r].input, data[r].output);//цикл обучения
                }
            }
        }

        public void BackPropagation(double[] input, double[] output)
        {
            double[] outputDelta = new double[Topologi.OutputCount];//создаю массив для разници
            var outputResault = FeedForward(input);// между результатом и требумым 
            for (int i = 0; i < Topologi.OutputCount; i++)
            {//заполняю массив требуемыми значениями
                outputDelta[i] = (outputResault[i] - output[i]);
                _errorStatistic += (outputResault[i] - output[i]) * (outputResault[i] - output[i]);
            }//ввожу полученный массив в последний слой 
            Layers[Layers.Length - 1].LearnOutputLayer(outputDelta, Topologi.LearningRate);
            //начинаю  обучение с слоя ниже до предпследнего (на входных нейронах вес всегда 1 )
            for (int i = Layers.Length - 2; i > 0; i--)//перебираю слои  
            {
                for (int j = 0; j < Layers[i].NeuronCount; j++)
                {// в каждый нейрон отправляю значение взвешанной суммы слоя выше 
                    Layers[i].Neurons[j].LearnSigmoid(Layers[i + 1].WeightedAmount(j), Topologi.LearningRate);
                    //   Console.WriteLine($"{j}тый нейрон {i}того слоя из {Layers.Length} готов");
                }//найденной специально для этого нейрона  
                 //  Console.WriteLine($"{i }тый слой готов");
            }//отслеживание ошибки, обнуление при 1 000 000 циклов обучения 
            _countEpoch++;

        }
        // конверитруем в одномерный массив выбранный 
        private double[] ConvertRowArray(double[,] array, int n)
        {// беру указанную строку из массива 
            double[] resault = new double[array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++)
            {
                resault[i] = array[n, i];
            }// и возвращаю ее в виде массива 
            return resault;
        }
        /// <param name="inputSignal">Входной сигнал, который обработает нейросеть</param>
        public double[] FeedForward(double[] inputSignal)
        {//прогоняем вводимый массив через сеть
            SendSignalsToInputNeurons(inputSignal); //вводим данные в первый слой 
            DrawDataThroughInvisibleLayers(); //можно прогнать все результаты прошлого слоя в новый 
            return Layers[Layers.Length - 1].Output;
        }


        private void DrawDataThroughInvisibleLayers()
        {//в каждый следующий слой вводим массив значений прошлого слоя 
            for (int i = 1; i <= Topologi.HiddenLayerd.Length + 1; i++)
            {//При этом начинаем с 1 недидимого слоя до последнего глобального слоя 
                Layers[i].GetSignal(Layers[i - 1].Output);
            }//вводим результат прошлого слоя в текущий слой 
        }

        private void SendSignalsToInputNeurons(double[] inputSignal)
        {//вводим данные в первый слой 
            Layers[0].SetInputLayers(inputSignal);
        }

        private void CreateOutputLauer()
        {
            //Создаю массив выходных значений  
            OutputNeuron[] neuron = new OutputNeuron[Topologi.OutputCount];// 
            for (int i = 0; i < Topologi.OutputCount; i++)
            {
                var n = new OutputNeuron(Layers[Layers.Length - 2].NeuronCount);
                neuron[i] = n;
            }
            Layers layer = new Layers(neuron);
            Layers[Layers.Length - 1] = layer;
            //в нейроне надо создать количество связей равное количеству связей на последнем скрытом слое 
        }

        private void CreateHidenLauers()
        {
            for (int i = 1; i < Layers.Length - 1; i++)
            {//заполняю скрытые слои, они расположены с 1 до пред последнего слоя 
                try { Layers[i] = CreateHidenLauer(i); } catch { Console.WriteLine ($"Ошибка создания внутреннего слоя {i}"); }
            }
        }
        private Layers CreateHidenLauer(int n)
        {// n значение глобального слоя который создаем, но 1 глобальному слою сооветсвует 0 слой скрытого  
            Neuron[] resaylr = new Neuron[Topologi.HiddenLayerd[n - 1] + 1];//
            for (int i = 0; i < Topologi.HiddenLayerd[n - 1]; i++)
            {   //заполняю нейронами стандартного (скрытого) типа 
                var neuron = new Neuron(Layers[n - 1].NeuronCount);//количество связей равно количеству нейронов
                resaylr[i] = neuron;// на предыдущем слое 
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[resaylr.Length - 1] = neuronBias;
            //формирую слой 
            Layers layer = new Layers(resaylr);
            return layer;
        }

        private void CreateInputLauer()
        {//Создаю на один больше для порогового смещения
            Neuron[] resaylr = new Neuron[Topologi.InputCount + 1];
            for (int i = 0; i < Topologi.InputCount; i++)
            {   //заполняю нейронами входного типа 
                var neuron = new InputNeuron(1);
                resaylr[i] = neuron;
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[Topologi.InputCount] = neuronBias;
            Layers layer = new Layers(resaylr);
            Layers[0] = layer;
        }
        /// <param name="learningRite">Новое значение шага сходимости</param>
        public void NewLearningRite(double learningRite)
        {//переопределяю значение шага сходимости в топологии 
            Topologi.LearningRate = learningRite;
        }
        //Интерфейс копирования нейросети 
        public object Clone()
        {
            return MemberwiseClone();
        }
        public string Type//возвращает тип функции активации
        {
            get {return Layers[0].TypeActivF;}
        }
        public override string ToString()//возвращаем топологию и веса всех нейронов в виде текста
        {
            string res = Topologi.ToString() + "\n";
            foreach (var i in Layers)
            {
                res += i.ToString() + "*";
            }
            return res;
        }

    }
}
