param projectName string
param environmentShort string
param accountName string = 'db-${projectName}-${environmentShort}'
param location string = resourceGroup().location

resource account 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: accountName
  location: location
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    enableFreeTier: true // FREE: 1000 RU/s + 25 GB storage
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session' // Cheapest option
    }
    backupPolicy: {
      type: 'Periodic' // FREE (vs Continuous which costs extra)
      periodicModeProperties: {
        backupIntervalInMinutes: 240 // Max interval = fewer backups = cheaper
        backupRetentionIntervalInHours: 8 // Min retention = cheaper
        backupStorageRedundancy: 'Local' // Cheapest redundancy
      }
    }
    enableAnalyticalStorage: false // Costs extra, disabled
    enableAutomaticFailover: false // Not needed for single region
    disableKeyBasedMetadataWriteAccess: false
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  parent: account
  name: projectName
  properties: {
    resource: {
      id: projectName
    }
    options: {
      throughput: 1000 // Shared across all containers - stays under 1000 RU/s free tier
    }
  }
}

resource trainingDaysContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: database
  name: 'training-days'
  properties: {
    resource: {
      id: 'training-days'
      partitionKey: {
        paths: [
          '/UserId'
        ]
      }
    }
  }
}

resource exercisesContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: database
  name: 'exercises'
  properties: {
    resource: {
      id: 'exercises'
      partitionKey: {
        paths: [
          '/UserId'
        ]
      }
    }
  }
}

resource trainingSessionContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: database
  name: 'training-sessions'
  properties: {
    resource: {
      id: 'training-sessions'
      partitionKey: {
        paths: [
          '/UserId'
        ]
      }
    }
  }
}

resource usersContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: database
  name: 'users'
  properties: {
    resource: {
      id: 'users'
      partitionKey: {
        paths: [
          '/id'
        ]
        kind: 'Hash'
      }
    }
  }
}

resource setsContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: database
  name: 'sets'
  properties: {
    resource: {
      id: 'sets'
      partitionKey: {
        paths: [
          '/UserId'
        ]
      }
    }
  }
}
