﻿Import-Module $PSScriptRoot\K2Field.Powershell.Module.dll -Verbose -Force

# 1. Copying the assembly name and registering the service type
$assemblyName = "K2Field.ManagementPack.ServiceBroker.dll"
$assemblyPath = "$PSScriptRoot\$assemblyName"
$serviceTypeSystemName = "K2Field.ManagementPack.ServiceBroker.ServiceBroker"
$serviceTypeDisplayName = "K2Field.ManagementPack.ServiceBroker"
$serviceTypeGuid = [guid]"ec28072b-7f0c-43e7-ae2f-a5ca205e90b6"
$dllPath = "$PSScriptRoot\$assemblyName"

Write-Host "`n 1. Copying and registering the Service Type"
Write-Verbose "Restarting K2 Service"
Restart-K2Service -Verbose
Write-Verbose "K2 Service was restarted"

Write-Verbose "Copying the Assembly Name"
$k2InstallPath = Get-K2InstallPath
$serviceBrokerPath = $k2InstallPath + "ServiceBroker"
Copy-Item $assemblyPath -Destination $serviceBrokerPath

Write-Verbose "Registering Service type"
New-K2ServiceType -Assembly $assemblyName -SystemName $serviceTypeSystemName -DisplayName $serviceTypeDisplayName -Guid $serviceTypeGuid -Verbose

Write-Host "`n 2. Deployment of the K2 Package"
