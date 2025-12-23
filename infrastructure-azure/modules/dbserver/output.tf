output "name" {
  value = azurerm_mssql_server.sql.name
}

output "id" {
  value = azurerm_mssql_server.sql.id
}

output "host" {
  value = "${azurerm_mssql_server.sql.name}.mssql.database.azure.com"
}

output "username" {
  value = azurerm_mssql_server.sql.administrator_login
}

output "password" {
  value = azurerm_mssql_server.sql.administrator_login_password
}