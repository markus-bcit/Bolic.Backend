param projectName string
param environmentShort string
param location string = resourceGroup().location

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: 'st${projectName}${environmentShort}'
  location: location
  sku: {
    name: 'Standard_LRS' // Cheapest: Locally Redundant Storage
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    accessTier: 'Hot' // Required for Functions
    supportsHttpsTrafficOnly: true
  }
}

output storageAccountName string = storageAccount.name
output storageAccountKey string = storageAccount.listKeys().keys[0].value
