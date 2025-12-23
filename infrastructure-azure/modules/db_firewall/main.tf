locals {
  allowed_ips_map = { for idx, ip in var.allowed_ips : idx => ip }
}

resource "azurerm_mssql_firewall_rule" "firewall" {
  for_each = local.allowed_ips_map
  name = "GlobalProtect_${each.key}"
  start_ip_address = each.value
  end_ip_address = each.value
  server_id = var.server_id
}