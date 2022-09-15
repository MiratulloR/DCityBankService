using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace DCityBankService
{
    public class ServiceBootstrapper
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceBootstrapper));
        private HttpSelfHostServer _serv;
        private void StartSelfHost()
        {


            var selfHostConfiguration = new HttpSelfHostConfiguration("http://127.0.0.1:3535");
            selfHostConfiguration.Routes.MapHttpRoute
                (
                    name: "DefaultApiRoute",
                    routeTemplate: "{controller}",
                    defaults: null
                );
            _serv = new HttpSelfHostServer(selfHostConfiguration);
            _serv.OpenAsync();




        }
        public void Start()
        {

            StartSelfHost();
            _log.Debug("====== start service ======");

        }
        public void Stop()
        {
            _serv.CloseAsync();
            _log.Debug("====== stop service ======");
        }
    }
}
