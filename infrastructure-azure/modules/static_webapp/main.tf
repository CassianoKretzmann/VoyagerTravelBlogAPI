resource "azurerm_static_web_app" "staticwebapp" {
  name                = var.name
  resource_group_name = var.rg_name
  location            = "eastus2"
}

output "default_hostname" {
  value = azurerm_static_web_app.staticwebapp.default_host_name
}

output "id" {
  value = azurerm_static_web_app.staticwebapp.id
}