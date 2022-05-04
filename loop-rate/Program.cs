using System.Diagnostics;

class Program {
    //Hardcoded to "core"-domain.
    //"Package"-domain trivial since 1 ms update rate.
    private const string FILE_PATH = "/sys/class/powercap/intel-rapl/intel-rapl:0/intel-rapl:0:0/energy_uj";

    private static string read_rapl_value() {
        return System.IO.File.ReadAllText(FILE_PATH);
    }

    private static void log_while_true_rapl(int measurement_time_secs) {

        List<(string, long)> measurements = new List<(string, long)>(1_000_000_000);

        var watch = Stopwatch.StartNew();

        while (watch.ElapsedMilliseconds < measurement_time_secs * 1000) {
            var rapl_value = read_rapl_value();
            measurements.Add((rapl_value, watch.ElapsedTicks));
        }
        foreach (var tuple in measurements) {
            System.Console.WriteLine(tuple.Item1.Replace("\n", String.Empty) + ";" + tuple.Item2);
        }

    }

    static void PrintHelp() {
        System.Console.WriteLine("Usage: loop-rate <time_secs>");
    }
    static int Main(string[] args) {
        if (args.Length < 1) {
            PrintHelp();
            return 1;
        }
        int num = 0;
        var is_arg1_int = int.TryParse(args[0], out num);

        if (!is_arg1_int) {
            PrintHelp();
            return 1;
        }
        log_while_true_rapl(num);
        return 0;
    }

}