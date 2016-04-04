using System.IO;

namespace MyUtility
{
    public class IoCommon
    {
        /// <summary>
        ///     Kiểm tra một đường dẫn có phải là thư mục không
        /// </summary>
        /// <param name="path">Đường dẫn cần kiểm tra</param>
        public static bool IsDirectory(string path)
        {
            // get the file attributes for file or directory
            var attr = File.GetAttributes(path);

            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>
        ///     Xóa file hoặc thư mục tùy vào đường dẫn truyền vào là gì
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Delete(string path)
        {
            if (!IsExist(path))
            {
                return true;
            }

            return IsDirectory(path) ? DeleteDirectory(path) : DeleteFile(path);
        }

        /// <summary>
        ///     Xóa file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Tạo thư mục
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Xóa thư mục
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Kiểm tra xem file hoặc thư mục có tồn tại không
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsExist(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        ///     Read file and return a text
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, string fileName)
        {
            return ReadFile(Path.Combine(filePath, fileName));
        }

        /// <summary>
        ///     Đọc file theo full path
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string ReadFile(string fullPath)
        {
            // Check existed file, create if it's not then goto result step
            if (!File.Exists(fullPath))
            {
                var f = File.Create(fullPath);
                f.Close();
                f.Dispose();
                return string.Empty;
            }

            // Open file and read all text
            TextReader file = new StreamReader(fullPath);
            var strLine = file.ReadToEnd();
            file.Close();
            file.Dispose();

            return strLine;
        }

        /// <summary>
        ///     Write text to file
        /// </summary>
        /// <param name="text"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool WriteFile(string text, string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            // Check existed directory, create if it's not
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            // Check existed file, create if it's not then goto result step
            filePath = Path.Combine(filePath, fileName);
            if (!File.Exists(filePath))
            {
                var f = File.Create(filePath);
                f.Close();
                f.Dispose();
            }

            // Open file and write text
            TextWriter file = new StreamWriter(filePath);
            file.Write(text);
            file.Close();
            file.Dispose();

            return true;
        }
    }
}