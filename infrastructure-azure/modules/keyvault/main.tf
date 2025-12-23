resource "azurerm_key_vault" "vault" {
  name                        = var.name
  location                    = var.region_name
  resource_group_name         = var.rg_name
  tenant_id                   = var.tenant_id
  sku_name                    = "standard"
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false
  enabled_for_disk_encryption = true
  enable_rbac_authorization   = true

  access_policy {
    tenant_id = var.tenant_id
    object_id = var.object_id

    key_permissions = [
      "List",
      "Get",
      "Create",
      "Delete"
    ]

    secret_permissions = [
      "List",
      "Get",
      "Set",
      "Delete"
    ]
  }
}