namespace test_3
{
    class Program
    {
        static void Main()
        {
            int squreSize, tableSize;

            //получаем значение размера таблицы
            System.Console.WriteLine("Введите размер таблицы (не больше 100): ");
            if (!int.TryParse(System.Console.ReadLine(), out tableSize)) return;

            //проверяем что квадрат не больше таблицы
            if (tableSize > 100)
            {
                System.Console.WriteLine("Размер таблицы не должен быть превышать 100!");

                System.Console.WriteLine("\r\nНажмите любую клавишу для завершения работы");
                System.Console.ReadKey(true);
                return;
            }

            //получаем значение размера квадрата
            System.Console.WriteLine("Введите размер квадрата (не больше {0}): ", tableSize);
            if (!int.TryParse(System.Console.ReadLine(), out squreSize)) return;

            //проверяем что квадрат не больше таблицы
            if(tableSize<squreSize)
            {
                System.Console.WriteLine("Размер квадрата не должен быть больше размера таблицы!");

                System.Console.WriteLine("\r\nНажмите любую клавишу для завершения работы");
                System.Console.ReadKey(true);
                return; 
            }

            int[,] massiv=new int[tableSize,tableSize];

            //Заполняем массив
            System.Random random = new System.Random();
            for (int x = 1; x < tableSize; x++)
            {
                for (int y = 1; y < tableSize; y++)
                {
                    massiv[x,y] = random.Next(10); 
                }
            }

            //выбор квадрата
            int nomer = 1;
            for (int x = 0; x < tableSize - squreSize+1; x++)
            {
                for (int y = 0; y < tableSize - squreSize+1; y++)
                {
                    //расчет суммы квадрата
                    int sum = 0;
                    for (int k = x; k < squreSize + x; k++)
                    {
                        for (int l = y; l < squreSize + y; l++)
                        {
                            sum += massiv[k, l];
                        }
                    }
                    System.Console.WriteLine("Сумма квадрата {0} равна {1}",nomer,sum);
                    nomer++;
                }
            }
            System.Console.WriteLine("\r\nНажмите любую клавишу для завершения работы");
            System.Console.ReadKey(true);
        }
    }
}
