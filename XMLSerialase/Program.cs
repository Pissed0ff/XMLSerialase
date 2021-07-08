using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Text.Encodings;
using System.Text.Json;
using System.Threading.Tasks;

namespace XMLSerialase
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car("Car1", "Very Confidential");
            var rad = new Radio[] {
                new Radio() { Name = "Radio C", Wave = 100.5 },
                new Radio() { Name = "Ваня", Wave = 90 } };
            car.ListRadio.AddRange(rad);
            car.Save();
            var carBin = Car.Load();
            car.SaveXml();
            var carXml = Car.LoadXml();
            car.SaveSoap();
            var carSoap = Car.LoadSoap();
            var json1 = new Car("JCar", "Opana");
            json1.ListRadio.AddRange(rad);
            json1.SaveJson();
            var jResult = Car.LoadJson();
        }
    }
    [Serializable]
    public class Car : Base
    {
        #region WORKS
        public Car(string name, string mes)
        {
            Private = mes;
            Name = name;
        }
        public Car() { }
        public string Name { get; set; }
        public List<Radio> ListRadio { get; } = new List<Radio>();

        private string Private;
        public void Save()
        {
            var el = this;
            Save<Car>(this);
        }
        public static Car Load()
        {
            return Load<Car>();
        }

        public void SaveXml()
        {
            SaveXml<Car>(this);
        }

        public static Car LoadXml()
        {
            return LoadXml<Car>();
        }
        public void SaveSoap()
        {
            SaveSoap<Car>(this);
        }

        public static Car LoadSoap()
        {
            return LoadSoap<Car>();
        }

        #endregion
        public void SaveJson()
        {
            SaveJson<Car>(this);
        }
        public static Car LoadJson()
        {
            return LoadJson<Car>();
        }
    }

    [Serializable]
    public class Radio
    {
        public string Name { get; set; }
        public double Wave { get; set; }
    }

    [Serializable]
    abstract public class Base
    {
        #region WORKS
        public void Save<T>(T obj)
        {
            var Formater = new BinaryFormatter();
            using (var fs = new FileStream("data.dat", FileMode.Create))
            {
                Formater.Serialize(fs, obj);
                Console.WriteLine("Сохранено");
            }
        }

        public static T Load<T>()
        {
            BinaryFormatter Form = new BinaryFormatter();
            using (FileStream fs = new FileStream("data.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && Form.Deserialize(fs) is T obj)
                    return obj;

                else return default;
            };
        }

        public void SaveXml<T>(T obj)
        {
            var formatter = new XmlSerializer(typeof(T));
            using (var fs = new FileStream("data.xml", FileMode.Create))
            {
                formatter.Serialize(fs, obj);
                Console.WriteLine("Saved successfully");
            }
        }
        public static T LoadXml<T>()
        {
            var formatter = new XmlSerializer(typeof(T));
            using (var fs = new FileStream("data.xml", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is T obj)
                    return obj;
                else
                    return default;
            }
        }

        public void SaveSoap<T>(T obj)
        {
            var Formatter = new SoapFormatter();
            using (var fs = new FileStream("data.soap", FileMode.Create))
            {
                Formatter.Serialize(fs, obj);
                Console.WriteLine("Spap saved");
            }
        }

        public static T LoadSoap<T>()
        {
            var Formatter = new SoapFormatter();
            using (var fs = new FileStream("data.soap", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && Formatter.Deserialize(fs) is T obj)
                    return obj;
                else return default;
            }
        }
        #endregion
        public void SaveJson<T>(T obj)
        {
            using (var fs = new FileStream("data.json", FileMode.Create))
            {
                JsonSerializer.SerializeAsync<T>(fs, obj);
            }
        }

        public static T LoadJson<T>()
        {
            string path = "data.json";
            if (File.Exists(path))
            {
                var str = File.ReadAllText(path);
                T obj = JsonSerializer.Deserialize<T>(str);
                return obj;
            }
            else 
            return default;

        }
    }
}
