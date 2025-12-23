resource "azurerm_storage_account" "storage" {
  name                             = var.name
  resource_group_name              = var.rg_name
  location                         = var.region_name
  account_tier                     = "Standard"
  account_replication_type         = "LRS"
  cross_tenant_replication_enabled = false
  shared_access_key_enabled        = true
  is_hns_enabled                   = false
  large_file_share_enabled         = false
  min_tls_version                  = "TLS1_0"

  blob_properties {
    cors_rule {
      allowed_methods    = var.cors_allowed_methods
      allowed_headers    = var.cors_allowed_headers
      allowed_origins    = var.cors_allowed_origins
      exposed_headers    = var.cors_exposed_headers
      max_age_in_seconds = var.cors_max_age_in_seconds
    }
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_storage_container" "container" {
  name                  = var.container_name
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}

resource "azurerm_storage_management_policy" "lifecycle" {
  storage_account_id = azurerm_storage_account.storage.id

  rule {
    name    = "lifecycleRule"
    enabled = true
    filters {
      prefix_match = ["${azurerm_storage_container.container.name}/temp/"]
      blob_types   = ["blockBlob"]
    }
    actions {
      base_blob {
        tier_to_cool_after_days_since_modification_greater_than    = 10
        tier_to_archive_after_days_since_modification_greater_than = 50
      }
    }
  }
}