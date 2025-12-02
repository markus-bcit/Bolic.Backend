param projectName string
param environmentShort string
param accountName string = 'db-${projectName}-${environmentShort}'
param location string = resourceGroup().location

resource account 'Microsoft.DocumentDB/databaseAccounts@2025-10-15' = {
  name: accountName
  location: location
  properties: {
    enableFreeTier: true // FREE: 1000 RU/s + 25 GB storage
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session' // Cheapest option
    }
    locations: [
      {
        locationName: location
      }
    ]
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2025-10-15' = {
  parent: account
  name: projectName
  properties: {
    resource: {
      id: projectName
    }
    options: {
      throughput: 1000
    }
  }
}

resource trainingDaysContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-10-15' = {
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

resource exercisesContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-10-15' = {
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

resource trainingSessionContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-10-15' = {
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

resource usersContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-10-15' = {
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

resource setsContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-10-15' = {
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
