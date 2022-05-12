// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

contract NetMon {

    struct monData {
        string hostname;
        bool state;
    }

    // Масив от тип monData (struct)
    monData[] public hosts;


    // Добавяне на хост.
    function addHost(string memory hostname) public returns (bool) {

        monData memory host;
        uint i;

        for(i = 0; i < hosts.length; i++)
        {
            if(keccak256(abi.encodePacked(hosts[i].hostname)) == keccak256(abi.encodePacked(hostname))) {
                return false;
            }
        }

        host.hostname = hostname;
        host.state = false;
        hosts.push(host);

        return true;

    }

    // Промяна на състоянието на хост.
    function setHostState(string memory hostname, bool hostState) public returns (bool) {
        uint i;

        for(i = 0; i < hosts.length; i++)
        {
            if (keccak256(abi.encodePacked(hosts[i].hostname)) == keccak256(abi.encodePacked(hostname))) {
                hosts[i].state = hostState;
                return true;
            }
        }

        // Ако хостът не е намерен.
        return false;
    }


    // Взима състояние на хост.
    function getHostState(string memory hostname) public view returns (bool) {
        uint i;

        for(i = 0; i < hosts.length; i++)
        {
            if (keccak256(abi.encodePacked(hosts[i].hostname)) == keccak256(abi.encodePacked(hostname))) {
                return hosts[i].state;
            }
        }

        // Ако хостът не е намерен.
        return false;
    }

    function getAllHostsState() public view returns (monData[] memory) {
        return hosts;
    }
}