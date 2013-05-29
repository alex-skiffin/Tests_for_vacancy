﻿/*
Дан неубывающий массив неотрицательных целых чисел a[1]<=a[2]<=…<=a[n], n<=10000. 
Найти наименьшее целое положительное число, не представимое в виде суммы нескольких 
элементов этого массива (каждый элемент массива может быть использован не более одного раза). 
*/

namespace test_1
{
    internal class Program
    {
        private static void Main()
        {
            //получаем размер массива
            System.Console.WriteLine("Укажите размер массива (цифрами):");
            int razmer;

            //проверяем вводимые данные
            if (!int.TryParse(System.Console.ReadLine(), out razmer))
            {
                System.Console.WriteLine("Это были не цифры...");
                System.Console.WriteLine("\r\nНажмите любую клавишу для закрытия");
                System.Console.ReadKey(true);
                return;
            }
            int[] massiv = new int[razmer];


            System.Console.WriteLine("Укажите способ заполнения. \r\n1-вручную      2-случайными числами");
            int vybor;
            //проверяем вводимые данные
            if (!int.TryParse(System.Console.ReadLine(), out vybor))
            {
                System.Console.WriteLine("Указано неправильное значение");
                System.Console.WriteLine("\r\nНажмите любую клавишу для закрытия");
                System.Console.ReadKey(true);
                return;
            }
            if (vybor == 1)
            {
                for (int i = 0; i < razmer; i++)
                {
                    massiv[i] = int.Parse(System.Console.ReadLine());
                }
            }
            else
            {
                //заполняем массив случайными числами
                System.Random random = new System.Random();
                for (int i = 0; i < razmer; i++)
                {
                    massiv[i] = massiv[i == 0 ? 0 : i - 1] + random.Next(5); //шаг небольшой, но для примера хватит
                }
                //тестовый массив[24]. ответ - 26
                //int[] massiv = new[] { 0,1,2,4,5,6,7,27,33,34,35,38,39,40,42,45,47,49,51,53,54,56,57,59 };

                //выводим результат случайного заполнения
                foreach (var i in massiv)
                {
                    System.Console.Write(i + "  ");
                }
            }
            System.Console.Write("\n");
            System.Console.WriteLine(Razchet(massiv) + "\r\nНажмите любую клавишу для закрытия");
            System.Console.ReadKey(true);
        }

        //расчет и поиск минимиального значения
        public static string Razchet(int[] vhod)
        {
            int k = 0;
            int minZnach = vhod[k];

            while ((k != vhod.Length-1)&&(vhod[k + 1] <= minZnach + 1))
            {
                minZnach = minZnach + vhod[k + 1];
                k++;
            }
            return (minZnach + 1).ToString();
        }
    }
}