using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for AmazonAPIRequest
/// </summary>

namespace AmazonProductAPI
{
    public class AmazonAPIRequest
    {
        public static String AWS_ACCESS_KEY_ID = ConfigurationManager.AppSettings["AWS_ACCESS_KEY_ID"];
        public static String AWS_SECRET_KEY    = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];
        private static String ENDPOINT         = ConfigurationManager.AppSettings["ENDPOINT"];
        public static String ASSOCIATE_TAG     = ConfigurationManager.AppSettings["ASSOCIATE_TAG"];
        public static String NAMESPACE         = ConfigurationManager.AppSettings["NAMESPACE"];

        public static XmlDocument Request(int returnSheet, String keyword)
        {
            AmazonRequestHelper helper = new AmazonRequestHelper(AWS_ACCESS_KEY_ID, AWS_SECRET_KEY, ENDPOINT);

            String requestUrl;
            XmlDocument response;

            IDictionary<String, String> request = new Dictionary<String, String>();
            request["Service"] = "AWSECommerceService";
            request["Operation"] = "ItemSearch";
            request["AWSAccessKeyId"] = AWS_ACCESS_KEY_ID;
            request["AssociateTag"] = ASSOCIATE_TAG;
            request["SearchIndex"] = "All";
            request["Condition"] = "New";
            request["MerchantId"] = "Amazon";
            request["Keywords"] = keyword;
            request["ItemPage"] = returnSheet.ToString();
            request["ResponseGroup"] = "ItemAttributes,OfferSummary,Images";

            requestUrl = helper.Sign(request);
            response = FetchData(requestUrl);

            return response;
        }

        // Executes the webrequest using the already signed url.
        public static XmlDocument FetchData(String url)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument docx = new XmlDocument();
                docx.Load(response.GetResponseStream());
                XmlNodeList errorNodes = docx.GetElementsByTagName("Message", NAMESPACE);
                // Display error in case signing works, but error was in response.
                if (errorNodes != null && errorNodes.Count > 0)
                {
                    XmlDocument message = docx;
                    return message;
                }

                return docx;
            } catch(Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
            }

            return null;
        }
  }
}