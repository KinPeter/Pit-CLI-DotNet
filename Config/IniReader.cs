using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pit.Logs;

namespace Pit.Config
{
    public class IniReader
    {
        private readonly Logger log = new Logger("IniReader");

        public ConfigDictionary Read(string path)
        {
            try
            {
                Regex keyRegex = new Regex(@"^\[\w+\]$");
                var dict = new ConfigDictionary();
                var lines = File.ReadAllLines(path);
                if (lines.Length == 0) throw new Exception("Config file is empty.");

                string prevKey = null;
                foreach (string line in lines)
                {
                    if (keyRegex.IsMatch(line))
                    {
                        string key = line.Trim(new[] {'[', ']'});
                        prevKey = key;
                        dict[key] = new Dictionary<string, string>();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(prevKey))
                            throw new Exception($"Invalid file syntax: no key for value: {line}");

                        var keyValue = line.Trim().Split('=');
                        if (keyValue.Length < 2 || string.IsNullOrWhiteSpace(keyValue[0]) ||
                            string.IsNullOrWhiteSpace(keyValue[1]))
                            throw new Exception($"Invalid key-value pair: {line}");

                        if (keyValue.Length > 2)
                        {
                            keyValue[1] = string.Join(
                                "=",
                                keyValue.Skip(1).Take(keyValue.Length - 1)
                            );
                        }

                        dict[prevKey].Add(keyValue[0], keyValue[1]);
                    }
                }

                if (dict.Keys.Count == 0) throw new Exception("No parsable keys found in file.");

                return dict;
            }
            catch (Exception e)
            {
                log.Error($"Could not read file {path}", e.Message);
                Environment.Exit(1);
            }

            return null;
        }
    }
}