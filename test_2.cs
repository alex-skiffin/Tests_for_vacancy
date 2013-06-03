/*
Составить программу, печатающую разложение на простые множители натурального числа 0<n<=10000. 
*/
namespace test_2
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
                if(vhod==1) vyvod += "2";
                else
                {
                    vyvod += "2*";
                }
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
                    vhod = vhod / mnozh;
                    c = (int)System.Math.Sqrt(vhod) + 1;
                    vyvod += mnozh.ToString() + "*";
                }
                //если множитель не подходит - переходим к следующему нечетному числу
                else
                {
                    mnozh += 2;
                }
            }
            if (vhod != 1)
            {
                vyvod += vhod.ToString();
            }
            System.Console.WriteLine(vyvod);
            System.Console.Write("Нажмите любую клавишу для закрытия.");
            System.Console.ReadKey(true);
        }
    }
}