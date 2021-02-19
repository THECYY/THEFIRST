using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Allpurpose.Utils
{
    public class FileUtils
    {
        public static void copy(String sourcePath, String aimPath) {
            try
            {
                using (FileStream inputStream = new FileStream(sourcePath, FileMode.Open))
                {
                    using (FileStream outputStream = new FileStream(aimPath, FileMode.OpenOrCreate))
                    {
                        byte[] buffer = new byte[1024];
                        int len = 0;
                        while ((len = inputStream.Read(buffer, 0, 1024)) != 0)
                        {
                            outputStream.Write(buffer, 0, len);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

   
}
