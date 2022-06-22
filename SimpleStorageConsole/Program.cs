﻿// // See https://aka.ms/new-console-template for more information

// using EthereumSmartContracts.Contracts.SimpleStorage;
// using EthereumSmartContracts.Contracts.SimpleStorage.ContractDefinition;
// using Nethereum.Web3;
// using Nethereum.Web3.Accounts;
// using System;
// using System.Threading.Tasks;

// // Setup using the Nethereum public test chain
// var url = "http://testchain.nethereum.com:8545";
// var privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";
// var account = new Account(privateKey);
// var web3 = new Web3(account, url);

// Console.WriteLine("Deploying...");
// var deployment = new SimpleStorageDeployment();
// var receipt = await SimpleStorageService.DeployContractAndWaitForReceiptAsync(web3, deployment);
// var service = new SimpleStorageService(web3, receipt.ContractAddress);
// Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
// Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
// Console.WriteLine("");

// Console.WriteLine("Sending a transaction to the function set()...");
// var receiptForSetFunctionCall = await service.SetRequestAndWaitForReceiptAsync(new SetFunction() { X = 42, Gas = 400000 });
// Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForSetFunctionCall.TransactionHash}");
// Console.WriteLine($"Finished storing an int: Tx Status: {receiptForSetFunctionCall.Status.Value}");
// Console.WriteLine("");

// Console.WriteLine("Calling the function get()...");
// var intValueFromGetFunctionCall = await service.GetQueryAsync();
// Console.WriteLine($"Int value: {intValueFromGetFunctionCall} (expecting value 42)");
// Console.WriteLine("");


//NEW


using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;


public class SmartContracts_DeployingContract
{
	// To deploy a contract we will create a class inheriting from the ContractDeploymentMessage, 
	// here we can include our compiled byte code and other constructor parameters.
	// As we can see below the StandardToken deployment message includes the compiled bytecode 
	// of the ERC20 smart contract and the constructor parameter with the “totalSupply” of tokens.
	// Each parameter is described with an attribute Parameter, including its name "totalSupply", type "uint256" and order.

	public class StandardTokenDeployment : ContractDeploymentMessage
	{
		public static string BYTECODE =
			"0x60606040526040516020806106f5833981016040528080519060200190919050505b80600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005081905550806000600050819055505b506106868061006f6000396000f360606040523615610074576000357c010000000000000000000000000000000000000000000000000000000090048063095ea7b31461008157806318160ddd146100b657806323b872dd146100d957806370a0823114610117578063a9059cbb14610143578063dd62ed3e1461017857610074565b61007f5b610002565b565b005b6100a060048080359060200190919080359060200190919050506101ad565b6040518082815260200191505060405180910390f35b6100c36004805050610674565b6040518082815260200191505060405180910390f35b6101016004808035906020019091908035906020019091908035906020019091905050610281565b6040518082815260200191505060405180910390f35b61012d600480803590602001909190505061048d565b6040518082815260200191505060405180910390f35b61016260048080359060200190919080359060200190919050506104cb565b6040518082815260200191505060405180910390f35b610197600480803590602001909190803590602001909190505061060b565b6040518082815260200191505060405180910390f35b600081600260005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008573ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925846040518082815260200191505060405180910390a36001905061027b565b92915050565b600081600160005060008673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561031b575081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505410155b80156103275750600082115b1561047c5781600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a381600160005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505403925050819055506001905061048656610485565b60009050610486565b5b9392505050565b6000600160005060008373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505490506104c6565b919050565b600081600160005060003373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561050c5750600082115b156105fb5781600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a36001905061060556610604565b60009050610605565b5b92915050565b6000600260005060008473ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005054905061066e565b92915050565b60006000600050549050610683565b9056";

		public StandardTokenDeployment() : base(BYTECODE)
		{
		}

		[Parameter("uint256", "totalSupply")]
		public BigInteger TotalSupply { get; set; }
	}

	public static async Task Main()
	{
		//  Instantiating Web3 and the Account

		// To create an instance of web3 we first provide the url of our testchain and the private key of our account. 
		// When providing an Account instantiated with a  private key all our transactions will be signed “offline” by Nethereum.

		var privateKey = "c2a803391ae58e53088284e37cca5a465ac4c2f850c2b3fcdafa5540a6c7382c";
		var account = new Account(privateKey);
		Console.WriteLine("Our account: " + account.Address);
		//Now let's create an instance of Web3 using our account pointing to our nethereum testchain
		var web3 = new Web3(account, "http://127.0.0.1:7545");
		web3.TransactionManager.UseLegacyAsDefault = true;
		// web3.TransactionManager.UseLegacyAsDefault = true; //Using legacy option instead of 1559

		//  Deploying the Contract
		// The next step is to deploy our Standard Token ERC20 smart contract, in this scenario the total supply (number of tokens) is going to be 100,000.
		// First we create an instance of the StandardTokenDeployment with the TotalSupply amount.

		var deploymentMessage = new StandardTokenDeployment
		{
			TotalSupply = 100000
		};

		// Then we create a deployment handler using our contract deployment definition and simply deploy the contract using the deployment message. We are auto estimating the gas, getting the latest gas price and nonce so nothing else is set anything on the deployment message.
		// Finally, we wait for the deployment transaction to be mined, and retrieve the contract address of the new contract from the receipt.

		var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenDeployment>();

		var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);

		var contractAddress = transactionReceipt.ContractAddress;

		Console.WriteLine("Deployed Contract address is: " + contractAddress);
	}
}