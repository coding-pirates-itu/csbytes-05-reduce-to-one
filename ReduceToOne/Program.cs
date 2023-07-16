while (true)
{
    Console.Write("Enter the number > ");
    var str = Console.ReadLine();
    if (string.IsNullOrEmpty(str)) return;

    var digits = str.ToCharArray().Select(c => c - '0').ToArray();
    var ops = new[] { '+', '-', '*', '/', '^' };

    var solution = FindSolution(digits, ops, 1);

    if (string.IsNullOrEmpty(solution))
    {
        Console.WriteLine("Solution is not found");
    }
    else
    {
        Console.WriteLine($"Solution found: {solution}");
    }
}


string FindSolution(int[] digits, char[] ops, int target)
{
    var opPad = Enumerable.Range(0, digits.Length - 1).Select(_ => ops.First()).ToArray();
    var maxCount = (int)Math.Pow(ops.Length, digits.Length);

    for (var opNumber = 0; opNumber < maxCount; opNumber++)
    {
        if (Calculate(digits, opPad) == target)
        {
            var tail = opPad.Zip(digits.Skip(1), (o, d) => $"{o}{d}");
            return $"{digits.First()}{string.Join("", tail)}";
        }

        var n = 0;
        while (n < opPad.Length && Increment(opPad, n, ops))
            n++;
    }

    return "";
}


// Return true if carry
bool Increment(char[] array, int index, char[] source)
{
    var next = source.TakeWhile(c => c != array[index]).Count() + 1;

    if (next < source.Length)
    {
        array[index] = source[next];
        return false;
    }

    array[index] = source.First();
    return true;
}


int Calculate(int[] digits, char[] ops)
{
    var res = digits.First();

    for (var i = 0; i < ops.Length; i++)
    {
        var next = digits[i + 1];

        switch (ops[i])
        {
            case '+': res += next; break;
            case '-': res -= next; break;
            case '*': res *= next; break;
            case '/':
                if (next == 0) return int.MaxValue;
                if (res % next != 0) return int.MaxValue;
                res /= next;
                break;
            case '^':
                if (res == 0 && next == 0) return int.MaxValue;
                res = (int)Math.Pow(res, next);
                break;
            default: throw new NotImplementedException($"Do not know operation {ops[i]}");
        }
    }

    return res;
}
