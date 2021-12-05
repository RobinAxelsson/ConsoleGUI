using System;
using System.Collections.Generic;
using System.IO;

namespace Flags
{
    class Error
    {
        public static void ToLog(Exception ex)
        {
            var lines = new List<string>
            {
                DateTime.Now.ToString(),
                $"Message: {ex.Message}",
                ex.StackTrace
            };

            File.AppendAllLines(TextfilesIO.TextfilesPath + "ErrorLog.csv", lines);
        }
    }
}
