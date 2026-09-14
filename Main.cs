//#Surok
//КИ25-21Б Сурков Никита 2 вариант
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OOPIntro
{
    public class FileInfo
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int Size { get; set; }

        public FileInfo(string name, DateTime creationDate, int size)
        {
            Name = name;
            CreationDate = creationDate;
            Size = size;
        }

        public override string ToString()
        {
            return $"Тип объекта: Файл{Environment.NewLine}" +
                   $"  Название файла: {Name}{Environment.NewLine}" +
                   $"  Дата создания:  {CreationDate:yyyy.MM.dd}{Environment.NewLine}" +
                   $"  Размер:         {Size} байт";
        }
    }

    public class FileParser
    {
        public static FileInfo Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Входная строка пуста.");

            int firstSpace = input.IndexOf(' ');
            if (firstSpace < 0)
                throw new FormatException("Не удалось определить тип объекта.");

            string type = input.Substring(0, firstSpace).Trim();
            if (!type.Equals("Файл", StringComparison.OrdinalIgnoreCase))
                throw new FormatException($"Неизвестный тип объекта: {type}");

            string rest = input.Substring(firstSpace).Trim();

            string name = ExtractQuotedString(ref rest);

            string[] parts = SplitByWhitespace(rest);

            if (parts.Length < 2)
                throw new FormatException("Недостаточно свойств: ожидались дата и размер.");

            DateTime creationDate = DateTime.ParseExact(
                parts[0],
                "yyyy.MM.dd",
                CultureInfo.InvariantCulture);

            int size = int.Parse(parts[1], CultureInfo.InvariantCulture);

            return new FileInfo(name, creationDate, size);
        }

        private static string ExtractQuotedString(ref string source)
        {
            int openQuote = source.IndexOf('"');
            if (openQuote < 0)
                throw new FormatException("Не найдена открывающая кавычка строкового свойства.");

            int closeQuote = source.IndexOf('"', openQuote + 1);
            if (closeQuote < 0)
                throw new FormatException("Не найдена закрывающая кавычка строкового свойства.");

            string result = source.Substring(openQuote + 1, closeQuote - openQuote - 1);

            source = source.Substring(closeQuote + 1).Trim();

            return result;
        }

        private static string[] SplitByWhitespace(string source)
        {
            return source.Split(
                new[] { ' ', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== Программа создания объектов \"Файл\" ===");
            Console.WriteLine();

            List<FileInfo> files = new List<FileInfo>();

            bool continueInput = true;

            while (continueInput)
            {
                Console.WriteLine("Введите описание файла в формате:");
                Console.WriteLine("  Файл \"название файла\" гггг.мм.дд размер");
                Console.WriteLine("Пример: Файл \"отчет.docx\" 2024.03.15 2048");
                Console.WriteLine();

                Console.Write("> ");
                string input = Console.ReadLine();

                try
                {
                    FileInfo file = FileParser.Parse(input);
                    files.Add(file);

                    Console.WriteLine();
                    Console.WriteLine("Создан объект:");
                    Console.WriteLine(file);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки: {ex.Message}");
                    Console.WriteLine();
                }

                continueInput = AskContinue();
                Console.WriteLine();
            }

            Console.WriteLine("=== Итоговый список объектов ===");
            if (files.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                for (int i = 0; i < files.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}]");
                    Console.WriteLine(files[i]);
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static bool AskContinue()
        {
            while (true)
            {
                Console.Write("Добавить ещё объект? (д/н): ");
                string answer = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(answer))
                    continue;

                answer = answer.Trim().ToLower();

                if (answer == "д" || answer == "да" || answer == "y" || answer == "yes")
                    return true;

                if (answer == "н" || answer == "нет" || answer == "n" || answer == "no")
                    return false;

                Console.WriteLine("Пожалуйста, введите \"д\" или \"н\".");
            }
        }
    }
}