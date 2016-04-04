using System.Data;
using System.IO;
using System.Text;

namespace MyUtility
{
    public static class CsvUtility
    {
        public static MemoryStream GetCsv(DataTable data)
        {
            var fieldsToExpose = new string[data.Columns.Count];
            for (var i = 0; i < data.Columns.Count; i++)
            {
                fieldsToExpose[i] = data.Columns[i].ColumnName;
            }

            return GetCsv(fieldsToExpose, data);
        }

        public static MemoryStream GetCsv(string[] fieldsToExpose, DataTable data)
        {
            var stream = new MemoryStream();

            var sw = new StreamWriter(stream, Encoding.UTF8);
            //sw.Write("{");

            //foreach (var kvp in keysAndValues)
            //{
            //    sw.Write("'{0}':", kvp.Key);
            //    sw.Flush();
            //    ser.WriteObject(stream, kvp.Value);
            //}
            for (var i = 0; i < fieldsToExpose.Length; i++)
            {
                if (i != 0)
                {
                    sw.Write(",");
                }
                sw.Write("\"");
                sw.Write(fieldsToExpose[i].Replace("\"", "\"\""));
                sw.Write("\"");
            }
            sw.Write("\n");

            foreach (DataRow row in data.Rows)
            {
                for (var i = 0; i < fieldsToExpose.Length; i++)
                {
                    if (i != 0)
                    {
                        sw.Write(",");
                    }
                    sw.Write("\"");
                    sw.Write(row[fieldsToExpose[i]].ToString()
                        .Replace("\"", "\"\""));
                    sw.Write("\"");
                }

                sw.Write("\n");
            }
            //sw.Write("}");
            sw.Flush();
            stream.Position = 0;
            //stream.Write(sw.GetBuffer(), 0, (int)ms2.Length);
            return stream;
            //return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
        }

        public static void SaveStreamToFile(string fileFullPath, MemoryStream stream)
        {
            if (File.Exists(fileFullPath)) return;
            File.Exists(fileFullPath);
            if (stream.Length == 0) return;
            // Create a FileStream object to write a stream to a file
            using (var fileStream = File.Create(fileFullPath, (int) stream.Length))
            {
                // Fill the bytes[] array with the stream data
                var bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }
}