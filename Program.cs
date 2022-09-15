using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace DCityBankService
{
  class Program
    {
        static void Main(string[] args)
        {
            InitializeService();
        }
        private static void InitializeService()
        {

            XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));

            Topshelf.Host h = HostFactory.New(x =>
            {
                x.Service<ServiceBootstrapper>(s =>
                {
                    s.ConstructUsing(name => new ServiceBootstrapper());
                    s.WhenStarted(wd => wd.Start());
                    s.WhenStopped(wd => wd.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("Babilon.DcityService");
                x.SetDisplayName("Babilon.DcityService");
                x.SetServiceName("Babilon.DcityService");
            });

            h.Run();

        }
        public static String GetHash(String text, String key)
        {
            // change according to your needs, an UTF8Encoding
            // could be more suitable in certain situations
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).ToLower();
        }
    }
}
