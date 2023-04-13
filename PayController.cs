using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
 

namespace DCityBankService
{
   public class PaymentController : ApiController
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(PaymentController));
        public PaymentAnswer Get(string msisdn, string amount, string serviceID, string toNumberID, string region, long TransactionId, string ProviderID, string UserNumber, string AdditionalField = "-1")
        {
            PaymentAnswer payAnswer = new PaymentAnswer();
            Functions functions = new Functions();
            BankRequests requests = new BankRequests();
            payAnswer = requests.CreateTransaction(msisdn, amount, serviceID, toNumberID, region, TransactionId, ProviderID, UserNumber);
            Thread t = new Thread(() => functions.CheckTransaction(TransactionId.ToString(), 0));
            t.Start();

            return payAnswer;
        }
    }
    public class MerchantPayController : ApiController
    {
        
        public PaymentAnswer Get(string amount, string serviceID, string transuctionID, string Number, string PartnerId, string QRCode, string msisdn)
        {
            PaymentAnswer payAnswer = new PaymentAnswer();
            Functions functions = new Functions();
            BankRequests requests = new BankRequests();
            payAnswer = requests.CreateTransaction(msisdn, amount, serviceID, Number, msisdn, long.Parse(transuctionID), serviceID, msisdn);
            Thread t = new Thread(() => functions.CheckTransaction(transuctionID, 0));
            t.Start();

            return payAnswer;
        }
    }
    public class CheckMerchantController : ApiController
    {

        public Merchant Get(string id)
        {
            Merchant merchantAnswer  = new Merchant();

            merchantAnswer.Code = 1;
            merchantAnswer.Id = id.ToString();
            merchantAnswer.BankId = "11";
            merchantAnswer.MerchantName = "Dcity";
            merchantAnswer.ServiceMaxAmount = 4300;
            merchantAnswer.ServiceMinAmount = 1;
            merchantAnswer.ServiceId = 2417;
            merchantAnswer.ServiceName = "QrDcity";

            return merchantAnswer;
        }
    }

    public class CreateUserController : ApiController
    {
        public CreateUser Get(string phone, string name, string surname, string middlename, string userid)
        {
            CreateUser payAnswer = new CreateUser();

            BankRequests requests = new BankRequests();
            payAnswer = requests.CreateUser(phone,userid,name,surname,middlename);
            Functions functions = new Functions();
            //if (payAnswer.Code == -90)
            //{
            //    Thread.Sleep(6 * 1000);
            //    payAnswer = requests.CreateUser(phone, userid, name, surname, middlename);
            //    //Thread tt = new Thread(() => functions.CheckTransaction2(userid,0 ));
            //    //tt.Start();

            //}
            return payAnswer;
        }
    }
    public class RemoveUserController : ApiController
    {
        public DcityTerminateWalletAnswer Get(string phone, string name, string surname, string middlename, string userid)
        {
            DcityTerminateWalletAnswer payAnswer = new DcityTerminateWalletAnswer();
            CreateUser create = new CreateUser();
            BankRequests requests = new BankRequests();
            create = requests.RemoveUser(phone, userid, name, surname, middlename);

            if (create.isError == false)
            {
                payAnswer.code = 1;
                payAnswer.Comment = "ok";
            }
            else
            {
                payAnswer.code = -1;
                payAnswer.Comment = "error";
            }
           
            return payAnswer;
        }
    }
    public class BalanceController : ApiController
    {
        public PartnerBalance Get(string clientCode, string msisdn)
        {
            
            PartnerBalance payAnswer = new PartnerBalance();
            BankRequests requests = new BankRequests();
            //payAnswer = requests.GetBalance(msisdn, clientCode);.

            //Заглушка для пропуска проверки баланса кошелька
            payAnswer.Balance = "0";
            payAnswer.Identified = requests.CheckifIdentifiedWalletDcity(msisdn);
            payAnswer.Status = true;
            payAnswer.Type = "wallet";
            payAnswer.Number = "992" + msisdn;
            payAnswer.clientCode = "992" + msisdn;
            payAnswer.Currency = "TJS";
            payAnswer.code = 1;

            return payAnswer;
        }
    }
    public class CancelTransactionController : ApiController
    {
        public CancelTransaction Get(string TransactionId,string NewAccount=null)
        {
            CancelTransaction cancelTransaction = new CancelTransaction();
            BankRequests requests = new BankRequests();
            cancelTransaction = requests.CancelTransactions(TransactionId,NewAccount);
            return cancelTransaction;
        }
    }
    public class CheckAccountController : ApiController
    {
        public PaymentCheckAnswer Get(string msisdn, string amount, string serviceID, string toNumberID, string region, long TransactionId, string ProviderID, string UserNumber)
        {
            PaymentCheckAnswer payAnswer = new PaymentCheckAnswer();

            BankRequests requests = new BankRequests();
            payAnswer = requests.CheckAccount(msisdn, amount, serviceID, toNumberID, region, TransactionId, ProviderID, UserNumber);

            return payAnswer;
        }
    }
    public class DealerBalanceController : ApiController
    {
        public PartnerBalanc Get(string msisdn)
        {
            PartnerBalanc payAnswer = new PartnerBalanc();

            BankRequests requests = new BankRequests();
            payAnswer = requests.Balance(msisdn);

            return payAnswer;
        }
    }
    public class ProviderController : ApiController
    {
        public AvailibleProviders Get(string prvid)
        {
            AvailibleProviders payAnswer = new AvailibleProviders();

            BankRequests requests = new BankRequests();
            payAnswer = requests.AProviders(prvid);

            return payAnswer;
        }
    }
    public class UserInfoController : ApiController
    {
        public Userinfo Get(string msisdn,string serviceid)
        {
            Userinfo payAnswer = new Userinfo();

            BankRequests requests = new BankRequests();
            payAnswer = requests.GetInfo(msisdn,serviceid);

            return payAnswer;
        }
    }
    //
    public class PaymentCheckTransactionStatusController : ApiController
    {
        public PaymentCheckstatusAnswer Get(long TransactionId)
        {
            PaymentCheckstatusAnswer payAnswer = new PaymentCheckstatusAnswer();

            BankRequests requests = new BankRequests();
            payAnswer = requests.GetStatus(TransactionId);

            return payAnswer;
        }
    }
    public class ExchangeRateController : ApiController
    {
        public CurrencyRate Get(string amount,string currency)
        {
            CurrencyRate payAnswer = new CurrencyRate();

            BankRequests requests = new BankRequests();
            payAnswer = requests.GetCurrencyRate(amount,currency);

            return payAnswer;
        }
    }
    public class CardAttachController : ApiController
    {
        public CardAttachAnswer Get(string orderid, string Pan, string Expdate, string cvv, string holderName, string phone)
        {
            CardAttachAnswer payAnswer = new CardAttachAnswer();

            BankRequests requests = new BankRequests();
            payAnswer = requests.CardAttach(orderid, Pan,Expdate,cvv,holderName,phone);

            return payAnswer;
        }
    }
    public class CardPayController : ApiController
    {
        public CardAttachAnswer Get(string orderid, string cardId, string serviceid, string toNumberid, decimal amount, string msisdn)
        {
            CardAttachAnswer payAnswer = new CardAttachAnswer();

            BankRequests requests = new BankRequests();
            payAnswer = requests.CardPay(orderid, cardId, serviceid, toNumberid, amount, msisdn);

            return payAnswer;
        }
    }
}
