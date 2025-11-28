param projectName string
param environmentShort string
param location string = resourceGroup().location
param appInsightsConnectionString string
param storageAccountName string
param storageAccountKey string
param hostingPlanId string

resource functionApp 'Microsoft.Web/sites@2023-12-01' = {
  name: 'func-${projectName}-${environmentShort}'
  location: location
  kind: 'functionapp,linux'
  properties: {
    serverFarmId: hostingPlanId
    reserved: true
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|8.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccountKey}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccountKey}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower('func-${projectName}-${environmentShort}')
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
