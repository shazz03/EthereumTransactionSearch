﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TransactionSearch.Core.Models
{
    public class AccessList
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("storageKeys")]
        public List<string> StorageKeys { get; set; }
    }

    public class Transaction
    {
        [JsonPropertyName("blockHash")]
        public string BlockHash { get; set; }

        [JsonPropertyName("blockNumber")]
        public string BlockNumber { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("gas")]
        public string Gas { get; set; }

        [JsonPropertyName("gasPrice")]
        public string GasPrice { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("input")]
        public string Input { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        [JsonPropertyName("r")]
        public string R { get; set; }

        [JsonPropertyName("s")]
        public string S { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("transactionIndex")]
        public string TransactionIndex { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("v")]
        public string V { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("accessList")]
        public List<AccessList> AccessList { get; set; }

        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }

        [JsonPropertyName("maxFeePerGas")]
        public string MaxFeePerGas { get; set; }

        [JsonPropertyName("maxPriorityFeePerGas")]
        public string MaxPriorityFeePerGas { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("baseFeePerGas")]
        public string BaseFeePerGas { get; set; }

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }

        [JsonPropertyName("extraData")]
        public string ExtraData { get; set; }

        [JsonPropertyName("gasLimit")]
        public string GasLimit { get; set; }

        [JsonPropertyName("gasUsed")]
        public string GasUsed { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("logsBloom")]
        public string LogsBloom { get; set; }

        [JsonPropertyName("miner")]
        public string Miner { get; set; }

        [JsonPropertyName("mixHash")]
        public string MixHash { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("parentHash")]
        public string ParentHash { get; set; }

        [JsonPropertyName("receiptsRoot")]
        public string ReceiptsRoot { get; set; }

        [JsonPropertyName("sha3Uncles")]
        public string Sha3Uncles { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("stateRoot")]
        public string StateRoot { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("totalDifficulty")]
        public string TotalDifficulty { get; set; }

        [JsonPropertyName("transactions")]
        public List<Transaction> Transactions { get; set; }

        [JsonPropertyName("transactionsRoot")]
        public string TransactionsRoot { get; set; }

        [JsonPropertyName("uncles")]
        public List<object> Uncles { get; set; }
    }

    public class InfuraTransactionResponse
    {
        [JsonPropertyName("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("result")]
        public Result Result { get; set; }
    }

}
