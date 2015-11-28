using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    static class ServiceLocator //zamienia klasy statyczne na dynamiczne i na odwrot //zeby mozna byo uzywac wszsystkich klas wszedzie
    {
        static Dictionary<Type, Object> services; //słownik lista zwraca metode
        static ServiceLocator()
        {
            services = new Dictionary<Type, object>();
        }

        public static T GetService<T>() where T : new()
        {
            if (!services.ContainsKey(typeof(T)))
            {
                services.Add(typeof(T), new T());
            }
            return (T)services[typeof(T)];
        }
    }

