param projectName string
param environmentShort string
param location string = resourceGroup().location

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'appi-${projectName}-${environmentShort}'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 30 // Minimum retention = cheaper
    IngestionMode: 'ApplicationInsights'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    SamplingPercentage: 50 // Sample 50% of telemetry to stay under free tier
  }
}

output appInsightsName string = appInsights.name
output connectionString string = appInsights.properties.ConnectionString
