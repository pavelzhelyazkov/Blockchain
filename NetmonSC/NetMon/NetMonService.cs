using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using NetmonSC.NetMon.ContractDefinition;

namespace NetmonSC.NetMon
{
    public partial class NetMonService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, NetMonDeployment netMonDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<NetMonDeployment>().SendRequestAndWaitForReceiptAsync(netMonDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, NetMonDeployment netMonDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<NetMonDeployment>().SendRequestAsync(netMonDeployment);
        }

        public static async Task<NetMonService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, NetMonDeployment netMonDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, netMonDeployment, cancellationTokenSource);
            return new NetMonService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public NetMonService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddHostRequestAsync(AddHostFunction addHostFunction)
        {
             return ContractHandler.SendRequestAsync(addHostFunction);
        }

        public Task<TransactionReceipt> AddHostRequestAndWaitForReceiptAsync(AddHostFunction addHostFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addHostFunction, cancellationToken);
        }

        public Task<string> AddHostRequestAsync(string hostname)
        {
            var addHostFunction = new AddHostFunction();
                addHostFunction.Hostname = hostname;
            
             return ContractHandler.SendRequestAsync(addHostFunction);
        }

        public Task<TransactionReceipt> AddHostRequestAndWaitForReceiptAsync(string hostname, CancellationTokenSource cancellationToken = null)
        {
            var addHostFunction = new AddHostFunction();
                addHostFunction.Hostname = hostname;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addHostFunction, cancellationToken);
        }

        public Task<GetAllHostsStateOutputDTO> GetAllHostsStateQueryAsync(GetAllHostsStateFunction getAllHostsStateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetAllHostsStateFunction, GetAllHostsStateOutputDTO>(getAllHostsStateFunction, blockParameter);
        }

        public Task<GetAllHostsStateOutputDTO> GetAllHostsStateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetAllHostsStateFunction, GetAllHostsStateOutputDTO>(null, blockParameter);
        }

        public Task<bool> GetHostStateQueryAsync(GetHostStateFunction getHostStateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetHostStateFunction, bool>(getHostStateFunction, blockParameter);
        }

        
        public Task<bool> GetHostStateQueryAsync(string hostname, BlockParameter blockParameter = null)
        {
            var getHostStateFunction = new GetHostStateFunction();
                getHostStateFunction.Hostname = hostname;
            
            return ContractHandler.QueryAsync<GetHostStateFunction, bool>(getHostStateFunction, blockParameter);
        }

        public Task<HostsOutputDTO> HostsQueryAsync(HostsFunction hostsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<HostsFunction, HostsOutputDTO>(hostsFunction, blockParameter);
        }

        public Task<HostsOutputDTO> HostsQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var hostsFunction = new HostsFunction();
                hostsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<HostsFunction, HostsOutputDTO>(hostsFunction, blockParameter);
        }

        public Task<string> SetHostStateRequestAsync(SetHostStateFunction setHostStateFunction)
        {
             return ContractHandler.SendRequestAsync(setHostStateFunction);
        }

        public Task<TransactionReceipt> SetHostStateRequestAndWaitForReceiptAsync(SetHostStateFunction setHostStateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setHostStateFunction, cancellationToken);
        }

        public Task<string> SetHostStateRequestAsync(string hostname, bool hostState)
        {
            var setHostStateFunction = new SetHostStateFunction();
                setHostStateFunction.Hostname = hostname;
                setHostStateFunction.HostState = hostState;
            
             return ContractHandler.SendRequestAsync(setHostStateFunction);
        }

        public Task<TransactionReceipt> SetHostStateRequestAndWaitForReceiptAsync(string hostname, bool hostState, CancellationTokenSource cancellationToken = null)
        {
            var setHostStateFunction = new SetHostStateFunction();
                setHostStateFunction.Hostname = hostname;
                setHostStateFunction.HostState = hostState;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setHostStateFunction, cancellationToken);
        }
    }
}
