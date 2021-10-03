using System;
using System.Globalization;
using System.Numerics;

namespace TransactionSearch.Core.Extensions
{
    public class EthereumHexParser
    {
        public long ParseAmountValue(string hexNumber)
        {
            try
            {
                if (hexNumber == "0x0")
                    return 0;

                var hex = hexNumber.Replace("0x", "");
                const int pow = 18;
                var bign = Convert.ToDouble(BigInteger.Parse(hex, NumberStyles.AllowHexSpecifier).ToString());

                if (!double.TryParse(Convert.ToInt64(hexNumber, 16).ToString(), out var result))
                    return 0;

                //Ether	1,000,000,000,000,000,000	 10^18
                //Gas	1,000,000,000,000,000,000	 10^9
                result /= Math.Pow(10, pow);

                return (long)result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public long ParseGasValue(string hexNumber)
        {
            try
            {
                if (hexNumber == "0x0")
                    return 0;

                if (!double.TryParse(Convert.ToInt64(hexNumber, 16).ToString(), out var result))
                    return 0;

                //Gas	1,000,000,000,000,000,000	 10^9
                result /= Math.Pow(10, 9);

                return (long)result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public long ParseBlockNumber(string hexNumber, int pow = 0)
        {
            try
            {
                return hexNumber == "0x0" ? 0 : Convert.ToInt64(hexNumber, 16);
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
