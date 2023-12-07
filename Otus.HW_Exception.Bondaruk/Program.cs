using System.Collections;

namespace Otus.HW_Exception.Bondaruk
{
    public enum Severity
    {
        Warning,  // 0
        Error,  // 1
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string?> data = new Dictionary<int, string?>();

            #region Collecting and parsing data

            var ParseOkFlag = false;
            while (ParseOkFlag!=true)
            {
                data = ComplexMenu.Start();
                Console.SetCursorPosition(0, 5);

                try
                {
                    foreach (KeyValuePair<int, string?> pair in data)
                    {
                        _ = ParseEquationParams(pair.Key, pair.Value);
                    }
                    ParseOkFlag = true;
                }
                catch (ParseToIntException ex)
                {
                    ParseOkFlag = false;
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

        public class ParseToIntException : Exception
        {
            public ParseToIntException(string message) : base(message)
            {
            }
        }
        public class RootAbscenceException : Exception
        {
            public RootAbscenceException(string message) : base(message)
            {
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
                    WriteDivLine(50);
                    Console.WriteLine(message);
                    WriteDivLine(50);
                    break;
                case Severity.Error:    //1
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    WriteDivLine(50);
                    Console.WriteLine(message);
                    WriteDivLine(50);
                    Console.WriteLine();
                    foreach (KeyValuePair<int, string?> pair in data)
                    {
                        Console.WriteLine($"{(char)pair.Key} = {pair.Value}");
                    }
                        break;
            }
            Console.ResetColor();
        }

        static void WriteDivLine(int length)
        {
            for (int i = 0; i<length; i++)
                Console.Write('-');
            Console.Write("\n");
        }

        //static (double?, double?) SquareEquationRootsFinding (Dictionary<int, string?> data)
        static void SquareEquationRootsFinding(Dictionary<int, string?> data)

        {
            //(double?, double?) result = (null, null);
            int a = int.Parse(data.GetValueOrDefault((int)'a'));
            int b = int.Parse(data.GetValueOrDefault((int)'b'));
            int c = int.Parse(data.GetValueOrDefault((int)'c'));
            Double d = Math.Pow(b,2)- 4 * a * c;
            try
            {
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
            catch (RootAbscenceException raex)
            {
                throw raex;
            }
        }
    }
    class ComplexMenu
    {
        /// <summary>
        /// Параметры уравнения
        /// </summary>
        private static string[] EqParams = new[]
        {
            "a",
            "b",
            "c",
        };

        /// <summary>
        /// Исходное положение стрелки меню
        /// </summary>
        private static int selectedValue = 0;
        private static void WriteCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(">");
            Console.SetCursorPosition(6, pos);
        }
        private static void ClearCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(" ");
            Console.SetCursorPosition(6, pos);
        }

        /// <summary>
        /// Вывести меню
        /// </summary>
        private static void PrintEquationParams(Dictionary<int, string?> data)
        {
            Console.WriteLine("Введите параметры уравнения. Для подтверждения ввода нажмите Enter");
            Console.WriteLine(
                $"{(data.TryGetValue((int)'a', out var dataValA) ? (dataValA + "*x^2") : "a*x^2")}" +
                $"{(data.TryGetValue((int)'b', out var dataValB) ? (dataValB.StartsWith('-') ? (dataValB + "*x") : ('+' + dataValB + "*x")) : "+b*x")}" +
                $"{(data.TryGetValue((int)'c', out var dataValC) ? (dataValC.StartsWith('-') ? (dataValC) : ('+' + dataValC)) : "+c")} = 0");


            for (var i = 0; i < EqParams.Length; i++)
            {
                if (data.GetValueOrDefault((int)'a' + i) != null)
                {
                    Console.WriteLine($" {i + 1}. {EqParams[i]} " + data.GetValueOrDefault((int)'a' + i));
                }
                else
                {
                    Console.WriteLine($" {i + 1}. {EqParams[i]} ");
                }
            }
        }

        /// <summary>
        /// На одну строку вниз
        /// </summary>
        private static void SetDown()
        {
            if (selectedValue < EqParams.Length + 1)
            {
                selectedValue++;
            }
            else
            {
                selectedValue = 2;
            }
        }

        /// <summary>
        /// На одну строку вверх
        /// </summary>
        private static void SetUp()
        {
            if (selectedValue > 2)
            {
                selectedValue--;
            }
            else
            {
                selectedValue = 4;
            }
        }

        /// <summary>
        /// Запуск меню
        /// </summary>
        public static Dictionary<int, string?> Start()
        {
            Dictionary<int, string?> data = new Dictionary<int, string?>();
            ConsoleKeyInfo ki;
            selectedValue = 2;
            PrintEquationParams(data);

            WriteCursor(selectedValue);
            do
            {
                ki = Console.ReadKey();
                ClearCursor(selectedValue);
                switch (ki.Key)
                {
                    case ConsoleKey.UpArrow:
                        SetUp();
                        break;
                    case ConsoleKey.DownArrow:
                        SetDown();
                        break;
                    case ConsoleKey.Enter:
                        SetDown();
                        break;
                    default:
                        Console.Write(ki.KeyChar.ToString());
                        Console.SetCursorPosition(7, selectedValue);
                        data.Remove((int)'a' + selectedValue - 2);
                        data.Add((int)'a' + Console.CursorTop - 2, (ki.KeyChar.ToString() + Console.ReadLine()));
                        Console.Clear();
                        PrintEquationParams(data);
                        SetDown();
                        break;
                }
                WriteCursor(selectedValue);
            }
            while (data.Count() < 3);
            return data;
        }
    }

}
