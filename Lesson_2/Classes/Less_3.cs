using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lesson_2.Neuron;

namespace Lesson_2.Classes
{
    class Less_3
    {
        NeuronMeneger neuronMeneger;
        List<DataLearn> learnList;
        string menu = "1.Загрузить учебную базу данных \n2.Цикл обучения \n3.Ввести пробные значения \n4.Статус \n5.Выход";
        public Less_3( )
        {
            Topologi topologi = new Topologi(9,3,0.01,new int[] {15,7});
            neuronMeneger = new NeuronMeneger(topologi);
            MenuController();
        }
        async Task MenuController()
        {
            bool activ = true;
            while (activ)
            {
                Console.SetCursorPosition(0,0);
                Console.WriteLine(menu);
                string number = Console.ReadLine();
                if (int.TryParse(number,out int resault))
                {
                    if (resault > 0 && resault < 6)
                    {
                        if(resault == 5)
                        {
                            activ = false;
                        }
                        ComandControllerAsync(resault);
                    }
                    else 
                    {
                        Console.SetCursorPosition(0,10);
                        Console.WriteLine("Введите число от 1 до ");
                    }
                }
                else
                {
                    Console.SetCursorPosition(0, 10);
                    Console.WriteLine("Введенное значение не число");
                }
            }
        }
        async Task ComandControllerAsync(int number)
        {
            switch (number)
            {
                case 1:
                    var cts1 = new CancellationTokenSource();
                    await DowloandLearnBaseAsync();
                    await TimeConmtrol(cts1,8000);
                    break;
                case 2:
                    await LearnNeuron();
                    break;
                case 3:ReadUsersInput();
                    break;
                case 4:
                    string a = "Список пуст";
                    string b = "Длинна списка = ";
                    string mess = learnList == null ? a : b + learnList.Count.ToString();
                    Console.WriteLine(mess);
                    break;
            }

        }
        async Task DowloandLearnBaseAsync()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["LearnBd"].ConnectionString);//Соеденяемся 
                sqlConnection.Open();//Читаем
                var command = new SqlDataAdapter("SELECT inp1,inp2, inp3,inp4,inp5,inp6,inp7,inp8,inp9,out1,out2,out3 " +
                                         "FROM DataLearnNeuron ", sqlConnection);//Команда для запроса
                DataSet dataSet = new DataSet();//внутренняя база данных
                command.Fill(dataSet);//записываем из внешней во внутреннюю
                foreach (DataRow i in dataSet.Tables[0].Rows)
                {
                    learnList.Add(new DataLearn(i));
                    //Временная задержка для наглядности
                    Task.Delay(100);
                }
                sqlConnection.Close();
                
            }catch(Exception ex)
            {
                int top = Console.CursorTop;
                int left = Console.CursorLeft;
                Console.SetCursorPosition(0, 10);
                Console.WriteLine(ex.Message);
                Console.SetCursorPosition(left, top);
            }
        }
        async Task LearnNeuron()
        {
            if (learnList != null)
            {
                neuronMeneger.LearnDataBase(learnList,1000);
            }
            else
            {
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Список для обучения пуст");
            }
        }
        void ReadUsersInput()
        {
            Console.SetCursorPosition(0, 8);
            Console.Write("Введите ваши значения через пробел: ");
            string s =Console.ReadLine();
            if (s.Split(' ').Length==9)
            {
                List<double> inp = new List<double>();
                foreach(var i in s.Split(' '))
                {
                    if (double.TryParse(i,out double res))
                    {
                        inp.Add(res);
                    }
                    else
                    {
                        inp.Add(0.5);
                    }
                    
                    
                }
                var resault = neuronMeneger.FeedForward(inp.ToArray());
                string mes = "Результат ";
                foreach (var j in resault)
                {
                    mes += j.ToString() + " ";
                }
                Console.WriteLine(mes);
            }
            else
            {
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Ошибка чтения значений пользователя.");
            }
        }
        async Task TimeConmtrol(CancellationTokenSource cts,int time)
        {
            Task.Delay(time);
            cts.Cancel();
        }

    }
    
    public class DataLearn
    {
        public double[] input { get; }
        public double[] output { get; }
        public DataLearn(DataRow data)
        {
            input = new double[9];
            output = new double[3];
            for(int i = 0;i<9;i++)
            {
                try
                {
                    input[i] = (double)data[i];
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Ошибка записи {i} входного элемента в учебную запись\n{ex.Message} ");
                }
            }
            for(int i =9;i<12;i++)
            {
                try
                {
                    output[i-9] = (double)data[i];
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка записи {i} выходного элемента в учебную запись\n{ex.Message} ");
                }
            }
        }
    }

}
