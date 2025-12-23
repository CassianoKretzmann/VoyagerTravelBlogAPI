resource "random_password" "password" {
  length  = 16
  special = false
}

resource "azurerm_mssql_server" "sql" {
  name                          = var.name
  location                      = var.region_name
  resource_group_name           = var.rg_name
  administrator_login           = "sqladmin"
  administrator_login_password  = random_password.password.result
  version                       = "12.0"
}