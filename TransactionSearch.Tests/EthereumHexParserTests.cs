using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionSearch.Core.Extensions;

namespace TransactionSearch.Tests
{
    [TestClass]
    public class EthereumHexParserTests
    {
        private readonly EthereumHexParser _parser;

        public EthereumHexParserTests()
        {
            _parser = new EthereumHexParser();
        }

        [TestMethod]
        public void ParseAmountValue_Null_Hex_Value_Returns_Zero()
        {
            var result = _parser.ParseAmountValue(null);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseAmountValue_Empty_Hex_Value_Returns_Zero()
        {
            var result = _parser.ParseAmountValue("");
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void ParseAmountValue_Zero_Hex_Returns_Zero()
        {
            var result = _parser.ParseAmountValue("0x0");
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseGasValue_Null_Hex_Value_Returns_Zero()
        {
            var result = _parser.ParseGasValue(null);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseGasValue_Empty_Hex_Value_Returns_Zero()
        {
            var result = _parser.ParseGasValue("");
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void ParseGasValue_Zero_Hex_Returns_Zero()
        {
            var result = _parser.ParseGasValue("0x0");
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseBlockNumber_Null_Hex_Value_Returns_Zero()
        {
            var result = _parser.ParseBlockNumber(null);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseBlockNumber_Empty_Hex_Value_Returns_Zero()
        {
            var result = _parser.ParseBlockNumber("");
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void ParseBlockNumber_Zero_Hex_Returns_Zero()
        {
            var result = _parser.ParseBlockNumber("0x0");
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ParseBlockNumber_Valid_Hex_Returns_Value()
        {
            var result = _parser.ParseBlockNumber("0xcba3b0");
            Assert.AreEqual(13345712, result);
        }

        [TestMethod]
        public void ParseGasValue_Valid_Hex_Returns_Value()
        {
            var result = _parser.ParseGasValue("0x1e8480");
            Assert.AreEqual(0.000000000002m, result);
        }

        [TestMethod]
        public void ParseAmountValue_Valid_Hex_Returns_Value()
        {
            var result = _parser.ParseAmountValue("0x2c68af0bb140000");

            Assert.AreEqual(0.2m, result);
        }
    }
}
