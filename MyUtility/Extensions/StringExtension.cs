using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtility.Extensions
{
    public static class StringExtension
    {
        public const string BrTag = "<br />";

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Format chuoi theo nhom VD: 434.345.334</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="numberOfPart"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatAsGroup(this string str, int numberOfPart = 4, string separator = ".")
        {
            str = Regex.Replace(str, string.Format("(?!^).{{{0}}}", numberOfPart), string.Format("{0}$0", separator),
                RegexOptions.RightToLeft);
            return str;
        }

        public static string FormatBytes(long bytes)
        {
            string[] suffix = {"B", "KB", "MB", "GB", "TB"};
            int i;
            double dblSByte = bytes;
            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes/1024.0;
            }

            return string.Format("{0:0.##} {1}", dblSByte, suffix[i]);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Cat chuoi theo length quy dinh</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string MakeSortString(this string str, int length)
        {
            if (str.Length < length)
                length = str.Length;
            var s = str.Substring(0, length);
            if (s.Length < str.Length && s.LastIndexOf(' ') > 0)
                s = str.Substring(0, s.LastIndexOf(' '));
            if (str.Length > s.Length)
                s += "...";
            return s;
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Chuyen chuoi thanh int number</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultInt"></param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultInt = 0)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return defaultInt;
            }
        }

        public static long ToLong(this string str)
        {
            return long.Parse(str);
        }

        public static string TextId(this string str, int length = 100)
        {
            return StringCommon.GetTextID(str, length);
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>
        ///         Chuyen so thanh chu
        ///     </para>
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConverToNumberToString(string number)
        {
            string[] dv = {"", "mươi", "trăm", "nghìn", "triệu", "tỉ"};
            string[] cs = {"không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"};

            var len = number.Length;
            number += "ss";
            var doc = "";
            var rd = 0;

            var i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                var n = (len - i + 2)%3 + 1;

                //Kiem tra so 0
                var found = 0;
                int j;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                int k;
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        var ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;

                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;

                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;

                            default:
                                doc += cs[number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }

                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n)%9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n)/9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else if (found != 0) doc += dv[(len - i - n + 1)%9/3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[number[0] - 48];

            var str = doc.ToCharArray();
            var result = "";
            for (var l = 0; l < str.Length; l++)
            {
                if (l == 0)
                {
                    result += str[l].ToString().ToUpper();
                }
                else
                {
                    result += str[l];
                }
            }
            return result;
        }

        public static string FormatDateTime(this string datetime)
        {
            var dateBetween = DateTime.Now - DateTime.Parse(datetime);
            if (dateBetween.Days < 1)
            {
                return string.Format("{0} giờ {1} phút", dateBetween.Hours, dateBetween.Minutes);
            }
            return DateTime.Parse(datetime).ToString("dd/MM/yyyy");
        }

        public static DateTime ToDateTimeParseExact(this string text, DateTime defaultError,
            string format = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return DateTime.ParseExact(text, format, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return defaultError;
            }
        }

        public static string ToVNdate(this DateTime? date, string format = "dd/MM/yyyy")
        {
            if (date.HasValue)
                return date.Value.ToString(format);
            return "";
        }

        public static string FormatDateTimeHhmmss(this string datetime)
        {
            var dateBetween = DateTime.Now - DateTime.Parse(datetime);
            if (dateBetween.Days < 1)
            {
                return string.Format("{0} giờ {1} phút", dateBetween.Hours, dateBetween.Minutes);
            }
            return DateTime.Parse(datetime).ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        ///     ThongNT : Loai bo dau tieng viet
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToUnSign(string s)
        {
            s = s.Replace("\"", " ");
            s = Regex.Replace(s, @"\s+", " ");
            s = s.Replace(@"/", " ");
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = s.Normalize(NormalizationForm.FormD);
            var str = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
            return str.Trim().Replace(" ", "-");
        }

        public static string CleanUpWordHtml(string html)
        {
            html = MakeSafe(html);
            html = FixEntityCharacters(html);

            var nvc = new NameValueCollection
            {
                {@"<!--(\w|\W)+?-->", string.Empty},
                {@"<title>(\w|\W)+?</title>", string.Empty},
                {@"<style>(\w|\W)+?</style>", string.Empty},
                {@"<script\s[^>]*>.+</script>", string.Empty},
                {@"<script (\w|\W)+?\><script>", string.Empty},
                {@"<object.*>.*<object>", string.Empty},
                {@"\[flash\](\w|\W)+?\[/flash\]", string.Empty},
                {@"\[music\](\w|\W)+?\[/music\]", string.Empty},
                {@"\[media\](\w|\W)+?\[/media\]", string.Empty},
                {@"<video>(\w|\W)+?</video>", string.Empty},
                {@"<form>(\w|\W)+?</form>", string.Empty},
                {@"<iframe (\w|\W)+?>(\w|\W)+?</iframe>", string.Empty},
                {@"<input.*/>", string.Empty},
                {@"<input.*></input>", string.Empty},
                {@"<div.*style=.*absolute.*>", "<div"},
                {@"\s?class=\w+", string.Empty},
                {
                    @"(font-family:[^&gt;]*[;'])|(font-size:[^&gt;]*[;'])|(line-height:[^&gt;]*[;'])|(MsoNormal)|(&lt;!--\[if.*?&lt;!\[endif\]--&gt;)",
                    string.Empty
                },
                {@"<(meta|link|/?o:|/?st\d|/?head|/?html|body|/?body|!\[)[^>]*?>", string.Empty},
                {@"(<[^>]+>)+&nbsp;(</\w+>)+", string.Empty},
                {@"\s+v:\w+=""[^""]+""", string.Empty},
                {@"(\n\r){2,}", string.Empty},
                {@"<[/]?(xml|del|ins|[ovwxp]:\w+)[^>]*?>", string.Empty},
                {@"<([^>]*)(?:lang|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>"}
            };

            //#Begin Hoang them
            //nvc.Add(@"<embed (\w|\W)+?>", string.Empty);
            //nvc.Add(@"<div .*>.*<img (\w|\W)* />", "<div>");
            //nvc.Add(@"<? background", string.Empty);
            //#End Hoang them


            html = nvc.Keys.Cast<string>().Aggregate(html, (current, key) => Regex.Replace(current, key, nvc[key], RegexOptions.None));

            return html.Trim();
        }

        public static string MakeSafe(string value)
        {
            var v = new StringBuilder(value);

            v
                .Replace("&Agrave;", "À")
                .Replace("&Aacute;", "Á")
                .Replace("&Acirc;", "Â")
                .Replace("&Atilde;", "Ã")
                .Replace("&Ccedil;", "Ç")
                .Replace("&Egrave;", "È")
                .Replace("&Eacute;", "É")
                .Replace("&Ecirc;", "Ê")
                .Replace("&Igrave;", "Ì")
                .Replace("&Iacute;", "Í")
                .Replace("&Icirc;", "Î")
                .Replace("&ETH;", "Ð")
                .Replace("&Ograve;", "Ò")
                .Replace("&Oacute;", "Ó")
                .Replace("&Ocirc;", "Ô")
                .Replace("&Otilde;", "Õ")
                .Replace("&Ugrave;", "Ù")
                .Replace("&Uacute;", "Ú")
                .Replace("&Yacute;", "Ý")
                .Replace("&agrave;", "à")
                .Replace("&aacute;", "á")
                .Replace("&acirc;", "â")
                .Replace("&atilde;", "ã")
                .Replace("&aring;", "å")
                .Replace("&ccedil;", "ç")
                .Replace("&egrave;", "è")
                .Replace("&eacute;", "é")
                .Replace("&ecirc;", "ê")
                .Replace("&igrave;", "ì")
                .Replace("&iacute;", "í")
                .Replace("&icirc;", "î")
                .Replace("&ograve;", "ò")
                .Replace("&oacute;", "ó")
                .Replace("&ocirc;", "ô")
                .Replace("&otilde;", "õ")
                .Replace("&ugrave;", "ù")
                .Replace("&uacute;", "ú")
                .Replace("&ucirc;", "û")
                .Replace("&yacute;", "ý");

            return v.ToString();
        }

        private static string FixEntityCharacters(string html)
        {
            var nvc = new NameValueCollection {{"“", "&ldquo;"}, {"”", "&rdquo;"}, {"–", "&mdash;"}};


            return nvc.Keys.Cast<string>().Aggregate(html, (current, key) => current.Replace(key, nvc[key]));
        }

        public static string RemoveAndReplaceAttribute(string input, bool isReplaceBrTag)
        {
            var strRet = input;

            if (isReplaceBrTag)
            {
                strRet = strRet.Replace(BrTag + "\r\n", BrTag)
                    .Replace("\r\n", BrTag)
                    .Replace("\r", BrTag)
                    .Replace("\n", BrTag);
            }

            strRet = ReplaceHtmlTag(strRet); // xoa cac the nguy hiem

            return strRet;
        }

        /// <summary>
        ///     xoa cac the html khong cho phep va chan ky tu xau
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string ReplaceHtmlTag(string input)
        {
            var strRet = input;

            strRet = CleanUpWordHtml(strRet.Trim());

            // replace the html
            strRet = ReplaceHtmlTag(strRet);
            return strRet == string.Empty ? string.Empty : strRet;
        }

        /// <summary>
        ///     Trả về chuỗi random
        /// </summary>
        /// <param name="size">độ dài của chuỗi</param>
        /// <param name="lowerCase">viết hoa hay thường.True:Viết hoa,Flase:Viết thường</param>
        /// <returns>Chuỗi sau khi random</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();

            var rndint = new Random();
            var rnd = new Random();
            for (var i = 0; i < size; i++)
            {
                var so = rnd.Next(0, 2);
                var ch = so == 1 ? Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65))) : Convert.ToChar(rndint.Next(0, 9).ToString());

                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string FormatCoin(double pCoin)
        {
            var strRet = string.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoin);
            if (strRet == "00")
            {
                strRet = "0";
            }
            return strRet;
        }

        /// <summary>
        ///     Author:TrungLD
        ///     tạo chuỗi bất kỳ
        /// </summary>
        /// <returns></returns>
        public static string GenarateRequestId()
        {
            return DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2015-03-17</para>
        ///     <para>Description: Cat chuoi neu chuoi qua dai</para>
        /// </summary>
        /// <returns></returns>
        public static string CutNick(string stringToCut, int lengthCut, string stringReplace)
        {
            var lengthString = stringToCut.Length;

            if (lengthString <= 0 || lengthString <= lengthCut) return stringToCut;
            var stringTemp = stringToCut.Substring(0, lengthCut - stringReplace.Length) + stringReplace;
            return stringTemp;
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            const string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[~`+=" + "\"";
            var specialCharactersArray = specialCharacters.ToCharArray();
            return new string(value.Except(specialCharactersArray).ToArray());
        }

        public static string RemoveSpecialCharacters_v2(this string str)
        {
            return Regex.Replace(str,
                @"[^a-zA-Z0-9\-ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ_{1}\s]",
                string.Empty);
        }

        public static string ReplaceEndString(this string text, int length = 3, string charStringReplace = "*")
        {
            if (string.IsNullOrEmpty(text)) return "";

            if (text.Length > length)
                text = text.Substring(0, text.Length - length);
            else
            {
                length = text.Length;
                text = "";
            }

            for (var i = 0; i < length; i++)
            {
                text += charStringReplace;
            }
            return text;
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2015-07-15</para>
        ///     <para>Description: Chuyen doi ti le cuoc bong da</para>
        /// </summary>
        /// <returns></returns>
        public static string ConvertToRatioFootball(double betRate)
        {
            var betrateTemp = betRate;
            if (betRate < 0)
            {
                var betrateSplit = betRate.ToString(CultureInfo.InvariantCulture).Split('-');
                betRate = double.Parse(betrateSplit[1]);
            }
            var strBetRate = "";
            var temp = betRate.ToString(CultureInfo.InvariantCulture).Split('.');
            const double tolerance = 0;
            if (temp.Length < 2)
                strBetRate = betRate.ToString(CultureInfo.InvariantCulture);
            else
            {
                if (betRate < 1)
                {
                    if (betRate > 0 && betRate < 0.5)
                        strBetRate = "1/4";
                    if (Math.Abs(betRate - 0.5) < tolerance)
                        strBetRate = "1/2";
                    if (betRate > 0.5 && betRate < 1)
                        strBetRate = "3/4";
                }
                else
                {
                    var strConvert = "0." + temp[1];
                    if (double.Parse(strConvert) > 0 && double.Parse(strConvert) < 0.5)
                        strBetRate = temp[0] + " 1/4";
                    if (Math.Abs(double.Parse(strConvert) - 0.5) < tolerance)
                        strBetRate = temp[0] + " 1/2";
                    if (double.Parse(strConvert) > 0.5 && double.Parse(strConvert) < 1)
                        strBetRate = temp[0] + " 3/4";
                }
            }
            if (betrateTemp < 0)
                return "-" + strBetRate;
            return strBetRate;
        }
    }
}