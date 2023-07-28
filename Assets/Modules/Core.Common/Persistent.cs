using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Utils.Modules.Utils
{
    public static class Persistent
    {
        public static readonly string PATH = Application.persistentDataPath;

        public static bool Exists(string path)
        {
            return File.Exists(Path.Combine(PATH, path));
        }

        public static bool Save(string path, string data)
        {
            try
            {
                path = Path.Combine(PATH, path);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                return SaveThroughTemp(path, data);
            }
            catch (IOException exception)
            {
                LogException(exception);

                return false;
            }
        }

        public static bool Save(string path, byte[] data)
        {
            try
            {
                path = Path.Combine(PATH, path);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                return SaveThroughTemp(path, data);
            }
            catch (Exception exception)
            {
                LogException(exception);

                return false;
            }
        }

        private static bool SaveThroughTemp(string path, string data)
        {
            string temp = $"{path}.tmp";

            File.WriteAllText(temp, data, Encoding.UTF8);

            return Move(temp, path, true);
        }

        private static bool SaveThroughTemp(string path, byte[] data)
        {
            string temp = $"{path}.tmp";

            File.WriteAllBytes(temp, data);

            return Move(temp, path, true);
        }

        public static string Load(string path)
        {
            string result = null;

            try
            {
                path = Path.Combine(PATH, path);

                result = File.Exists(path) ? File.ReadAllText(path, Encoding.UTF8) : null;
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return result;
        }

        public static byte[] LoadBytes(string path)
        {
            byte[] result = null;
            try
            {
                path = Path.Combine(PATH, path);

                if (File.Exists(path))
                {
                    result = File.ReadAllBytes(path);
                }
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return result;
        }

        public static bool Move(string source, string target, bool @override = false)
        {
            try
            {
                source = Path.Combine(PATH, source);
                target = Path.Combine(PATH, target);

                if (!File.Exists(source))
                {
                    return false;
                }

                if (File.Exists(target))
                {
                    if (@override)
                    {
                        File.Delete(target);
                    }
                    else
                    {
                        return false;
                    }
                }

                File.Move(source, target);

                return true;
            }
            catch (Exception exception)
            {
                LogException(exception);

                return false;
            }
        }

        public static bool Delete(string path, bool directory = false)
        {
            try
            {
                path = Path.Combine(PATH, path);

                if (directory)
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
                else
                {
                    File.Delete(path);
                }

                return true;
            }
            catch (Exception exception)
            {
                LogException(exception);

                return false;
            }
        }

        public static void DeleteDirectoryContent(string path, Func<string, bool> predicate = null)
        {
            path = Path.Combine(PATH, path);

            if (!Directory.Exists(path))
            {
                return;
            }

            foreach (string directory in Directory.EnumerateDirectories(path))
            {
                if (predicate != null && predicate.Invoke(directory) == false)
                {
                    continue;
                }

                try
                {
                    Directory.Delete(directory, true);
                }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }

            foreach (string file in Directory.EnumerateFiles(path))
            {
                if (predicate != null && predicate.Invoke(file) == false)
                {
                    continue;
                }

                try
                {
                    File.Delete(file);
                }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }
        }

        private static readonly Dictionary<Type, int> EXCEPTIONS = new();

        private static void LogException(Exception exception)
        {
            Type type = exception.GetType();

            lock (EXCEPTIONS)
            {
                if (EXCEPTIONS.ContainsKey(type))
                {
                    EXCEPTIONS[type]++;

                    return;
                }

                EXCEPTIONS[type] = 1;
            }

            Debug.LogException(new Exception($"[Persistent] Exception ({type}) - {exception.Message}"));
        }
    }
}