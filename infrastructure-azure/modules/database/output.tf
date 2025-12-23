output "name" {
  value = azurerm_mssql_database.db.name
}
output "database_secret_name" {
  value = module.secret.secret_name
}