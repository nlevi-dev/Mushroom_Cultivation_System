using System;
using System.IO;

namespace SEP4_Data.Data
{
    public class LogService : ILogService
    {
        public void Log(string value)
        {
            try
            {
                string temp = "[LOG " + DateTime.Now.ToString("dd/MM/yyyy HH.mm.ss") + "]\n" + value + "\n\n";
                using StreamWriter file = new StreamWriter("log.txt", true);
                file.WriteLine(temp);
                Console.WriteLine(temp);
            } catch (IOException) {/*ignored*/}
        }
    }
}