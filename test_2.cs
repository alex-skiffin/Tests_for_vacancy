namespace ивс_тест_2
{
    class Program
    {
        static void Main()
        {
            //строка хранения результата
            string vyvod = string.Empty;

            int vhod;
            System.Console.WriteLine("Введите число: ");

            //получаем число
            if (!int.TryParse(System.Console.ReadLine(), out vhod)) return;

            //проверяем делится ли введенное число на 2
            while ((vhod % 2) == 0)
            {
                vhod = vhod / 2;
                vyvod += "2*";
            }

            //первый нечетный множитель
            int mnozh = 3;

            //максимально допустимый множитель
            int c = (int)System.Math.Sqrt(vhod) + 1;

            //перебираем возможные множители
            while (mnozh < c)
            {
                //проверяем множитель
                if ((vhod % mnozh) == 0)
                {
                    vyvod += mnozh.ToString() + "*";
                    vhod = vhod / mnozh;
                    c = (int)System.Math.Sqrt(vhod) + 1;
                }
                //если множитель не подходит - переходим к следующему нечетному числу
                else
                {
                    mnozh += 2;
                }
            }
            vyvod += vhod.ToString();
            System.Console.WriteLine(vyvod);
            System.Console.Write("Нажмите любую клавишу для закрытия.");
            System.Console.ReadKey(true);
        }
    }
}