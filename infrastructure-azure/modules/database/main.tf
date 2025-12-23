resource "azurerm_mssql_database" "db" {
  name      = var.name
  server_id = var.db_server_id
  collation = "SQL_Latin1_General_CP1_CI_AS"
  license_type        = "LicenseIncluded"
  sku_name            = "S1" 

  lifecycle {
    prevent_destroy = true
  }
}

module "secret" {
  source       = "./../secret"
  key_vault_id = var.key_vault_id
  name         = "${azurerm_mssql_database.db.name}-secret"
  value        = jsonencode({
                    host     = var.db_server_host
                    username = var.db_server_username
                    password = var.db_server_password
                    dbname   = azurerm_mssql_database.db.name
                    engine   = "sqlserver"
                    port     = "1433"
                })
}