using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NetmonSC.NetMon;
using NetmonSC.NetMon.ContractDefinition;
using System.Net;
using System.Net.NetworkInformation;

namespace NetmonSC
{
    class Program
    {
        // Local chain URL (Ganache)
        static string url = "http://localhost:8545";
        // Account private key
        static string privateKey = "0xe799e14e970e922803c5afbfc4edc0860a45317db9bb7769f1e721bd61c3ef5a";
        // Chain ID
        static int chainId = 1337;
        // Contract address
        static string contract = "0x1581171db3732cb9f49d2fe95afb2d5e7335519f";

        static int Main(string[] args)
        {
            string hostname = "unknown";

            if (args.Length == 0)
            {
                System.Console.WriteLine("Missing arg: deploycontract, addhost, " +
                    "setstate, getstate, pingandset, getallstates");

                return 1;
            }

            string command = args[0];

            if (args.Length == 2)
            {
                 hostname = args[1];
            }

            switch (command)
            {
                case "deploycontract":
                    deployContract().Wait();
                    break;
                case "addhost":
                    addHost(hostname).Wait();
                    break;
                case "setstate":
                    setState(hostname).Wait();
                    break;
                case "getstate":
                    getState(hostname).Wait();
                    break;
                case "pingandset":
                    pingAndSetState(hostname).Wait();
                    break;
                case "getallstates":
                    getAllStates().Wait();
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }

            return 0;
        }

        static async Task deployContract()
        {
            try
            {
                var account = new Account(privateKey, chainId);
                var web3 = new Web3(account, url);

                Console.WriteLine("Deploying...");
                var deployment = new NetMonDeployment();

                // MaxFeePerGas
                deployment.MaxFeePerGas = 875000000;
                // MaxPriorityFeePerGas
                deployment.MaxPriorityFeePerGas = 1000;

                var receipt = await NetMonService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                var service = new NetMonService(web3, receipt.ContractAddress);

                Console.WriteLine($"Contract Deployment Status: {receipt.Status.Value}");
                Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
                Console.WriteLine("");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        static async Task addHost(string host)
        {
            try
            {
                var account = new Account(privateKey, chainId);
                var web3 = new Web3(account, url);

                var service = new NetMonService(web3, contract);

                Console.WriteLine("Sending a transaction to the function addHost()...");
                var result = await service.AddHostRequestAndWaitForReceiptAsync(
                new AddHostFunction() { Hostname = host, Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });

                Console.WriteLine($"Transaction Hash: {result.TransactionHash}");
                Console.WriteLine($"Transaction GasUsed: {result.GasUsed}");
                Console.WriteLine($"Status: {result.Status.Value}");
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        static async Task setState(string host)
        {
            try
            {
                var account = new Account(privateKey, chainId);
                var web3 = new Web3(account, url);

                var service = new NetMonService(web3, contract);

                Console.WriteLine("Calling the function setHostState()...");
                var result = await service.SetHostStateRequestAndWaitForReceiptAsync(new SetHostStateFunction { Hostname = host, HostState = true,
                    Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });

                Console.WriteLine($"Transaction hash: {result.TransactionHash}");
                Console.WriteLine($"Transaction GasUsed: {result.GasUsed}");
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        static async Task getState(string host)
        {
            try
            {
                var account = new Account(privateKey, chainId);
                var web3 = new Web3(account, url);

                var service = new NetMonService(web3, contract);

                Console.WriteLine("Calling the function getHostState()...");
                var result = await service.GetHostStateQueryAsync(new GetHostStateFunction { Hostname = host, 
                    Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });

                Console.WriteLine($"Host state value: {result}");
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        static async Task getAllStates()
        {
            try
            {
                var account = new Account(privateKey, chainId);
                var web3 = new Web3(account, url);

                var service = new NetMonService(web3, contract);

                Console.WriteLine("Calling the function getAllHostsStates()...");
                var result = await service.GetAllHostsStateQueryAsync(new GetAllHostsStateFunction { 
                    Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });

                foreach(MonData mondata in result.ReturnValue1)
                {
                    Console.WriteLine($"Host state: {mondata.Hostname}, {mondata.State.ToString()}");
                }
                
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        static async Task pingAndSetState(string host)
        {
            bool state = false;
            Ping pingSender = new Ping();

            PingReply reply = pingSender.Send(host);

            if(reply.Status == IPStatus.Success)
            {
                state = true;
            }

            try
            {
                var account = new Account(privateKey, chainId);
                var web3 = new Web3(account, url);

                var service = new NetMonService(web3, contract);

                Console.WriteLine("Calling the function setHostState()...");
                var result = await service.SetHostStateRequestAndWaitForReceiptAsync(new SetHostStateFunction { Hostname = host, HostState = state, 
                    Gas = 400000, MaxFeePerGas = 875000000, MaxPriorityFeePerGas = 1000 });

                Console.WriteLine($"Transaction hash: {result.TransactionHash}");
                Console.WriteLine($"Transaction GasUsed: {result.GasUsed}");
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
