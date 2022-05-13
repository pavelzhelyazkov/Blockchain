# Blockchain UniBit course work

netmon.sol - Smart ontract (NetMon)

NetmonSC - Visual Studio C# console project


Демонстрира създаването на смарт контракт за Ethereum чрез Solidity. Смарт контрактът съхранява масив от структурирани данни за ping състояние на мрежови хостове. Състоянието на отделните хостове може да се извлича чрез getHostState функция и съответно да се променя чрез setHostState функция. Извличане на състоянието на всички добавени хостове чрез функция addHost се прави посредством getAllHostsState.

Deployment и взаимодействие със смарт контракта е реализирано чрез Nethereum (.NET интеграционна библиотека за Ethereum).

  1. Чрез Visual Studio Code и разширение Truffle смарт контрактът (NetMon, netmon.sol) се компилира до bytecode;
  2. Във Visual Studio (C#) чрез Nethereum се съставят необходимите класове и обекти за работа със смарт контракта;
  3. Във Visual Studio (C#) конзолно приложение чрез команди, подавани като аргументи в command line изпълнява функции:
      
      netmonsc deploycontract - deployment на контрактът към тестов Blockchain - Ganache (localhost: 8545);
      netmonsc addhost {hostname} - добавя нов хост към масива от хостове в смарт контракта;
      setstate - променя състоянието (true, false) на хост присъстващ в масива от хостове в смарт контракта;
      getstate - взима състояние (true, false) на хост от масива от хостове в смарт контракта;
      pingandset - пингва (ICMP ping) хост и в зависимост от отговра, променя състояние (ture, false) на хост от масива от хостове в смарт контаркта;
      getallstates - извлича всички хостове от масива от хостове съхранен в смарт контракта и го визуализира в конзолата.
