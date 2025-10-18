param projectName string
param environment string
param environmentShort string
param location string
param locationShort string

module Cosmos 'cosmos/cosmos.bicep' = {
  params: {
    environmentShort: environmentShort
    projectName: projectName
  }
}
