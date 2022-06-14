// See https://aka.ms/new-console-template for more information

using EthereumSmartContracts.Contracts.SimpleStorage;
using EthereumSmartContracts.Contracts.SimpleStorage.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Threading.Tasks;

// Setup
var url = "https://rinkeby.infura.io/v3/6890a49514b74901ab2e907fc3b89308";
var web3 = new Web3(url);
// An already-deployed SimpleStorage.sol contract on Rinkeby:
var contractAddress = "0xb52Fe7D1E04fbf47918Ad8d868103F03Da6ec4fE";
var service = new SimpleStorageService(web3, contractAddress);

// Get the stored value
var currentStoredValue = await service.GetQueryAsync();
Console.WriteLine($"Contract has value stored: {currentStoredValue}");
