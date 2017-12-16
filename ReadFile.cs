using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Metro
{
    class ReadFile
    {
        private Dictionary<int, string> data;


        public ReadFile()
        {
            data = new Dictionary<int, string>();
        }

        public Dictionary<int, string> getData(byte[] bytes)
        {
            int length =  bytes.Length;
            if (length == 0)
                return null;

            String line = "";
            Stream stream = new MemoryStream(bytes);
            StreamReader reader = new StreamReader(stream);

            while(true){
                line = reader.ReadLine();
                if (line == null)
                    break;
           
                    string[] tempStr = line.Split('-');

                    if (tempStr.Length < 2)
                    {
                        Console.WriteLine("tempStr < 2");
                    }
                    else
                    {
                        String value = tempStr[0];
                        int key;

                        if (tempStr.Length == 3)
                        {
                            value += "-";
                            value += tempStr[1];
                            key = Int32.Parse(tempStr[2]);
                        }
                        else
                            key = Int32.Parse(tempStr[1]);

                        data.Add(key, value);
                        line = "";
                    }
                

            }
            reader.Close();

            return data;
        }
    }


}
