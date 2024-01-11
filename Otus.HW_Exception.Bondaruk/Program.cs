using System.Collections;

namespace Otus.HW_Exception.Bondaruk
{
    internal class Program
    {
       static readonly string _dividingLine = new string('-', 50);

        static void Main(string[] args)
        {
            Dictionary<int, string?> data = new Dictionary<int, string?>();
            #region Collecting and parsing data

            var parseOkFlag = false;
            while (!parseOkFlag)
            {
                data = ComplexMenu.Start();
                Console.SetCursorPosition(0, 5);

                try
                {
                    foreach (KeyValuePair<int, string?> pair in data)
                    {
                        _ = ParseEquationParams(pair.Key, pair.Value);
                    }
                    parseOkFlag = true;
                }
                catch (ParseToIntException ex)
                {
                    parseOkFlag = false;
                    FormatData(ex.Message, Severity.Error, data);
                    Console.WriteLine("Для повтора ввода нажмите любую клавишу");
                    Console.ReadKey();
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                }
            }
            #endregion
            
            try
            {
                SquareEquationRootsFinding(data);
            }
            catch (RootAbscenceException ndex)
            {
                FormatData(ndex.Message, Severity.Warning, data);
            }
        }

        private static int? ParseEquationParams (int dataKey, string dataValue)
        {
                try
                {
                    if (!int.TryParse(dataValue, out int retVal))
                        throw new ParseToIntException($"Неверный формат параметра {(char)dataKey}");
                    return retVal;
                }
                catch (ParseToIntException ptiex)
                {
                    Console.WriteLine($"Исключение при парсинге параметра {(char)dataKey} = {dataValue}");
                    throw ptiex;
                }
        }
            
        static void FormatData(string message, Severity severityLevel, Dictionary<int, string?> data)
        {
            switch (severityLevel)
            {
                case Severity.Warning:  //0
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(_dividingLine);
                    Console.WriteLine(message);
                    Console.WriteLine(_dividingLine);
                    break;
                case Severity.Error:    //1
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(_dividingLine);
                    Console.WriteLine(message);
                    Console.WriteLine(_dividingLine);
                    Console.WriteLine();
                    foreach (KeyValuePair<int, string?> pair in data)
                    {
                        Console.WriteLine($"{(char)pair.Key} = {pair.Value}");
                    }
                        break;
            }
            Console.ResetColor();
        }
        static void SquareEquationRootsFinding(Dictionary<int, string?> data)

        {
            int a = int.Parse(data.GetValueOrDefault((int)'a'));
            int b = int.Parse(data.GetValueOrDefault((int)'b'));
            int c = int.Parse(data.GetValueOrDefault((int)'c'));
            Double d = Math.Pow(b,2)- 4 * a * c;

           switch (d)
           {
               case < 0:
                   if (d < 0)
                       throw new RootAbscenceException($"Вещественных значений не найдено");
                   break;
               case <1e-10:    //one root
                   double root = (-b + Math.Sqrt(d)) / (2 * a);
                   Console.WriteLine($"x = {root}");
                   //return (root, null);
                   break;
               case > 1e-10:
                   double root1 = (-b + Math.Sqrt(d)) / (2 * a);
                   double root2 = (-b - Math.Sqrt(d)) / (2 * a);
                   Console.WriteLine($"x1 = {root1}, x2 = {root2}");
                   break;
           }
        }
    }
}
