param projectName string
param accountName string = 'db-shared'

resource account 'Microsoft.DocumentDB/databaseAccounts@2025-10-15' existing = {
  name: accountName
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2025-10-15' = {
  parent: account
  name: projectName
  properties: {
    resource: {
      id: projectName
    }
    options: {
      throughput: 1000 // Shared across all containers - stays under 1000 RU/s free tier, could change if another DB is added.
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
          '/userId'
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
          '/userId'
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
          '/userId'
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
          '/userId'
        ]
      }
    }
  }
}
