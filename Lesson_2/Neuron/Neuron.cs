using System;


namespace Lesson_2.Neuron
{
    public class Neuron
    {
        public double[] Weights { get; private set; }//вес аксона 
        public double[] Inputs { get; private set; }//массив входных значений
        public double Output { get; protected set; }// исходящий объем 
        public double LocalGradient { get; protected set; } // локальный градиент 

        public Type type;

        /// <param name="countInput">Количество входных значений</param>
        public Neuron(int countInput)//Количество связей с нижним слоем 
        {// объявляю и заполняю массивы значений 
            Weights = new double[countInput];
            Inputs = new double[countInput];
            RandomWeights(countInput);
            type = Type.Sigmoid;
        }

        public void LoadWeightToString(string s)
        {
            if (s.Split('#').Length >= Weights.Length)
            {//проверяем длинну строки с количеством аксонов
                for (int i = 0; i < Weights.Length; i++)
                {
                    Weights[i] = Convert.ToDouble(s.Split('#')[i]);
                }
            }

        }
        public void DeterminalType(int n)// переопределяем функцию активации
        {
            if (n == 0) type = Type.Sigmoid;
            if (n == 1) type = Type.GipTangens;
            if (n == 2) type = Type.Lin;
            if (n == 3) type = Type.ReLu;
            if (n == 4) type = Type.Sin;
            if (n == 5) type = Type.Arctg;
           
        }
        public string TypeActivF
        {
        get { return type.ToString(); }
        }
        protected virtual void RandomWeights(int countInput)
        {// случайные числа для начальных весов 
            var r = new Random();
            for (int i = 0; i < countInput; i++)
            {
                Weights[i] = r.NextDouble() / 2;
            }
        }
        /// <param name="inputs">Входные значения для данного нейрона, возращает сумму всех значений умноженную на веса, проведенную через функцию активации </param>
        virtual public double FeedForwardSigmoid(double[] inputs)
        {//получаю в нейрон массив значений 
            if (Inputs.Length == 1) { return 1; }
            inputs.CopyTo(Inputs, 0);
            Output = FunctionActiv(SumInputs());// повожу через фомулу активации
            return Output;    //вывожу результат 
        }
        protected double SumInputs()
        {// нахожу сумму всех входных значений и умножаю их на веса 
            var sum = 0.0;
            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i] * Weights[i];
            }
            return sum;
        }
        
        /// <param name="weightedAmount">Взвешанная сумма слоя </param>
        public virtual void LearnSigmoid(double weightedAmount, double learningRate)
        {// нахожу лакальный градиент как произведение взыешенной суммы слоя выше на производную
         // от функции активации
            LocalGradient = weightedAmount * FunctionDeActiv(Output);
            FixWeights(learningRate);
        }
        protected void FixWeights(double learningRate)
        {//перерасчитываю веса 
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = Weights[i] - learningRate * LocalGradient * Inputs[i];
            }
        }
        /// <param name="n">Номер аксона, который будет умножен на лакальный градиент</param>
        public virtual double MultiLocalGradientAndWeight(int n)
        {// возвращаем произведение градиента на вес указанного синапса
            return Weights[n] * LocalGradient;
        }
        public virtual double FeedForwardInput(double inputs)
        {
            return 0;
        }

        public override string ToString()
        {//создаем строку, в которой все компоненты записаны по проядку с разделителем 
            string res = "";
            foreach (var i in Weights)
            {
                res += i.ToString() + "#";
            }
            return res;
        }
        double FunctionActiv(double x)// выбор функции активации
        {
            if (type == Type.Sigmoid)return Sigmoid(x);
            if (type == Type.GipTangens) return GipTan(x) ;
            if (type == Type.Lin) return Lin(x) ;
            if (type == Type.ReLu) return ReLu(x) ;
            if (type == Type.Sin) return Sin(x);
            if (type == Type.Arctg) return Arctan(x) ;
            return 1; }
        double FunctionDeActiv( double x)// выбор функции деактивации
        {
            if (type == Type.Sigmoid) return SigmoidDx(x);
            if (type == Type.GipTangens) return DxGipTan(x);
            if (type == Type.Lin) return DxLin(x);
            if (type == Type.ReLu) return DxReLu(x);
            if (type == Type.Sin) return DxSin(x);
            if (type == Type.Arctg) return DxArctan(x);
            return 1;
        }
    
        double Sigmoid(double x)
        {//нахожу сигмойду (функция ативации)
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result;
        }
        double SigmoidDx(double x)
        {// производная сигмойды (функция деактивации) 
            var sigmoid = Sigmoid(x);
            var resault = sigmoid * (1 - sigmoid);
            return resault;
        }
        double GipTan(double x)
        {
            return ((double)2 / (1 + Math.Pow(Math.E, -2 * x))) - 1;
        }
        double DxGipTan(double x)
        {
            return 1 - GipTan(x) * GipTan(x);
        }
        double Lin(double x)
        {   
            return x;
        }
        double DxLin(double x)
        {
            return (double)x/100;
        }
        double ReLu(double x)
        {
            if (x > 1) return 1;
            if (x < 0) return 0;
        return x; }
        double DxReLu(double x)
        {
            if (x > 1) return 1;
            return 0.001d;
        }
        double Sin(double x)
        {
            if (x < 0) return 0;
            if (x > (double)Math.PI / 2) return 1;
            return Math.Sin(x);
        }
        double DxSin(double x)
        {
            if (x < 0) x = 0;
            if (x > (double)Math.PI / 2) x = (double)Math.PI / 2;
            return Math.Cos(x);

        }
        double Arctan(double x)
        {
            return Math.Atan(x);
        }
        double DxArctan(double x)
        {
            return 1 / (x * x + 1);
        }
    }
    

    public enum Type // список функций активации
    {
        Sigmoid,
        GipTangens,
        Lin,
        ReLu,
        Sin,
        Arctg
    }

}
