output "login_server" {
  value = azurerm_container_registry.acr.login_server
}

output "login_username" {
  value = azurerm_container_registry.acr.admin_username
}

output "login_password" {
  value = azurerm_container_registry.acr.admin_password
}
output "name" {
  value = azurerm_container_registry.acr.name
}