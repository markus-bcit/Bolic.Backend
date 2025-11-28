param projectName string
param environmentShort string
param location string = resourceGroup().location

resource hostingPlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'asp-${projectName}-${environmentShort}'
  location: location
  sku: {
    name: 'Y1' // Consumption plan - pay per execution
    tier: 'Dynamic'
  }
  properties: {
    reserved: true // Linux = cheaper than Windows
  }
}

output hostingPlanId string = hostingPlan.id
