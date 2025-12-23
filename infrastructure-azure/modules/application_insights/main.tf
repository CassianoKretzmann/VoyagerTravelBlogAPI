resource "azurerm_application_insights" "web_application_insights" {
  name                = var.name
  resource_group_name = var.rg_name
  location            = var.region_name
  application_type    = "web"
}