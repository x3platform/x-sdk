using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace X3Platform.BindingPaths
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StringBuilder outString = new StringBuilder();

                string keyPath = @"SYSTEM\ControlSet001\Control\Session Manager\Environment";
                string keyName = "Path";

                Microsoft.Win32.RegistryKey obj = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey objItem = obj.OpenSubKey(keyPath);

                string value = objItem.GetValue(keyName).ToString();

                var drive = Path.GetPathRoot(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

                NameValueCollection nameValues = System.Configuration.ConfigurationManager.AppSettings;

                var path = value;

                outString.AppendLine("Date:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                outString.AppendLine("Original PATH:" + path);
                outString.AppendLine();

                for (var i = 0; i < nameValues.Count; i++)
                {
                    var temp = nameValues[i];

                    if (string.IsNullOrEmpty(temp)) continue;

                    temp = temp.Replace("${Drive}", drive);

                    if (Directory.Exists(temp) && path.IndexOf(temp) == -1)
                    {
                        path += ";" + temp;
                        outString.AppendLine(temp + " bingding.");
                    }
                    else if (path.IndexOf(temp) > -1)
                    {
                        outString.AppendLine(temp + " exists.");
                        // Console.WriteLine(temp + " exists.");
                    }
                    else
                    {
                        outString.AppendLine(temp + " not found.");
                        // Console.WriteLine(temp + " not found.");
                    }
                }

                outString.AppendLine("Current PATH:" + path);

                Console.WriteLine("Current PATH:{0}", path);

                if (path != value)
                {
                    // Save PATH Value
                    objItem = obj.OpenSubKey(keyPath, true);
                    objItem.SetValue(keyName, path);

                    outString.AppendLine("Save PATH success.");

                    Console.WriteLine("Save PATH success.");
                }

                File.WriteAllText("binding-paths.log", outString.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
}
