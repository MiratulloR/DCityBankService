using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DCityBankService
{
    public class BankRequests
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(BankRequests));
        private readonly string BankLink = ConfigurationManager.AppSettings["BankLink"].ToString();
        private readonly string BankLinkCA = ConfigurationManager.AppSettings["BankLink"].ToString();
        private readonly string login = "90132";
        private readonly string password = "440824";
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public PaymentCheckAnswer CheckAccount(string msisdn, string amount, string serviceID, string toNumberID, string region, long TransactionId, string ProviderID, string UserNumber)
        {
            // DateTime date = DateTime.Now;
            string datahash = login + password;
            string sign = CreateMD5(datahash);
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string result = date.Substring(2, 12);
            // Console.WriteLine("RESULT: {0}", result);
            PaymentCheckAnswer check = new PaymentCheckAnswer();
            string transaction;
            try
            {
                string url = BankLink + "check" + "&login=" + login + "&txn_id=" + TransactionId + "&account=" + msisdn + "&prvid=" + serviceID + "&ccy=" + "TJS" + "&sum=" + amount + "&sign=" + sign + "&cr_amount=" + true + "&txn_date=" + result;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNode node = doc.SelectSingleNode("response/result");
                XmlNode payid = doc.SelectSingleNode("response/osmp_txn_id");
                XmlNode commentnode = doc.SelectSingleNode("response/comment");
                string json = JsonConvert.SerializeXmlNode(doc);

                transaction = node.InnerText.Trim();
                string comment = commentnode.InnerText.Trim();
                string payd = payid.InnerText.Trim();
                // transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);
                _log.Debug(url + " " + response);
                if (transaction == "0")
                {
                    check.isError = false;
                    check.Comment = comment;
                    check.PaymentID = payd;
                    check.Status = 1;

                }
                else if (transaction == "5")
                {
                    check.Comment = comment;
                    check.isError = true;
                    check.PaymentID = payd;
                    check.Status = -5;
                }
                else if (transaction == "13")
                {
                    check.Comment = comment;
                    check.isError = true;
                    check.PaymentID = payd;
                    check.Status = -13;
                }
                else if (transaction == "4")
                {
                    check.Comment = comment;
                    check.isError = true;
                    check.PaymentID = payd;
                    check.Status = -4;
                }
                else
                {
                    check.isError = true;
                    check.Status = -5;
                    check.Comment = comment;

                }
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }

        public PaymentAnswer CreateTransaction(string msisdn, string amount, string serviceID, string toNumberID, string region, long TransactionId, string ProviderID, string UserNumber)
        {
            PaymentAnswer answer = new PaymentAnswer();
            Functions functions = new Functions();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string result = date.Substring(2, 12);
            Console.WriteLine("RESULT: {0}", result);
            string dataToHash = login + TransactionId + toNumberID + password;
            string sign = CreateMD5(dataToHash);
            //CreateTransaction transaction2 = new CreateTransaction();
            string transaction = "";
            try
            {
                string url = BankLink + "pay" + "&login=" + login + "&txn_id=" + TransactionId + "&account=" + toNumberID + "&ccy=" + "TJS" + "&prvid=" + serviceID + "&sum=" + amount + "&sign=" + sign + "&txn_date=" + result + "&wallet=" + "992" + msisdn;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNode node = doc.SelectSingleNode("response/result");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                XmlNode payid = doc.SelectSingleNode("response/osmp_txn_id");
                string json = JsonConvert.SerializeXmlNode(doc);
                transaction = node.InnerText.Trim();
                string payd = payid.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                // transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);
                _log.Debug(url + " " + response);
                if (transaction == "0")
                {
                    answer.isError = false;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = 1;
                    
                }
                else if (transaction == "4")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -4;
                }
                else if (transaction == "13")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -13;
                }
                else if (transaction == "90")
                {
                    answer.isError =false;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -90;
                   
                }
                else if (transaction == "97")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -97;
                }
                else if (transaction == "12")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -12;
                }
                else if (transaction == "220")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -220;
                }
                else if (transaction == "300")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -300;
                }
                else if (transaction == "1")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -1;
                }
                else if (transaction == "7")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -7;
                }
                else if (transaction == "8")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -8;
                }
                else if (transaction == "10")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -10;
                }
                else
                {
                    answer.isError = true;
                    answer.Code = Convert.ToInt32(transaction);
                    answer.Comment = "Произошла ошибка";

                }
                return answer;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }

        public CreateUser CreateUser(string msisdn, string userid, string username, string userSurname, string middleName)
        {
           
            CreateUser answer = new CreateUser();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string result = date.Substring(2, 12);
            Console.WriteLine("RESULT: {0}", result);
            string dataToHash = login + userid + msisdn + password;
            string sign = CreateMD5(dataToHash);
            //CreateTransaction transaction2 = new CreateTransaction();
            string transaction = "";
            try
            {
                string url = BankLink + "pay" + "&login=" + login + "&txn_id=" + userid + "&account=" + msisdn + "&ccy=" + "TJS" + "&prvid=" + 124 + "&sum=" + "0.00" + "&sign=" + sign + "&txn_date=" + result + "&wallet=" + "992" + msisdn;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                request.Timeout = 10000;
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();

                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNode node = doc.SelectSingleNode("response/result");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                XmlNode payid = doc.SelectSingleNode("response/osmp_txn_id");
                string json = JsonConvert.SerializeXmlNode(doc);
                transaction = node.InnerText.Trim();
                string payd = payid.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                // transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);
                _log.Debug(url + " " + response);
               
                if (transaction == "0")
                {               
                    answer.isError = false;                                                                         
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = 1;
                    BankUserInfo userinfo = new BankUserInfo();
                    userinfo.balance = "0";
                    userinfo.name = username;
                    userinfo.surname = userSurname;
                    userinfo.middlename = middleName;
                    userinfo.identified = CheckifIdentifiedWalletDcity(msisdn);
                    userinfo.status = true;
                    userinfo.type = "wallet";
                    userinfo.msisdn = msisdn;
                    userinfo.number = "992" + msisdn;
                    userinfo.clientCode = "992" + msisdn;
                    userinfo.currency = "TJS";
                    answer.User = userinfo;
                }
               
                else if (transaction == "4")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -4;
                }
                else if (transaction == "13")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -13;
                }
                else if (transaction == "90")
                { 
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -90;
                }
                else if (transaction == "97")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -97;
                }
                else if (transaction == "12")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -12;
                }
                else if (transaction == "220")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -220;
                }
                else if (transaction == "300")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -300;
                }
                else if (transaction == "1")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -1;
                }
                else if (transaction == "7")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -7;
                }
                else if (transaction == "8")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -8;
                }
                else if (transaction == "10")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -10;
                }
                else
                {
                    answer.isError = true;
                    answer.Code = Convert.ToInt32(transaction);
                    answer.Comment = "Произошла ошибка";

                }
                return answer;
            }
            catch (Exception ee)
            {
                if (ee is TimeoutException)
                {
                    answer.isError = true;
                    answer.Code = -98;
                    answer.Comment = "Произошла ошибка";
                    _log.Error("-98 " + msisdn);
                    return answer;
                }
                else
                {
                    _log.Error("CreateUser"+ ee.Message+" "+msisdn );
                    answer.Code = -99;
                    answer.Comment = "Error";
                    answer.isError = true;
                    return answer;
                }
               
            }

        }
        public CreateUser RemoveUser(string msisdn, string userid, string username, string userSurname, string middleName)
        {

            CreateUser answer = new CreateUser();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string result = date.Substring(2, 12);
            Console.WriteLine("RESULT: {0}", result);
            string dataToHash = login + userid + msisdn + password;
            string sign = CreateMD5(dataToHash);
            //CreateTransaction transaction2 = new CreateTransaction();
            string transaction = "";
            try
            {
                string url = BankLink + "removeuser" + "&login=" + login + "&txn_id=" + userid + "&account=" + msisdn + "&ccy=" + "TJS" + "&prvid=" + 124 + "&sum=" + "0.00" + "&sign=" + sign + "&txn_date=" + result + "&wallet=" + "992" + msisdn;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                request.Timeout = 10000;
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();

                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNode node = doc.SelectSingleNode("response/result");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                XmlNode payid = doc.SelectSingleNode("response/osmp_txn_id");
                string json = JsonConvert.SerializeXmlNode(doc);
                transaction = node.InnerText.Trim();
                string payd = payid.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                // transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);
                _log.Debug(url + " " + response);

                if (transaction == "0")
                {
                    answer.isError = false;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = 1;
                    BankUserInfo userinfo = new BankUserInfo();
                    userinfo.balance = "0";
                    userinfo.name = username;
                    userinfo.surname = userSurname;
                    userinfo.middlename = middleName;
                    userinfo.identified = CheckifIdentifiedWalletDcity(msisdn);
                    userinfo.status = true;
                    userinfo.type = "wallet";
                    userinfo.msisdn = msisdn;
                    userinfo.number = "992" + msisdn;
                    userinfo.clientCode = "992" + msisdn;
                    userinfo.currency = "TJS";
                    answer.User = userinfo;
                }

                else if (transaction == "4")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -4;
                }
                else if (transaction == "13")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -13;
                }
                else if (transaction == "90")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -90;
                }
                else if (transaction == "97")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -97;
                }
                else if (transaction == "12")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -12;
                }
                else if (transaction == "220")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -220;
                }
                else if (transaction == "300")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -300;
                }
                else if (transaction == "1")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -1;
                }
                else if (transaction == "7")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -7;
                }
                else if (transaction == "8")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -8;
                }
                else if (transaction == "10")
                {
                    answer.isError = true;
                    answer.Comment = comment;
                    answer.PaymentID = payd;
                    answer.Code = -10;
                }
                else
                {
                    answer.isError = true;
                    answer.Code = Convert.ToInt32(transaction);
                    answer.Comment = "Произошла ошибка";

                }
                return answer;
            }
            catch (Exception ee)
            {
                if (ee is TimeoutException)
                {
                    answer.isError = true;
                    answer.Code = -98;
                    answer.Comment = "Произошла ошибка";
                    _log.Error("-98 " + msisdn);
                    return answer;
                }
                else
                {
                    _log.Error("CreateUser" + ee.Message + " " + msisdn);
                    answer.Code = -99;
                    answer.Comment = "Error";
                    answer.isError = true;
                    return answer;
                }

            }

        }

        public PartnerBalanc Balance(string msisdn)
        {
            DateTime date = DateTime.Now;
            string datahash = login + password;
            string sign = CreateMD5(datahash);
            PartnerBalanc check = new PartnerBalanc();
            string transaction;
            try
            {
                string url = BankLink + "getbalance" + "&login=" + login + "&prvid=" + 70 + "&ccy=" + "TJS" + "&sign=" + sign;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode node2 = doc.SelectSingleNode("response/result");
                XmlNode node = doc.SelectSingleNode("response/balance");
                XmlNode payid = doc.SelectSingleNode("response/limit");
                string json = JsonConvert.SerializeXmlNode(doc);

                transaction = node2.InnerText.Trim();
                string payd = payid.InnerText.Trim();
                string balance = node.InnerText.Trim();
                //transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);
                _log.Debug(url + " " + response);
                if (transaction == "0")
                {
                    check.isError = false;
                    check.Limit = payd;
                    check.Comment = "Success";
                    check.Balance = balance;
                    check.Status = 1;

                }
                else
                {
                    check.isError = true;
                    check.Status = Convert.ToInt32(transaction);
                    check.Comment = "Произошла ошибка";

                }
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public AvailibleProviders AProviders(string prvid)
        {
            Provids provids = new Provids();
            List<Providers> providers = new List<Providers>();
            DateTime date = DateTime.Now;
            string datahash = login + password;
            string sign = CreateMD5(datahash);
            AvailibleProviders check = new AvailibleProviders();
            string transaction;
            try
            {
                string url = BankLink + "prvid" + "&login=" + login + "&sign=" + sign;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode node2 = doc.SelectSingleNode("response/result");
                // XmlNode node = doc.SelectSingleNode("response/balance");
                //XmlNode payid = doc.SelectSingleNode("response/limit");
                string json = JsonConvert.SerializeXmlNode(doc);
                provids = JsonConvert.DeserializeObject<Provids>(json);
                _log.Debug(json);
                transaction = node2.InnerText.Trim();
                //string payd = payid.InnerText.Trim();
                //string balance = node.InnerText.Trim();
                //transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);

                if (transaction == "0")
                {
                    check.isError = false;
                    //check.Limit = payd;
                    check.Comment = "Success";
                    check.Balance = transaction;
                    check.Status = 1;

                }
                else
                {
                    check.isError = true;
                    check.Status = Convert.ToInt32(transaction);
                    check.Comment = "Произошла ошибка";

                }
                _log.Debug(url + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public CancelTransaction CancelTransactions(string trid, string newaccount = null)
        {
            DateTime date = DateTime.Now;
            string datahash = login + trid + password;
            string sign = CreateMD5(datahash);
            CancelTransaction check = new CancelTransaction();
            string transaction;
            try
            {
                string url = BankLink + "cancel" + "&login=" + login + "&txn_id=" + trid + "&newaccount=" + newaccount + "&sign=" + sign;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode nodeResult = doc.SelectSingleNode("response/result");
                XmlNode nodeComment = doc.SelectSingleNode("response/comment");
                XmlNode payid = doc.SelectSingleNode("response/osmp_txn_id");
                string json = JsonConvert.SerializeXmlNode(doc);

                transaction = payid.InnerText.Trim();
                string comment = nodeComment.InnerText.Trim();
                string result = nodeResult.InnerText.Trim();

                //transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);

                if (result == "31")
                {
                    check.isError = false;
                    check.Comment = comment;
                    check.Status = 1;
                    check.PaymentID = transaction;

                }
                else if (result == "30")
                {
                    check.Status = 30;
                    check.isError = false;
                    check.PaymentID = transaction;
                    check.Comment = comment;

                }
                else if (result == "17")
                {
                    check.Status = -17;
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Comment = comment;
                }
                else if (result == "18")
                {
                    check.Status = -18;
                    check.PaymentID = transaction;
                    check.Comment = comment;
                }
                else if (result == "19")
                {
                    check.Status = -19;
                    check.Comment = comment;
                    check.PaymentID = transaction;
                }
                else if (result == "21")
                {
                    check.Status = -21;
                    check.isError = true;
                    check.Comment = comment;
                    check.PaymentID = transaction;
                }
                else if (result == "22")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -6;
                    check.Comment = comment;
                }
                _log.Debug(url + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public Userinfo GetInfo(string msisdn, string serviceid)
        {
            DateTime date = DateTime.Now;
            string datahash = login + password;
            string sign = CreateMD5(datahash);
            Userinfo check = new Userinfo();
            string transaction;
            try
            {
                string url = BankLink + "getinfo" + "&login=" + login + "&account=" + msisdn + "&prvid=" + serviceid + "&ccy=" + "TJS" + "&sign=" + sign;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode node2 = doc.SelectSingleNode("response/result");
                XmlNode node = doc.SelectSingleNode("response/info");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                //XmlNode payid = doc.SelectSingleNode("response/limit");
                string json = JsonConvert.SerializeXmlNode(doc);
                transaction = node2.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                string info = node.InnerText.Trim();
                //transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);

                if (transaction == "0")
                {
                    check.isError = false;
                    check.Info = info;
                    check.Comment = comment;
                    check.Balance = transaction;
                    check.Status = 1;

                }
                else
                {
                    check.isError = true;
                    check.Status = Convert.ToInt32(transaction);
                    check.Comment = "Произошла ошибка";

                }
                _log.Debug(url + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public PaymentCheckstatusAnswer GetStatus(long transactionid)
        {

            DateTime date = DateTime.Now;
            string datahash = login + transactionid + password;
            string sign = CreateMD5(datahash);
            PaymentCheckstatusAnswer check = new PaymentCheckstatusAnswer();
            string transaction;
            try
            {
                string url = BankLink + "getstatus" + "&login=" + login + "&txn_id=" + transactionid + "&sign=" + sign;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode nodeResult = doc.SelectSingleNode("response/result");
                XmlNode trNode = doc.SelectSingleNode("response/prv_txn");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                //XmlNode payid = doc.SelectSingleNode("response/limit");
                string json = JsonConvert.SerializeXmlNode(doc);
                string resultNode = nodeResult.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                transaction = trNode.InnerText.Trim();
                //transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);

                if (resultNode == "0")
                {
                    check.isError = false;
                    check.Comment = comment;
                    check.PaymentID = transaction;
                    check.Status = 1;
                    JsonConvert.SerializeObject(check);
                }
                else if (resultNode == "4")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -4;
                    check.Comment = comment;

                }
                else if (resultNode == "13")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -13;
                    check.Comment = comment;
                }
                else if (resultNode == "90")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -90;
                    check.Comment = comment;
                }
                else if (resultNode == "97")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -97;
                    check.Comment = comment;
                }
                else if (resultNode == "12")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -12;
                    check.Comment = comment;
                }
                else if (resultNode == "220")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -220;
                    check.Comment = comment;
                }
                else if (resultNode == "300")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -300;
                    check.Comment = comment;
                }
                else if (resultNode == "6")
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -6;
                    check.Comment = comment;
                }
                else
                {
                    check.isError = true;
                    check.PaymentID = transaction;
                    check.Status = -9;
                    check.Comment = "Непонятная ошибка";
                }
                _log.Debug(url + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Error(ee.Message);
                check.isError = true;
                check.Status = -98;
                check.Comment = ee.Message;
                return check;
            }

        }
        public CurrencyRate GetCurrencyRate(string amount, string currency)
        {
            string newcurr = currency.ToUpper();
            DateTime date = DateTime.Now;
            string datahash = login + password;
            string sign = CreateMD5(datahash);
            CurrencyRate check = new CurrencyRate();
            string transaction;
            try
            {
                string url = BankLink + "getcurrency" + "&login=" + login + "&prvid=" + 70 + "&ccy=" + newcurr + "&sum=" + amount + "&sign=" + sign;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode node2 = doc.SelectSingleNode("response/result");
                XmlNode node = doc.SelectSingleNode("response/rate");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                //XmlNode payid = doc.SelectSingleNode("response/limit");
                string json = JsonConvert.SerializeXmlNode(doc);
                transaction = node2.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                string info = node.InnerText.Trim();
                //transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);

                if (transaction == "0")
                {
                    check.Rate = info;
                    check.Status = 1;
                    check.Comment = comment;
                    check.date = DateTime.Now;


                }
                else
                {

                    check.Status = -1;
                    check.Comment = "Произошла ошибка";

                }
                _log.Debug(url + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public bool CheckifIdentifiedWalletDcity(string phone)
        {
            bool isden = false;
            DCityAnswer result = new DCityAnswer();
            CheckIden checkIden = new CheckIden();
            checkIden.Login = "90132";
            checkIden.Password = "440824";
            checkIden.Phone = "992" + phone;
            checkIden.Action = "check_identification";
            string datatohash = checkIden.Login + checkIden.Password + checkIden.Phone;
            string hashResult = CreateMD5(datatohash);
            checkIden.Sign = hashResult;
            string json = JsonConvert.SerializeObject(checkIden);
            string data = json;
            string url = "http://agent.dc.tj/api.php";
            try
            {
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 30000;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                }
                string responseContent = null;
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            responseContent = sr.ReadToEnd();
                        }
                    }
                }
                result = JsonConvert.DeserializeObject<DCityAnswer>(responseContent);
                _log.Debug("RequestToService responseContent: " + responseContent);
                _log.Debug("RequestToService :body(" + json + ") " + url + " Answer:  " + JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                _log.Error("RequestToService :body(" + json + ") " + url + " Answer:  " + ex);
            }
            if (result != null)
            {
                if (result.Status == "200")
                {
                    isden = true;
                }
                else
                {
                    isden = false;
                }

            }
            
            return isden;
        }
        public PartnerBalance GetBalance(string userid, string msisdn)
        {
            PartnerBalance userinfo = new PartnerBalance();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string result = date.Substring(2, 12);
            Console.WriteLine("RESULT: {0}", result);
            string dataToHash = login +  msisdn + userid+ password;
            string sign = CreateMD5(dataToHash);
            //CreateTransaction transaction2 = new CreateTransaction();
            string transaction = "";
            try
            {
                string url = BankLink + "pay" + "&login=" + login + "&txn_id=" + msisdn+ "&account=" + userid + "&ccy=" + "TJS" + "&prvid=" + 124 + "&sum=" + "0.00" + "&sign=" + sign + "&txn_date=" + result + "&wallet=" + "992" + userid;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //  request.ContentType = "application/json";
                request.Method = "GET";
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNode node = doc.SelectSingleNode("response/result");
                XmlNode commentNode = doc.SelectSingleNode("response/comment");
                XmlNode payid = doc.SelectSingleNode("response/osmp_txn_id");
                string json = JsonConvert.SerializeXmlNode(doc);
                transaction = node.InnerText.Trim();
                string payd = payid.InnerText.Trim();
                string comment = commentNode.InnerText.Trim();
                // transaction2 = JsonConvert.DeserializeObject<CreateTransaction>(json);
                _log.Debug(url + " " + response);
                if (transaction == "0")
                {                  
                    //PartnerBalance userinfo = new PartnerBalance();
                    userinfo.Balance = "0";
                    userinfo.Identified = CheckifIdentifiedWalletDcity(userid);
                    userinfo.Status = true;
                    userinfo.Type = "wallet";
                    userinfo.Number = "992" + msisdn;
                    userinfo.clientCode = "992" + msisdn;
                    userinfo.Currency = "TJS";
                    userinfo.code = 1;
                }
                else if (transaction == "4")
                {
                    userinfo.code=-4;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    //answer.Code = -4;
                }
                else if (transaction == "13")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -13;
                }
                else if (transaction == "90")
                {

                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -90;
                }
                else if (transaction == "97")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -97;
                }
                else if (transaction == "12")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -12;
                }
                else if (transaction == "220")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -220;
                }
                else if (transaction == "300")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -300;
                }
                else if (transaction == "1")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -1;
                }
                else if (transaction == "7")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -7;
                }
                else if (transaction == "8")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -8;
                }
                else if (transaction == "10")
                {
                    //answer.isError = true;
                    //answer.Comment = comment;
                    //answer.PaymentID = payd;
                    userinfo.code = -10;
                }
                else
                {
                    //answer.isError = true;
                    //answer.Comment = "Произошла ошибка";
                    userinfo.code = Convert.ToInt32(transaction);
                }
                return userinfo;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }
        }
        public CardAttachAnswer CardAttach(string orderid, string Pan, string Expdate, string cvv, string holderName, string phone)
        {

            DateTime date = DateTime.Now;
            string datahash = 100101  + "test123";
            string sign = CreateMD5(datahash);
            CardAttachAnswer check = new CardAttachAnswer();
            string destinationUrl = "https://acquiring.dc.tj/pay/BindingCards.php";
            string requestXml = CreateXml(orderid, Pan, Expdate, cvv, holderName, phone,sign);
            try
            {
                string comment = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //HttpWebResponse response;
                //response = (HttpWebResponse)request.GetResponse();
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                
              //  responseReader.Close();
                _log.Debug(requestXml+" response "+response);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Substring(response.IndexOf(Environment.NewLine)));
                //  doc.Load(response);

                XmlNode resultNode = doc.SelectSingleNode("Result/Status");
                XmlNode commentNode = doc.SelectSingleNode("Result/OrderStatus");
                XmlNode payid = doc.SelectSingleNode("Result/OrderId");
                string status = resultNode.InnerText.Trim();
                string ordsts = commentNode.InnerText.Trim();
                string ordeid = payid.InnerText.Trim();
                string transaction = resultNode.InnerText.Trim();

                if (transaction == "10")
                {
                    check.Comment = ordsts;
                    check.Status = 1;
                    check.Orderid = ordeid;
                }
               else
                {
                    check.Status = -9;
                    check.Comment = ordsts;
                    check.Orderid = ordeid;
                }
                _log.Debug(destinationUrl + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public CardAttachAnswer CardPay(string orderid, string cardId, string serviceid, string toNumberid, decimal amount, string msisdn)
        {

            DateTime date = DateTime.Now;
            string datahash = 100101 + "test123";
            string paySign = CreateMD5(datahash);
            CardAttachAnswer check = new CardAttachAnswer();
            string destinationUrl = "https://acquiring.dc.tj/pay/PayWithCardsID.php";
            string requestXml = CreateXmlPay(orderid, cardId, serviceid, toNumberid, amount, msisdn, paySign);
            try
            {
                string comment = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //HttpWebResponse response;
                //response = (HttpWebResponse)request.GetResponse();
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();
              
                _log.Debug(requestXml + " response " + response);
                XmlDocument doc = new XmlDocument();
               doc.LoadXml(response.Substring(response.IndexOf(Environment.NewLine)));
              //  doc.Load(response);

                XmlNode resultNode = doc.SelectSingleNode("Result/Status");
                XmlNode commentNode = doc.SelectSingleNode("Result/OrderStatus");
                XmlNode payid = doc.SelectSingleNode("Result/OrderId");
                string status = resultNode.InnerText.Trim();
                string ordsts = commentNode.InnerText.Trim();
                string ordeid="";
                if (payid != null)
                {
                    ordeid = payid.InnerText.Trim();
                }
                
                if (status == "10")
                {

                    check.Comment = ordsts;
                    check.Status = 1;
                    check.Orderid = ordeid;
                }
                else
                {

                    check.Status = -9;
                    check.Comment = ordsts;
                    check.Orderid = ordeid;
                }
                _log.Debug(destinationUrl + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public CardAttachAnswer CardBalance( string cardId )
        {

            DateTime date = DateTime.Now;
            string datahash = 100101 + "test123";
            string paySign = CreateMD5(datahash);
            CardAttachAnswer check = new CardAttachAnswer();
            string destinationUrl = "https://acquiring.dc.tj/test/CBalanceCardsID.php";
            string requestXml = CreateXmlBalance( cardId, paySign);
            try
            {
                string comment = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                //HttpWebResponse response;
                //response = (HttpWebResponse)request.GetResponse();
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                responseReader.Close();

                _log.Debug(requestXml + " response " + response);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Substring(response.IndexOf(Environment.NewLine)));
                //  doc.Load(response);

                XmlNode resultNode = doc.SelectSingleNode("Result/Status");
                XmlNode commentNode = doc.SelectSingleNode("Result/OrderStatus");
                XmlNode payid = doc.SelectSingleNode("Result/OrderId");
                string status = resultNode.InnerText.Trim();
                string ordsts = commentNode.InnerText.Trim();
                string ordeid = "";
                if (payid != null)
                {
                    ordeid = payid.InnerText.Trim();
                }

                if (status == "10")
                {

                    check.Comment = ordsts;
                    check.Status = 1;
                    check.Orderid = ordeid;
                }
                else
                {

                    check.Status = -9;
                    check.Comment = ordsts;
                    check.Orderid = ordeid;
                }
                _log.Debug(destinationUrl + " " + response);
                return check;
            }
            catch (Exception ee)
            {
                _log.Debug(ee.Message);
                return null;
            }

        }
        public string CreateXml(string orderid,string Pan,string Expdate, string cvv, string holderName,string phone,string sign)
        {
            string xm = @"<Request>
       <Operation>BindingCards</Operation> " +
       "<Order> " +
       "<Merchant>100101</Merchant> " +
       "<OrderId>"+orderid+"</OrderId>" +
       "<PAN>"+Pan+"</PAN> <ExpDate>"+Expdate+"</ExpDate><CVV>"+cvv+"</CVV> <HolderName>"+holderName+"</HolderName> " +
       "<Phone>"+phone+"</Phone>" +
       "<SenderName>Babilon</SenderName>" +
       "<Sign>"+sign+"</Sign>" +
        "</Order> </Request>";

            return xm;
        }
        public string CreateXmlPay(string orderid, string cardId, string serviceid, string toNumberid, decimal amount, string msisdn,string sign)
        {
            string xm = @"<TKKPG>
					<Request>
						<Operation>PayWithCardsID</Operation>
						<Order>
							<Merchant>100101</Merchant>
							<OrderId>"+orderid+"</OrderId>" +
                            "<CardID>"+cardId+"</CardID> " +
                            "<Articul>"+ serviceid + "</Articul>" +
                            "<Account>"+toNumberid+" </Account>" +
                            "<Amount>"+amount*100+"</Amount>" +
                            "<Currency>972</Currency> " +
                            "<Description>Описаниетовара</Description> " +
                            "<Phone>992"+msisdn+"</Phone> " +
                            "<SenderName>Babilon</SenderName> " +
                            "<Sign>"+sign+"</Sign>" +
                            "</Order> " +
                            "</Request> " +
                            "</TKKPG>";

            return xm;
        }
        public string CreateXmlBalance( string cardId, string sign)
        {
            string xm = @"<Request>
            <Operation>CheckBalance</Operation>
             <Order>
             <Merchant>100101</Merchant>
             <CardID>"+cardId+"</CardID>" +
             "<Sign>"+sign+"</Sign> " +
             "</Order> </Request>";

            return xm;
        }
    }

}
