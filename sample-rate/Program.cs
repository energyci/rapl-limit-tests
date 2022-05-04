using System.Diagnostics;

class Program {
    private const string FILE_PATH_CORE = "/sys/class/powercap/intel-rapl/intel-rapl:0/intel-rapl:0:0/energy_uj";
    private const string FILE_PATH_PACKAGE = "/sys/class/powercap/intel-rapl/intel-rapl:0/energy_uj";

    private static string read_rapl_value(string filepath) {
        return System.IO.File.ReadAllText(filepath);
    }

    private static void rapl_update_interval(int measurement_time_secs, bool core) {
        List<long> measurements = new List<long>(1_000_000_000);
        var outer_watch = Stopwatch.StartNew();
        string RAPL_PATH = core ? FILE_PATH_CORE : FILE_PATH_PACKAGE;

        while (outer_watch.ElapsedMilliseconds < measurement_time_secs * 1000) {
            bool changed = false;
            var inital_value = read_rapl_value(RAPL_PATH);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (!changed) {
                var new_value = read_rapl_value(RAPL_PATH);
                changed = new_value != inital_value;
            }
            watch.Stop();
            measurements.Add(watch.ElapsedTicks);
        }
        System.Console.WriteLine(String.Join("\n", measurements));
    }

    static void PrintHelp() {
        System.Console.WriteLine("Usage: sampling-rate (core|package) <time_secs>");
    }
    static int Main(string[] args) {
        if (args.Length < 2) {
            PrintHelp();
            return 1;
        }
        var action = args[0].ToLower();
        string[] valid_actions = { "core", "package" };
        int num = 0;
        var is_arg1_int = int.TryParse(args[1], out num);

        if (!is_arg1_int || !valid_actions.Contains(action)) {
            PrintHelp();
            return 1;
        }
        rapl_update_interval(num, action == "core");
        return 0;
    }

}