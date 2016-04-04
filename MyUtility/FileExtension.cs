using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace MyUtility
{
    public static class FileExtension
    {
        public static DataTable CsvCardToDataTable(Stream stream)
        {
            var dt = new DataTable();
            var csvreader = new StreamReader(stream);

            var readLine = csvreader.ReadLine();
            if (readLine == null) return dt;
            var headers = readLine.Split(',');
            foreach (var header in headers)
            {
                dt.Columns.Add(string.IsNullOrEmpty(header) ? string.Empty : header.Trim());
            }
            while (!csvreader.EndOfStream)
            {
                var line = readLine;
                var rows = line.Split(',');
                var dr = dt.NewRow();
                for (var i = 0; i < headers.Length; i++)
                {
                    dr[i] = string.IsNullOrEmpty(rows[i]) ? string.Empty : rows[i].Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        ///     Kiểm tra xem có phải là ảnh ko?
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                    Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Kiểm tra Extension ten file
        /// </summary>
        /// <returns></returns>
        public static bool IsValidExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var splitFileName = fileName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            if (splitFileName.Length <= 0) return false;
            var length = splitFileName.Length;

            switch (splitFileName[length - 1].ToLower())
            {
                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                    return true;
            }

            return false;
        }
    }
}