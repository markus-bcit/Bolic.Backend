param projectName string
param environmentShort string
param accountName string = 'db-${projectName}-${environmentShort}'
param location string = resourceGroup().location


resource account 'Microsoft.DocumentDB/databaseAccounts@2025-05-01-preview' = {
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
    enableFreeTier: true // this
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2024-02-15-preview' = {
  parent: account
  name: projectName
  properties: {
    resource: {
      id: projectName
    }
  }
}

resource trainingDaysContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-05-01-preview' = {
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

resource exercisesContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-05-01-preview' = {
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

resource trainingSessionContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-05-01-preview' = {
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

resource usersContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-05-01-preview' = {
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

resource setsContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-05-01-preview' = {
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
