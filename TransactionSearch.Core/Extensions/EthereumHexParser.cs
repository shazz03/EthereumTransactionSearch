using System;
using System.Globalization;
using System.Numerics;

namespace TransactionSearch.Core.Extensions
{
    public class EthereumHexParser
    {
        public decimal ParseAmountValue(string hexNumber)
        {
            return ParseValue(hexNumber, 18);
        }

        public decimal ParseGasValue(string hexNumber)
        {
            return ParseValue(hexNumber, 18);
        }

        private static decimal ParseValue(string hexNumber, int pow)
        {
            if (string.IsNullOrWhiteSpace(hexNumber) || hexNumber == "0x0")
                return 0;


            var hexResult = BigInteger.Parse($"0{hexNumber.Replace("0x", "")}", NumberStyles.HexNumber);

            //Ether	1,000,000,000,000,000,000	 10^18
            var result = decimal.Parse((double.Parse(hexResult.ToString()) / double.Parse(BigInteger.Pow(10, pow).ToString())).ToString(), NumberStyles.Float);
            return result;
        }

        public double ParseBlockNumber(string hexNumber)
        {
            if (string.IsNullOrWhiteSpace(hexNumber) || hexNumber == "0x0")
                return 0;

            return Convert.ToInt64(hexNumber, 16);
        }

    }
}
