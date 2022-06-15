// See https://aka.ms/new-console-template for more information

using EthereumSmartContracts.Contracts.SimpleStorage;
using EthereumSmartContracts.Contracts.SimpleStorage.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Threading.Tasks;

// Setup using the Nethereum public test chain
var url = "http://testchain.nethereum.com:8545";
var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
var account = new Account(privateKey);
var web3 = new Web3(account, url);

Console.WriteLine("Deploying...");
var deployment = new SimpleStorageDeployment();
var receipt = await SimpleStorageService.DeployContractAndWaitForReceiptAsync(web3, deployment);
var service = new SimpleStorageService(web3, receipt.ContractAddress);
Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
Console.WriteLine("");

Console.WriteLine("Sending a transaction to the function set()...");
var receiptForSetFunctionCall = await service.SetRequestAndWaitForReceiptAsync(new SetFunction() { X = 42, Gas = 400000 });
Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForSetFunctionCall.TransactionHash}");
Console.WriteLine($"Finished storing an int: Tx Status: {receiptForSetFunctionCall.Status.Value}");
Console.WriteLine("");

Console.WriteLine("Calling the function get()...");
var intValueFromGetFunctionCall = await service.GetQueryAsync();
Console.WriteLine($"Int value: {intValueFromGetFunctionCall} (expecting value 42)");
Console.WriteLine("");
