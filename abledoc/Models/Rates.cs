using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class Rates
    {
        public double ConvertGOOG(decimal amount, string toCurrency, ConversionRate conversion_rates)
        {

            try
            {

                double rate1 = 0;
                if ("AED" == toCurrency)
                {
                    rate1 = conversion_rates.AED;
                }
                else if ("ARS" == toCurrency)
                {
                    rate1 = conversion_rates.ARS;
                }
                else if ("AUD" == toCurrency)
                {
                    rate1 = conversion_rates.AUD;
                }
                else if ("BGN" == toCurrency)
                {
                    rate1 = conversion_rates.BGN;
                }
                else if ("BRL" == toCurrency)
                {
                    rate1 = conversion_rates.BRL;
                }
                else if ("BSD" == toCurrency)
                {
                    rate1 = conversion_rates.BSD;
                }
                else if ("CAD" == toCurrency)
                {
                    rate1 = conversion_rates.CAD;
                }
                else if ("CHF" == toCurrency)
                {
                    rate1 = conversion_rates.CHF;
                }
                else if ("CLP" == toCurrency)
                {
                    rate1 = conversion_rates.CLP;
                }
                else if ("CNY" == toCurrency)
                {
                    rate1 = conversion_rates.CNY;
                }
                else if ("COP" == toCurrency)
                {
                    rate1 = conversion_rates.COP;
                }
                else if ("CZK" == toCurrency)
                {
                    rate1 = conversion_rates.CZK;
                }
                else if ("DKK" == toCurrency)
                {
                    rate1 = conversion_rates.DKK;
                }
                else if ("DOP" == toCurrency)
                {
                    rate1 = conversion_rates.DOP;
                }
                else if ("EGP" == toCurrency)
                {
                    rate1 = conversion_rates.EGP;
                }
                else if ("EUR" == toCurrency)
                {
                    rate1 = conversion_rates.EUR;
                }
                else if ("FJD" == toCurrency)
                {
                    rate1 = conversion_rates.FJD;
                }
                else if ("GBP" == toCurrency)
                {
                    rate1 = conversion_rates.GBP;
                }
                else if ("GTQ" == toCurrency)
                {
                    rate1 = conversion_rates.GTQ;
                }
                else if ("HKD" == toCurrency)
                {
                    rate1 = conversion_rates.HKD;
                }
                else if ("HRK" == toCurrency)
                {
                    rate1 = conversion_rates.HRK;
                }
                else if ("HUF" == toCurrency)
                {
                    rate1 = conversion_rates.HUF;
                }
                else if ("IDR" == toCurrency)
                {
                    rate1 = conversion_rates.IDR;
                }
                else if ("ILS" == toCurrency)
                {
                    rate1 = conversion_rates.ILS;
                }
                else if ("INR" == toCurrency)
                {
                    rate1 = conversion_rates.INR;
                }
                else if ("ISK" == toCurrency)
                {
                    rate1 = conversion_rates.ISK;
                }
                else if ("JPY" == toCurrency)
                {
                    rate1 = conversion_rates.JPY;
                }
                else if ("KRW" == toCurrency)
                {
                    rate1 = conversion_rates.KRW;
                }
                else if ("KZT" == toCurrency)
                {
                    rate1 = conversion_rates.KZT;
                }
                else if ("MXN" == toCurrency)
                {
                    rate1 = conversion_rates.MXN;
                }
                else if ("MYR" == toCurrency)
                {
                    rate1 = conversion_rates.MYR;
                }
                else if ("NOK" == toCurrency)
                {
                    rate1 = conversion_rates.NOK;
                }
                else if ("NZD" == toCurrency)
                {
                    rate1 = conversion_rates.NZD;
                }
                else if ("PAB" == toCurrency)
                {
                    rate1 = conversion_rates.PAB;
                }
                else if ("PEN" == toCurrency)
                {
                    rate1 = conversion_rates.PEN;
                }
                else if ("PHP" == toCurrency)
                {
                    rate1 = conversion_rates.PHP;
                }
                else if ("PKR" == toCurrency)
                {
                    rate1 = conversion_rates.PKR;
                }
                else if ("PLN" == toCurrency)
                {
                    rate1 = conversion_rates.PLN;
                }
                else if ("PYG" == toCurrency)
                {
                    rate1 = conversion_rates.PYG;
                }
                else if ("RON" == toCurrency)
                {
                    rate1 = conversion_rates.RON;
                }
                else if ("RUB" == toCurrency)
                {
                    rate1 = conversion_rates.RUB;
                }
                else if ("SAR" == toCurrency)
                {
                    rate1 = conversion_rates.SAR;
                }
                else if ("SEK" == toCurrency)
                {
                    rate1 = conversion_rates.SEK;
                }
                else if ("SGD" == toCurrency)
                {
                    rate1 = conversion_rates.SGD;
                }
                else if ("THB" == toCurrency)
                {
                    rate1 = conversion_rates.THB;
                }
                else if ("TRY" == toCurrency)
                {
                    rate1 = conversion_rates.TRY;
                }
                else if ("TWD" == toCurrency)
                {
                    rate1 = conversion_rates.TWD;
                }
                else if ("UAH" == toCurrency)
                {
                    rate1 = conversion_rates.UAH;
                }
                else if ("USD" == toCurrency)
                {
                    rate1 = conversion_rates.USD;
                }
                else if ("NZD" == toCurrency)
                {
                    rate1 = conversion_rates.NZD;
                }
                else if ("UYU" == toCurrency)
                {
                    rate1 = conversion_rates.UYU;
                }
                else if ("ZAR" == toCurrency)
                {
                    rate1 = conversion_rates.ZAR;
                }

                rate1 = rate1 * double.Parse(amount.ToString());
                return rate1;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }
    }
    public class API_Obj
    {
        public string result { get; set; }
        public string documentation { get; set; }
        public string terms_of_use { get; set; }
        public string time_zone { get; set; }
        public string time_last_update { get; set; }
        public string time_next_update { get; set; }
        public ConversionRate conversion_rates { get; set; }
    }

    public class ConversionRate
    {
        public double AED { get; set; }
        public double ARS { get; set; }
        public double AUD { get; set; }
        public double BGN { get; set; }
        public double BRL { get; set; }
        public double BSD { get; set; }
        public double CAD { get; set; }
        public double CHF { get; set; }
        public double CLP { get; set; }
        public double CNY { get; set; }
        public double COP { get; set; }
        public double CZK { get; set; }
        public double DKK { get; set; }
        public double DOP { get; set; }
        public double EGP { get; set; }
        public double EUR { get; set; }
        public double FJD { get; set; }
        public double GBP { get; set; }
        public double GTQ { get; set; }
        public double HKD { get; set; }
        public double HRK { get; set; }
        public double HUF { get; set; }
        public double IDR { get; set; }
        public double ILS { get; set; }
        public double INR { get; set; }
        public double ISK { get; set; }
        public double JPY { get; set; }
        public double KRW { get; set; }
        public double KZT { get; set; }
        public double MXN { get; set; }
        public double MYR { get; set; }
        public double NOK { get; set; }
        public double NZD { get; set; }
        public double PAB { get; set; }
        public double PEN { get; set; }
        public double PHP { get; set; }
        public double PKR { get; set; }
        public double PLN { get; set; }
        public double PYG { get; set; }
        public double RON { get; set; }
        public double RUB { get; set; }
        public double SAR { get; set; }
        public double SEK { get; set; }
        public double SGD { get; set; }
        public double THB { get; set; }
        public double TRY { get; set; }
        public double TWD { get; set; }
        public double UAH { get; set; }
        public double USD { get; set; }
        public double UYU { get; set; }
        public double ZAR { get; set; }
    }
}
