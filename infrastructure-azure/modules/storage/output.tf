output "storage_account_id" {
  value = azurerm_storage_account.storage.id
}
output "storage_account_name" {
  value = azurerm_storage_account.storage.name
}

output "storage_container_id" {
  value = azurerm_storage_container.container.id
}

output "storage_container_name" {
  value = azurerm_storage_container.container.name
}

output "storage_access_key" {
  value = azurerm_storage_account.storage.primary_access_key
}

output "storage_connection_string" {
  value = azurerm_storage_account.storage.primary_connection_string
}

output "storage_url" {
  value = azurerm_storage_account.storage.primary_blob_endpoint
}