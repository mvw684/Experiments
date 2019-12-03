using System;
using System.IO;
using Newtonsoft.Json;

namespace Configuration {
    public class ConfigurationManager {
        private string location;
        private JsonSerializer serializer;
        public ConfigurationManager(string applicationName) {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            location = Path.Combine(appData, applicationName);
            serializer = new JsonSerializer();
        }

        private string GetFullPath<T>() {
            var name = Path.Combine(location, typeof(T).Name + ".json");
            return name;
        }

        public void Save<T>(T instance) {
            var name = GetFullPath<T>();
            if (File.Exists(name)) {
                string backup = name + ".bak";
                if (File.Exists(backup)) {
                    File.Delete(backup);
                }
                File.Move(name, backup);
            } else {
                var folder = Path.GetDirectoryName(name);
                Directory.CreateDirectory(folder);
            }
            using (var writer = new StreamWriter(name)) {
                using(var jsonWriter = new JsonTextWriter(writer)) {
                    serializer.Serialize(jsonWriter, instance, typeof(T));
                }
            }
        }

        public T Read<T>() where T: new() {
            var name = GetFullPath<T>();
            if (File.Exists(name)) {
                using var reader = new StreamReader(name);
                using var jsonReader = new JsonTextReader(reader);
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<T>(jsonReader);
                return result ?? new T();
            } else {
                return new T();
            }
        }
    }
}
