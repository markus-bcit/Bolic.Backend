param projectName string
param environmentShort string
param location string = resourceGroup().location
param appInsightsConnectionString string
param storageAccountName string
param hostingPlanId string

resource functionApp 'Microsoft.Web/sites@2023-12-01' = {
  name: 'func-${projectName}-${environmentShort}'
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlanId
    reserved: true
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|8.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage__accountName'
          value: storageAccountName
        }
        {
          name: 'AzureWebJobsStorage__credential'
          value: 'managedidentity'
        }
        {
          name: 'AzureWebJobsStorage__clientId'
          value: 'system'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
      ]
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      use32BitWorkerProcess: false // 64-bit = better performance, same cost on Consumption
    }
  }
}

output functionAppName string = functionApp.name
output functionAppPrincipalId string = functionApp.identity.principalId
