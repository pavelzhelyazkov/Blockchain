using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NetmonSC.NetMon;
using NetmonSC.NetMon.ContractDefinition;

namespace NetmonSC
{
    class Program
    {
        static void Main(string[] args)
        {
            Demo().Wait();
        }

        static async Task Demo()
        {
            try
            {
                // Setup
                // Here we're using local chain eg Geth https://github.com/Nethereum/TestChains#geth
                var url = "http://localhost:8545";
                var privateKey = "0xe799e14e970e922803c5afbfc4edc0860a45317db9bb7769f1e721bd61c3ef5a";
                var account = new Account(privateKey, 1337);
                var web3 = new Web3(account, url);

                Console.WriteLine("Deploying...");
                var deployment = new NetMonDeployment();
                deployment.MaxFeePerGas = 875000000;
                deployment.MaxPriorityFeePerGas = 1000;

                var receipt = await NetMonService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                var service = new NetMonService(web3, receipt.ContractAddress);

                Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
                Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
                Console.WriteLine("");

                Console.WriteLine("Sending a transaction to the function set()...");
                var receiptForSetFunctionCall = await service.AddHostRequestAndWaitForReceiptAsync(
                    new AddHostFunction() { Hostname = "ntp.host.bg", Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });

                Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForSetFunctionCall.TransactionHash}");
                Console.WriteLine($"Finished storing an int: Tx Status: {receiptForSetFunctionCall.Status.Value}");
                Console.WriteLine("");

                Console.WriteLine("Calling the function get()...");
                var intValueFromGetFunctionCall = await service.GetHostStateQueryAsync(new GetHostStateFunction { Hostname = "ntp.host.bg", Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });
                Console.WriteLine($"Host state value: {intValueFromGetFunctionCall}");
                Console.WriteLine("");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

    }
}
