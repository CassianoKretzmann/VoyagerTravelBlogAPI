output "url" {
  value = "https://${azurerm_linux_web_app.appservice.name}.azurewebsites.net"
}

output "virtual_ip" {
  value = element(azurerm_linux_web_app.appservice.outbound_ip_address_list, length(azurerm_linux_web_app.appservice.outbound_ip_address_list) - 1)
}