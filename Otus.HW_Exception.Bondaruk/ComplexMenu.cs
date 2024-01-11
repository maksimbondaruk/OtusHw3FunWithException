namespace Otus.HW_Exception.Bondaruk;

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