resource "azurerm_service_plan" "app_service_plan" {
  name                = var.name
  location            = var.region_name
  resource_group_name = var.rg_name
  os_type             = "Linux"
  sku_name            = var.asp_tier
}