param projectName string
param environmentShort string
param location string

module Cosmos 'cosmos/cosmos.bicep' = {
  params: {
    environmentShort: environmentShort
    projectName: projectName
    location: location
  }
}

module StorageAccount 'storage/storageAccount.bicep' = {
  params: {
    environmentShort: environmentShort
    projectName: projectName
    location: location
  }
}

module AppInsights 'monitoring/appInsights.bicep' = {
  params: {
    environmentShort: environmentShort
    projectName: projectName
    location: location
  }
}

module HostingPlan 'app/hostingPlan.bicep' = {
  params: {
    environmentShort: environmentShort
    projectName: projectName
    location: location
  }
}

module FunctionApp 'app/functionApp.bicep' = {
  params: {
    environmentShort: environmentShort
    projectName: projectName
    location: location
    appInsightsConnectionString: AppInsights.outputs.connectionString
    storageAccountName: StorageAccount.outputs.storageAccountName
    hostingPlanId: HostingPlan.outputs.hostingPlanId
  }
}

module StorageRoleAssignments 'storage/roleAssignments.bicep' = {
  params: {
    storageAccountName: StorageAccount.outputs.storageAccountName
    functionAppPrincipalId: FunctionApp.outputs.functionAppPrincipalId
  }
}
