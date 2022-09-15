using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DCityBankService
{
    
    public class Functions
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(Functions));
        public void CheckTransaction(string TransactionId, int Counter)
        {
            while (Counter < 3)
            { _log.Debug("sdsd - " + DateTime.Now);
                Thread.Sleep(30 * 1000);
                _log.Debug("sdsd1 - " + DateTime.Now);
                Counter = Counter + 1;
                _log.Debug("Counter" + Counter);
                BankRequests Bank = new BankRequests();
                DBConnection DB = new DBConnection();
                PaymentCheckstatusAnswer payment = new PaymentCheckstatusAnswer();
                //string convertedJson = new Functions().CreateCheckBody(int.Parse(TransactionId.ToString()));
                //var paymentAnswer2 = new Functions().GetSigns(convertedJson);
                long trid = Convert.ToInt64(TransactionId);
                PaymentCheckstatusAnswer Check = Bank.GetStatus(trid);
                _log.Debug("Stat "+Check);
                if (Check != null)
                {
                    if (Check.Status == 1)
                    {
                        _log.Debug("sdsd");
                        DB.DBSUpdateTransactionSucceed(TransactionId.ToString(), "2");
                        payment.isError = false;
                        payment.Status = 1;

                    }
                    else
                    if (Check.Status == -90 )
                    {
                        _log.Debug("sss");
                        //Thread thread2 = new Thread(() => DB.DBSUpdateTransactionSucceed(TransactionId.ToString(), "3"));
                        //thread2.Start();
                    }
                    else
                    if (Check.Status == -98)
                    {
                        _log.Debug("Ошибка 500 со стороны сервера или нехватка средств");
                    }
                    else
                    {
                        _log.Debug("aaaa");
                        Thread thread2 = new Thread(() => DB.DBSUpdateTransactionSucceed(TransactionId.ToString(), "3"));
                        thread2.Start();
                    }

                }
                else
                {
                    CheckTransaction(TransactionId, Counter);
                }
            }
          
        }
        public void CheckTransaction2(string TransactionId, int Counter)
        {
            while (Counter < 3)
            {
                _log.Debug("sdsd - " + DateTime.Now);
                Thread.Sleep(30 * 1000);
                _log.Debug("sdsd1 - " + DateTime.Now);
                Counter = Counter + 1;
                _log.Debug("Counter" + Counter);
                BankRequests Bank = new BankRequests();
                DBConnection DB = new DBConnection();
                PaymentCheckstatusAnswer payment = new PaymentCheckstatusAnswer();
                //string convertedJson = new Functions().CreateCheckBody(int.Parse(TransactionId.ToString()));
                //var paymentAnswer2 = new Functions().GetSigns(convertedJson);
                long trid = Convert.ToInt64(TransactionId);
                PaymentCheckstatusAnswer Check = Bank.GetStatus(trid);
                _log.Debug("Stat " + Check);
                if (Check != null)
                {
                    if (Check.Status == 1)
                    {
                        _log.Debug("sdsd");
                        DB.DBSUpdateTransactionSucceed(TransactionId.ToString(), "2");
                        payment.isError = false;
                        payment.Status = 1;
                    }
                    else
                    if (Check.Status == -90)
                    {
                        _log.Debug("sss");
                        Thread thread2 = new Thread(() => DB.DBSUpdateTransactionSucceed(TransactionId.ToString(), "3"));
                        thread2.Start();
                    }
                    else
                    {
                        _log.Debug("aaaa");
                        Thread thread2 = new Thread(() => DB.DBSUpdateTransactionSucceed(TransactionId.ToString(), "3"));
                        thread2.Start();
                    }

                }
                else
                {
                    CheckTransaction(TransactionId, Counter);
                }
            }

        }

    }
}
