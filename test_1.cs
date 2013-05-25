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

            //заполняем массив случайными числами
            System.Random random = new System.Random();
            for (int i = 0; i < razmer; i++)
            {
                massiv[i] = massiv[i == 0 ? 0 : i - 1] + random.Next(10); //шаг небольшой, но для примера хватит
            }

            //тестовый массив. ответ - 26
            //int[] massiv = new[] { 0, 1, 2, 4, 5, 6, 7, 27, 33 };
            //выводим результат
            System.Console.WriteLine(Razchet(massiv) + "\r\nНажмите любую клавишу для закрытия");
            System.Console.ReadKey(true);
        }

        //расчет и поиск минимиального значения
        public static string Razchet(int[] vhod)
        {
            bool[] can = new bool[(vhod.Length<<vhod.Length)+1];

            //используем битовый сдвиг
            for (int i = 0; i < ((1 << vhod.Length) - 1); i++)
            {
                int sum = 0;
                for (int j = 0; j < vhod.Length; j++)
                {
                    sum += ProvBit(i, j)*vhod[j];
                }
                if (sum > 0)
                    {
                        can[sum] = true;
                    }
                
            }
            //ищем минимальное значение
            for (int i = 1; i < (vhod.Length<<vhod.Length); i++)
            {
                if (!can[i])
                {
                    return i.ToString();
                }
            }
            return "Нет такого числа\r\n";
        }

        //проверка бита
        private static int ProvBit(int n, int i)
        {
            return (n & (1 << i)) != 0 ? 1 : 0;
        }
    }
}