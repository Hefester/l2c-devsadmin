using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace L2.Net
{
    /// <summary>
    /// A Basic properties, based on Java.
    /// </summary>
    public class Properties
    {
        private Dictionary<String, String> list;
        private String filename;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string this[string propName, string def]
        {
            get
            {
                string val = null;
                try
                {
                    val = list[propName];
                }
                catch
                {
                    Logger.WriteLine("[Config] Missing Property:"+propName);
                }
                return string.IsNullOrEmpty(val) ? def : val;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public string this[string propName]
        {
            get
            {
                return list[propName];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">Archivo</param>
        public Properties(String file)
        {
            reload(file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void set(String field, Object value)
        {
            if (!list.ContainsKey(field))
                list.Add(field, value.ToString());
            else
                list[field] = value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            Save(this.filename);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void Save(String filename)
        {
            this.filename = filename;

            if (!System.IO.File.Exists(filename))
                System.IO.File.Create(filename);

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);

            foreach (String prop in list.Keys.ToArray())
                if (!String.IsNullOrEmpty(list[prop]))
                    file.WriteLine(prop + "=" + list[prop]);

            file.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void reload()
        {
            reload(this.filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void reload(String filename)
        {
            this.filename = filename;
            list = new Dictionary<String, String>();

            if (System.IO.File.Exists(filename))
                loadFromFile(filename);
            else
                System.IO.File.Create(filename);
        }

        /// <summary>
        /// If config is loaded.
        /// </summary>
        public bool Loaded
        {
            get;
            protected set;
        }

        private void loadFromFile(String file)
        {
            if(System.IO.File.Exists(file))
            {
                Loaded = true;
            }
            else
            {
                Loaded = false;
            }
            foreach (String line in System.IO.File.ReadAllLines(file))
            {
                if ((!String.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    String key = line.Substring(0, index).Trim();
                    String value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore dublicates
                        list.Add(key, value);
                    }
                    catch { }
                }
            }
        }


    }
}
