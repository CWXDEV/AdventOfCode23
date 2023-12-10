namespace Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var total = 0;
            var lines = GetStrings();
            var whileBreak = true;
            var symbolsToCheckFor = "*-$=%@/#+";

            // go through each line
            for (int i = 0; i < lines.Length; i++)
            {
                // go through each char
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (int.TryParse($"{lines[i][j]}", out int result))
                    {
                        whileBreak = true;
                        var test = new List<EngingPart>
                        {
                            new EngingPart() { indexPosition = j, value = result }
                        };

                        var index = j;

                        // get the rest of the number
                        while(index < lines[i].Length - 1 && whileBreak)
                        {
                            index++;
                            try
                            {
                                if (int.TryParse($"{lines[i][index]}", out int innerResult))
                                {
                                    test.Add(new EngingPart() { indexPosition = index, value = innerResult });
                                }
                                else
                                {
                                    whileBreak = false;
                                }
                            } 
                            catch
                            {
                                Console.WriteLine("\n something fucked up");
                                throw;
                            }
                        }

                        var symbolExists = false;

                        // go through each index and check for a symmbol in 8 different positions
                        for (var k = 0; k < test.Count; k++)
                        {
                            var preString = "";
                            var sameString = "";
                            var nexString = "";

                            if (i >= 1)
                            {
                                preString = lines[i - 1];
                            }

                            sameString = lines[i];

                            if (i < lines.Length - 1)
                            {
                                nexString = lines[i + 1];
                            }

                            if (preString != "")
                            {
                                // check previous string and index-1
                                if (test[k].indexPosition >= 1 && IsSymbol(preString[test[k].indexPosition - 1]))
                                    symbolExists = true;

                                // check previous string and index
                                if (IsSymbol(preString[test[k].indexPosition]))
                                    symbolExists = true;

                                // check previous string and index+1
                                if (test[k].indexPosition < preString.Length - 1 && IsSymbol(preString[test[k].indexPosition + 1]))
                                    symbolExists = true;
                            }

                            if (sameString != "")
                            {
                                // check same string and index-1
                                if (test[k].indexPosition >= 1 && IsSymbol(sameString[test[k].indexPosition - 1]))
                                    symbolExists = true;

                                // check same string and index+1
                                if (test[k].indexPosition < sameString.Length - 1 && IsSymbol(sameString[test[k].indexPosition + 1]))
                                    symbolExists = true;
                            }

                            if (nexString != "")
                            {
                                // check next string and index-1
                                if (test[k].indexPosition >= 1 && IsSymbol(nexString[test[k].indexPosition - 1]))
                                    symbolExists = true;

                                // check next string and index
                                if (IsSymbol(nexString[test[k].indexPosition]))
                                    symbolExists = true;

                                // check next string and index+1
                                if (test[k].indexPosition < nexString.Length - 1 && IsSymbol(nexString[test[k].indexPosition + 1]))
                                    symbolExists = true;
                            }
                        }

                        if (symbolExists)
                        {
                            var stringToConvert = "";

                            foreach (var tes in test)
                            {
                                stringToConvert += tes.value;
                            }

                            if (int.TryParse($"{stringToConvert}", out int resultEnd))
                            {
                                total += resultEnd;
                                Console.WriteLine(resultEnd);
                            }
                        }

                        j = test.Last().indexPosition + 1;
                    }
                }
            }

            Console.WriteLine($"Total: {total}");
        }

        private static bool IsDigit(char test)
        {
            return
                test == '0' ||
                test == '1' ||
                test == '2' ||
                test == '3' ||
                test == '4' ||
                test == '5' ||
                test == '6' ||
                test == '7' ||
                test == '8' ||
                test == '9';
        }

        private static bool IsDot(char test) 
        {
            return test == '.';
        }

        private static bool IsSymbol(char test)
        {
            return !IsDigit(test) && !IsDot(test);
        }

        private static string[] GetStrings()
        {
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\input\givenInput.txt"));

            var text = File.ReadAllText(path);

            return text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries
            );
        }

        private static string[] testData()
        {
            return new string[]
            {
                "467....114",
                "...*......",
                "..35...633",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598.."
            };
        }
    }

    struct EngingPart
    {
        public int indexPosition;
        public int value;
    }
}

/*
 --- Day 3: Gear Ratios ---

You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.

It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.

"Aaah!"

You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone! 
The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.

The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. 
If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.

The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols you don't really understand, 
but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)

Here is an example engine schematic:

467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..

In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.

Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?

*/