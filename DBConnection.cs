using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DCityBankService
{
    public class DBConnection
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(DBConnection));
        private readonly string DB = ConfigurationManager.AppSettings["DBConection"].ToString();
       // public static string connString = ConfigurationManager.ConnectionStrings["DBConection"].ConnectionString;
        public result DBSUpdateTransactionSucceed(string TransuctionID, string Status)
        {
            result answer = new result();
            string url = DB + "TransactionChangeStatus?TransactionID=" + TransuctionID + "&Status=" + Status + "&partnerId=11";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = (180000);
                request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                answer = JsonConvert.DeserializeObject<result>(response);

                if (answer.code == 1)
                {
                    answer.IsError = false;
                }
                else
                {
                    answer.IsError = true;
                }
                _log.Debug("Request.DBSUpdateTransactionSucceed  URL: " + url + " Answer :" + JsonConvert.SerializeObject(answer));
            }
            catch (Exception ee)
            {
                answer.code = -3;
                answer.Comment = "Ошибка на стороне сервера";
                answer.IsError = true;
                _log.Error("Request.DBSUpdateTransactionSucceed  URL: " + url + " Answer :" + ee);
            }

            return answer;
        }
        public result AddReqID(string TransactionId, string ReqId)
        {
            result answer = new result();
            string url = DB + "TransactionChangeStatus?TransactionId=" + TransactionId + "&ReqId=" + ReqId + "&PartnerId=11&Status=";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 120000;
                request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                _log.Debug("BankApi.AddReqID   Request :TransactionId(" + TransactionId + ") " + url + " Answer:  " + response);
                responseReader.Close();
                answer = JsonConvert.DeserializeObject<result>(response);

                if (answer.code == 1)
                {
                    answer.IsError = false;
                }
                else
                {
                    answer.IsError = true;
                }
            }
            catch (Exception ee)
            {
                answer.code = -3;
                answer.Comment = "Ошибка на стороне сервера";
                answer.IsError = true;
                _log.Debug("BankApi.AddReqID   Request :TransactionId(" + TransactionId + ") " + url + " Answer:  " + ee);

            }
            return answer;
        }

    }
}
