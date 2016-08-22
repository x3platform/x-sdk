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
                string keyPath = @"SYSTEM\ControlSet001\Control\Session Manager\Environment";
                string keyName = "Path";

                Microsoft.Win32.RegistryKey obj = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey objItem = obj.OpenSubKey(keyPath);

                string value = objItem.GetValue(keyName).ToString();

                var drive = Path.GetPathRoot(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

                NameValueCollection nameValues = System.Configuration.ConfigurationManager.AppSettings;

                var path = value;

                for (var i = 0; i < nameValues.Count; i++)
                {
                    var temp = nameValues[i];

                    if (string.IsNullOrEmpty(temp)) continue;

                    temp = temp.Replace("${Drive}", drive);

                    if (Directory.Exists(temp) && path.IndexOf(temp) == -1)
                    {
                        path += ";" + temp;
                    }
                    else if (path.IndexOf(temp) > -1)
                    {
                        // Console.WriteLine(temp + " exists.");
                    }
                    else
                    {
                        Console.WriteLine(temp + " not found.");
                    }
                }

                Console.WriteLine("Path:{0}", path);

                if (path != value)
                {
                    // Save PATH Value
                    objItem = obj.OpenSubKey(keyPath, true);
                    objItem.SetValue(keyName, path);

                    Console.WriteLine("save success.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
}
