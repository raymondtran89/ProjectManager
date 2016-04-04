/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: CommonLogger
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using log4net;
using log4net.Config;

namespace Logger
{
    public class CommonLogger
    {
        private const string DEFAULT_LOGGER = "DefaultLogger";

        static CommonLogger()
        {
            //log4net.Config.DOMConfigurator.Configure();
            XmlConfigurator.Configure();
            DefaultLogger = LogManager.GetLogger(DEFAULT_LOGGER);
        }

        /// <summary>
        ///     Ghi log chung cho toan project
        /// </summary>
        public static ILog DefaultLogger { get; set; }
    }
}