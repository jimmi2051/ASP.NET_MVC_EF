using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
namespace Infrastructure
{
    public class Information
    {
        public static string GoogleID
        {
            get { return ConfigurationManager.AppSettings["GoogleID"].ToString(); }
        }
        public static string GoogleSecret
        {
            get { return ConfigurationManager.AppSettings["GoogleSecret"].ToString(); }
        }
        public static string FacebookID
        {
            get { return ConfigurationManager.AppSettings["FacebookID"].ToString(); }
        }
        public static string FacebookSecret
        {
            get { return ConfigurationManager.AppSettings["FacebookSecret"].ToString(); }
        }
        public static string GoogleLoginID
        {
            get { return ConfigurationManager.AppSettings["GoogleLoginID"].ToString(); }
        }
        public static string GoogleLoginSecret
        {
            get { return ConfigurationManager.AppSettings["GoogleLoginSecret"].ToString(); }
        }
        public static string CommonConstantUsers { get { return "UserSessionLogin"; } }
        public static string CommonConstantAdmin { get { return "AdminSessionLogin"; } }
        public static string CommonConstantCard { get { return "CartItem"; } }
    }
    [Serializable]
    public class ItemToPayment
    {
        public string Product_ID { get; set; }
        public int quality { get; set; }
        public float priceSell { get; set; }
    }
}
