namespace Payment_ZainCash
{

    /// <summary>
    ///  This class is used to make payment using ZainCash
    ///  developer : Ammar Rashad
    ///  year : 2023
    ///  for support : 009647830200030
    ///  linkedin : https://www.linkedin.com/in/ammar-alasafer-b2933415a/
    /// </summary>
    public class ZainPayment
    {
        public string GenerateZaincashUrl(
            string? orderid,
            float amount,
            bool isdollar,
            int dollar_exchange_rate,
            string redirectionUrl,
            string msisdn,
            string serviceType,
            string secret,
            string merchantid,
            string lang,
            bool developemntMode)
        {
            //Change currency to dollar if required
            int new_amount;
            if (isdollar) { new_amount = (int)(amount * dollar_exchange_rate); } else { new_amount = (int)amount; }

            //Setting expiration of token
            Int32 iat = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Int32 exp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + 60 * 60 * 4;


            Random random = new Random();
            //Generate the data array
            IDictionary<string, object> dataarray = new Dictionary<string, object>();
            dataarray.Add("amount", new_amount);
            dataarray.Add("serviceType", serviceType);
            dataarray.Add("msisdn", msisdn);
            dataarray.Add("orderId", orderid == null ? random.Next(100000, 999999).ToString() : orderid);
            dataarray.Add("redirectUrl", redirectionUrl);
            dataarray.Add("iat", iat);
            dataarray.Add("exp", exp);


            //Generating token
            string token = Jose.JWT.Encode(dataarray, System.Text.Encoding.ASCII.GetBytes(secret), Jose.JwsAlgorithm.HS256);


            //Posting token to ZainCash API to generate Transaction ID
            var httpclient = new System.Net.WebClient();
            var data_to_post = new System.Collections.Specialized.NameValueCollection();
            data_to_post["token"] = token;
            data_to_post["merchantId"] = merchantid;
            data_to_post["lang"] = lang;

            string response = System.Text.Encoding.ASCII.GetString(httpclient.UploadValues("https://test.zaincash.iq/transaction/init", "POST", data_to_post));

            //Parse JSON response to Object
            var jsona = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
            if (developemntMode)
            {
                //Return final URL
                return "https://test.zaincash.iq/transaction/pay?id=" + (string)jsona.id;
            }
            //Return final URL
            return "https://api.zaincash.iq/transaction/pay?id=" + (string)jsona.id;

        }

        public Dictionary<string, string> AfterRedirection(string token, string secret)
        {
            //Convert token to json, then to object
            var jsona_res = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Jose.JWT.Decode(token, System.Text.Encoding.ASCII.GetBytes(secret)));

            //Generating response array
            Dictionary<string, string> final = new Dictionary<string, string>();
            final.Add("status", (string)jsona_res.status);
            if (jsona_res.status == "failed") { final.Add("msg", (string)jsona_res.msg); }

            return final;
        }

    }
}