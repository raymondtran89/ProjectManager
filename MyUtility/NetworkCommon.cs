/**********************************************************************
 * Author: PhatVT
 * DateCreate: 11-20-2014
 * Description: Define network common static function
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using MyUtility.Extensions;

namespace MyUtility
{
    public class NetworkCommon
    {
        public enum HttpRequestEnum
        {
            [Description("GET")] Get,

            [Description("POST")] Post,

            [Description("DELETE")] Delete
        }

        /// <summary>
        ///     Get local ip, using for winform and webform
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            string ip = null;

            // Resolves a host name or IP address to an IPHostEntry instance.
            // IPHostEntry - Provides a container class for Internet host address information.
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());

            // IPAddress class contains the address of a computer on an IP network.
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                // InterNetwork indicates that an IP version 4 address is expected
                // when a Socket connects to an endpoint
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = ipAddress.ToString();
                }
            }
            return ip;
        }

        /// <summary>
        ///     Gửi email
        /// </summary>
        /// <param name="fromEmail">Địa chỉ gửi đi</param>
        /// <param name="fromName">Tên người gửi</param>
        /// <param name="fromPassword">Mật khẩu đăng nhập mail của người gửi</param>
        /// <param name="toEmail">Địa chỉ nhận mail</param>
        /// <param name="toName">Tên người nhận</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="body">Nội dung</param>
        /// <param name="host">Host của email. Gmail là "smtp.gmail.com"</param>
        /// <param name="port">Cổng. Nếu là Gmail thì là 587</param>
        /// <param name="isEnableSsl">Có mở chế độ SSL không</param>
        /// <param name="ccAddresses"></param>
        /// <param name="bccAddresses"></param>
        /// <returns></returns>
        public static bool SendMail(string fromEmail, string fromName, string fromPassword,
            string toEmail, string toName, string title, string body,
            string host, int port, bool isEnableSsl = false,
            List<MailAddress> ccAddresses = null, List<MailAddress> bccAddresses = null)
        {
            var fromAddress = new MailAddress(fromEmail, fromName);
            var toAddress = new MailAddress(toEmail, toName);

            return SendMail(fromAddress, toAddress, fromPassword, title, body, host, port, isEnableSsl,
                ccAddresses, bccAddresses);
        }

        /// <summary>
        ///     Gửi email
        /// </summary>
        /// <param name="fromMail">Địa chỉ gửi đi</param>
        /// <param name="fromPassword">Mật khẩu đăng nhập mail của người gửi</param>
        /// <param name="toEmail">Địa chỉ nhận mail</param>
        /// <param name="replyEmail">Địa chỉ reply</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="body">Nội dung</param>
        /// <param name="host">Host của email. Gmail là "smtp.gmail.com"</param>
        /// <param name="port">Cổng. Nếu là Gmail thì là 587</param>
        /// <param name="isEnableSsl">Có mở chế độ SSL không</param>
        /// <param name="ccAddresses"></param>
        /// <param name="bccAddresses"></param>
        /// <returns></returns>
        public static bool SendMail(MailAddress fromMail, MailAddress toEmail,
            string fromPassword, string title, string body, string host, int port,
            bool isEnableSsl = false, List<MailAddress> ccAddresses = null,
            List<MailAddress> bccAddresses = null, MailAddress replyEmail = null)
        {
            using (var message = new MailMessage(fromMail, toEmail)
            {
                Subject = title,
                Body = body,
                IsBodyHtml = true
            })
            {
                if (replyEmail != null)
                {
                    message.ReplyTo = replyEmail;
                }

                if (ccAddresses != null)
                {
                    foreach (MailAddress address in ccAddresses)
                    {
                        message.Bcc.Add(address);
                    }
                }

                if (bccAddresses != null)
                {
                    foreach (MailAddress address in bccAddresses)
                    {
                        message.CC.Add(address);
                    }
                }

                var smtp = new SmtpClient(host, port)
                {
                    EnableSsl = isEnableSsl,
                    Credentials = new NetworkCredential(fromMail.Address, fromPassword)
                };
                smtp.Send(message);
            }
            return true;
        }

        /// <summary>
        ///     Hàm gửi request dạng Get
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="referer">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendGetRequest(string requestUrl, CookieContainer cookieCon, string referer,
            out string htmlResult
            , out string nextLocation, bool allowRedirect, int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);

            webRequest.AllowAutoRedirect = false;
            webRequest.AutomaticDecompression = DecompressionMethods.GZip;
            webRequest.CookieContainer = cookieCon;

            // Time out is miliécond
            webRequest.Timeout = timeOut*1000;

            webRequest.UserAgent =
                "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.7";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Headers[HttpRequestHeader.AcceptLanguage] = "en-us,en;q=0.5";
            webRequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            webRequest.Headers[HttpRequestHeader.AcceptCharset] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            webRequest.KeepAlive = false; //Fix "The server committed a protocol violation. Section=ResponseStatusLine"
            if (!string.IsNullOrEmpty(referer.Trim()))
                webRequest.Referer = referer;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            webRequest.Method = "GET";

            try
            {
                using (var response = (HttpWebResponse) webRequest.GetResponse())
                {
                    cookieCon.Add(webRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loadSuccess = false;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        ///     Hàm gửi request dạng Get, phiên bản đơn giản
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="html">Kết quả html trả về</param>
        /// <returns></returns>
        public static bool SendGetRequest(string requestUrl, out string html)
        {
            string next;
            return SendGetRequest(requestUrl, new CookieContainer(), string.Empty, out html, out next, true);
        }

        /// <summary>
        ///     Hàm gửi request dạng Post có header
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="postData">Dữ liệu post</param>
        /// <param name="referer">Địa chỉ liên quan được gọi trước đó</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="headers">Thông tin Header</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendPostRequest(string requestUrl, CookieContainer cookieCon, string postData, string referer
            , out string htmlResult, out string nextLocation, bool allowRedirect, IEnumerable<string> headers,
            int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            byte[] dataByte = new ASCIIEncoding().GetBytes(postData);
            var myRequest = (HttpWebRequest) WebRequest.Create(requestUrl);
            myRequest.Method = "POST";
            myRequest.KeepAlive = true;
            myRequest.AllowAutoRedirect = allowRedirect;
            myRequest.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            myRequest.Headers.Add("Keep-Alive:15");

            // Time out is miliécond
            myRequest.Timeout = timeOut*1000;

            if (headers != null)
            {
                foreach (string header in headers)
                {
                    myRequest.Headers.Add(header);
                }
            }

            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            myRequest.ContentType = "application/json";
            if (!string.IsNullOrEmpty(referer.Trim()))
            {
                myRequest.Referer = referer;
            }
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20100101 Firefox/4.0";
            myRequest.ContentLength = postData.Length;
            myRequest.Proxy = null;
            myRequest.CookieContainer = cookieCon;
            try
            {
                Stream postStream = myRequest.GetRequestStream();
                postStream.Write(dataByte, 0, dataByte.Length);
                postStream.Flush();
                postStream.Close();
                using (var response = (HttpWebResponse) myRequest.GetResponse())
                {
                    cookieCon.Add(myRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    htmlResult = string.Empty;
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loadSuccess = false;
                resultOutput = ex.Message;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        ///     Hàm gửi request dạng Post
        /// </summary>
        /// <param name="requestUrl">Địa chỉ cần gọi</param>
        /// <param name="cookieCon">Cookie</param>
        /// <param name="postData">Dữ liệu post</param>
        /// <param name="referer">Địa chỉ liên quan được gọi trước đó</param>
        /// <param name="htmlResult">Kết quả html trả về</param>
        /// <param name="nextLocation">Địa chỉ tiếp theo, nếu có</param>
        /// <param name="allowRedirect">Cho phép tự redirect không</param>
        /// <param name="timeOut">Thời gian time out. Tính bằng giây. Mặc định là 30s</param>
        /// <returns></returns>
        public static bool SendPostRequest(string requestUrl, CookieContainer cookieCon, string postData, string referer
            , out string htmlResult, out string nextLocation, bool allowRedirect, int timeOut = 30)
        {
            bool loadSuccess = true;
            nextLocation = string.Empty;
            string resultOutput = string.Empty;
            byte[] dataByte = new ASCIIEncoding().GetBytes(postData);
            var myRequest = (HttpWebRequest) WebRequest.Create(requestUrl);
            myRequest.Method = "POST";
            myRequest.KeepAlive = true;
            myRequest.AllowAutoRedirect = allowRedirect;
            myRequest.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            myRequest.Headers.Add("Keep-Alive:15");
            myRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            // Time out is miliécond
            myRequest.Timeout = timeOut*1000;

            if (!string.IsNullOrEmpty(referer.Trim()))
            {
                myRequest.Referer = referer;
            }
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20100101 Firefox/4.0";
            myRequest.ContentLength = postData.Length;
            myRequest.Proxy = null;
            myRequest.CookieContainer = cookieCon;
            try
            {
                Stream postStream = myRequest.GetRequestStream();
                postStream.Write(dataByte, 0, dataByte.Length);
                postStream.Flush();
                postStream.Close();
                using (var response = (HttpWebResponse) myRequest.GetResponse())
                {
                    cookieCon.Add(myRequest.RequestUri, response.Cookies);
                    nextLocation = response.GetResponseHeader("Location");
                    using (var buffer = new BufferedStream(response.GetResponseStream()))
                    {
                        using (var readStream = new StreamReader(buffer, Encoding.UTF8))
                        {
                            resultOutput = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                loadSuccess = false;
            }
            finally
            {
                htmlResult = resultOutput;
            }
            return loadSuccess;
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2015-04-16</para>
        ///     <para>Description: Goi request data</para>
        /// </summary>
        /// <returns></returns>
        public static string CallRequest(string urlCall, HttpRequestEnum method, string postData = "")
        {
            WebRequest wRequest = WebRequest.Create(urlCall);
            wRequest.Method = method.Text();

            if (method == HttpRequestEnum.Post && postData != "")
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                wRequest.ContentType = "application/x-www-form-urlencoded";
                wRequest.ContentLength = byteArray.Length;

                Stream streamRequest = wRequest.GetRequestStream();
                streamRequest.Write(byteArray, 0, byteArray.Length);
                streamRequest.Close();
            }

            string responseFromServer = string.Empty;

            try
            {
                WebResponse wResponse = wRequest.GetResponse();
                Stream streamResponse = wResponse.GetResponseStream();

                if (streamResponse != null)
                {
                    var reader = new StreamReader(streamResponse);
                    responseFromServer = reader.ReadToEnd();

                    reader.Close();
                    streamResponse.Close();
                }

                wResponse.Close();
            }
            catch
            {
                responseFromServer = string.Empty;
            }

            return responseFromServer;
        }
    }
}