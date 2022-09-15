using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace DCityBankService
{
    public class AppModels
    {
    }
    public class good
    {
        public string name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
    }
    public class OtherServicesData
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal Count { get; set; }
        public decimal Sum { get; set; }
    }
    public class MVCoCoupon
    {
        public int ReceivedCouponId { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        public string About { get; set; }
        public string AboutTj { get; set; }
        public string NameTj { get; set; }


    }
   
    public class Merchant
    {
        public int Code { get; set; }
        public string Id { get; set; }
        public string MerchantName { get; set; }
        public bool Status { get; set; }
        public decimal ServiceMaxAmount { get; set; }
        public decimal ServiceMinAmount { get; set; }
        public string Comment { get; set; }
        public string ServiceName { get; set; }
        public int CategoriesId { get; set; }
        public int MerchantId { get; set; }
        public int ServiceId { get; set; }
        public long PartnerId { get; set; }
        public string PartnerName { get; set; }
        public List<good> goods { get; set; }
        public List<OtherServicesData> OtherServices { get; set; }
        public string PaymentId { get; set; }
        public int QRCodeType = -1;
        public string amount { get; set; }
        public bool IsBank { get; set; }
        public string BankId { get; set; }
        public MVCoCoupon Coupon { get; set; }
        public bool PartialPay { get; set; }
        public decimal RemainAmount { get; set; }
        public bool HasCoupon = false;
        public string CouponOutletName { get; set; }
        public List<Coupon> UserCoupons { get; set; }
        public string EMVCoTransactionId { get; set; }
        public bool IsEcommerceQR = false;
    }
    public class Coupon
    {
        public int code { get; set; }
        public int MerchantId { get; set; }
        public int Id { get; set; }
        public string Discount { get; set; }
        public int CampaignTypeId { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public int ReceiveDayType { get; set; }
        public DateTime StartReceiveDate { get; set; }
        public DateTime EndReceiveDate { get; set; }
        public string EndReceiveDateICouponPlus { get; set; }
        public string EndReceiveDateICouponPlusTj { get; set; }
        public string ActivateDayType { get; set; }
        public DateTime StartActivateDate { get; set; }
        public DateTime EndActivateDate { get; set; }
        public string EndActivateDateICouponPlus { get; set; }
        public string EndActivateDateICouponPlusTj { get; set; }
        public int Quantity { get; set; }
        public int MaxCouponForPerson { get; set; }
        public int IncomeFrom { get; set; }
        public int IncomeTo { get; set; }
        public int Roaming { get; set; }
        public int CarOwner { get; set; }
        public string LanguageSimId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Status { get; set; }
        public string Icon { get; set; }
        public string LogoImg { get; set; }
       // public TimeT RemainTime { get; set; }
        public int RemainQuantity { get; set; }
        //public List<CampaignsImages> CampaignsImages { get; set; }
        public bool CanBuy { get; set; }
        public string MerchantRate { get; set; }
        public string MerchantName { get; set; }
        public string MerchantNameTj { get; set; }
        public string GoogleRait { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string CouponType { get; set; }
        public int ReviewCount { get; set; }
        //public List<Rates> RateCounts { get; set; }
        //public List<RecieveWeekDay> ActiveDateList { get; set; }
        public string ReceivedCouponsNumber { get; set; }
        public int ReceivedCouponsId { get; set; }
        public int ContentServiceId { get; set; }
        public string NameTj { get; set; }
        public string AboutTj { get; set; }
        public string AddressTajik { get; set; }
        public int IsICouponPlus { get; set; }
        public decimal ICouponPlusPrice { get; set; }
        public decimal ICouponPlusOldPrice { get; set; }
        public int ICouponPlusType { get; set; }
        public string ICouponPlusProductColors { get; set; }
        //public List<ColorsModel> ICouponPlusProductColorsDetails { get; set; }
        public string ICouponPlusProductSizes { get; set; }
        //public List<ColorsModel> ICouponPlusProductSizesDetails { get; set; }
        //public List<CouponFOrMain> OtherICouponsPlus { get; set; }
        public int ICouponPlusNeedDelivery { get; set; }
        public string OutletGpsLat { get; set; }
        public string OutletGpsLng { get; set; }
        public DateTime DateUse { get; set; }
        public string CouponNumber { get; set; }
        //public List<Rates> ICouponRatingCount { get; set; }
        //public List<OutletsICouponPlusReviewsModel> OutletsICouponPlusReviews = new List<OutletsICouponPlusReviewsModel>();
        public string OutletsICouponPlusRating { get; set; }
        public string ChoosedICouponPlusColor { get; set; }
        public string ChoosedICouponPlusSize { get; set; }
        public string WebLink { get; set; }
        //public List<SizeColorModel> SizeColors { get; set; }
        public int MaxCouponForPersonRemainQuantity { get; set; }
    }
    public class PaymentAnswer
	{
		public string PaymentID { get; set; }
		public bool isError { get; set; }
		public int Code { get; set; }
		public string Comment { get; set; }
         
	}
    public class DcityTerminateWalletAnswer
    {
        public int code { get; set; }
        public string Comment { get; set; }
    }
    public class CreateUser
    {
        public string PaymentID { get; set; }
        public bool isError { get; set; }
        public int Code { get; set; }
        public string Comment { get; set; }
        public BankUserInfo User { get; set; }
    }
    public class BankUserInfo
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string middlename { get; set; }
        public string number { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public bool status { get; set; }
        public string balance { get; set; }
        public string clientCode { get; set; }
        public string msisdn { get; set; }
        public bool identified { get; set; }
    }
    public class result
    {
        public int code { get; set; }
        public string Comment { get; set; }
        public bool IsError { get; set; }
    }
    public class PaymentCheckstatusAnswer
    {
        public string PaymentID { get; set; }
        public bool isError { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }

    }
    public class CancelTransaction
	{
		public string PaymentID { get; set; }
		public bool isError { get; set; }
		public int Status { get; set; }
		public string Comment { get; set; }
	}
	public class PaymentCheckAnswer
	{
		public string PaymentID { get; set; }
		public bool isError { get; set; }
		public int Status { get; set; }
		public string Comment { get; set; }
	}
    public class CurrencyRate
    {
        public string Rate { get; set; }
        public DateTime date { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
    }
    public class PartnerBalanc
	{
		public bool isError { get; set; }
		public string Balance { get; set; }
		public string Limit { get; set; }
		public int Status { get; set; }
		public string Comment { get; set; }
	}
	public class AvailibleProviders
	{
		public bool isError { get; set; }
		public string Balance { get; set; }
		public string Limit { get; set; }
		public int Status { get; set; }
		public string Comment { get; set; }
	}
    public class Userinfo
    {
        public bool isError { get; set; }
        public string Balance { get; set; }
        public string Info { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
    }
    public class PayAnswer
    {
		public int Code { get; set; }
		public string Comment{ get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }
    }

    public class Provider
    {
        public string articul { get; set; }
        public string typeEnter { get; set; }
        public string typekb { get; set; }
        public string prefix { get; set; }
        public string name { get; set; }
        public string digits { get; set; }
        public string info { get; set; }
        public string phone_for_sms { get; set; }
        public string phone_for_sms_prefix { get; set; }
        public string checkznak { get; set; }
        public string category { get; set; }
        public string maxsum { get; set; }
        public string minsum { get; set; }
    }

    public class Providers
    {
        public List<Provider> provider { get; set; }
    }

    public class Response
    {
        public string result { get; set; }
        public string action { get; set; }
        public string date { get; set; }
        public string comment { get; set; }
        public Providers providers { get; set; }
    }

    public class Provids
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }
        public Response response { get; set; }
    }
    public class DCityAnswer
    {
        public string Status { get; set; }
        public string Description { get; set; }
    }
    public class CheckIden
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
        public string Sign { get; set; }
        public string Phone { get; set; }
    }
    public class PartnerBalance
    {
        public int code { get; set; }
        public string Balance { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
        public string Number { get; set; }
        public string clientCode { get; set; }
        public bool Identified { get; set; }
        public string PartnerName { get; set; }
    }
    public class CardAttachAnswer
    {
        public int Status { get; set; }
        public string Comment { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public bool NeedCallBack { get; set; }
        public int PartnerId { get; set; }
        public string Orderid { get; set; }
    }

}
