using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Ds3App2.Utilities
{
    public static class WebConfigurationSettings
    {
        public static string PayGateId
        {
            get
            {
                return ConfigurationManager.AppSettings["PAYGATEID"];
            }
        }
        public static string PayGateKey
        {
            get
            {
                return ConfigurationManager.AppSettings["PAYGATEKEY"];
            }
        }
        public static decimal VAT
        {
            get
            {
                return decimal.Parse(ConfigurationManager.AppSettings["VAT"].Replace('.',',').ToString());
            }
        }
        public static string LowStockEmailTo
        {
            get
            {
                return ConfigurationManager.AppSettings["LowStockEmailTo"];
            }
        }
    }
}