locals {
  allowed_ips_map = { for idx, ip in var.allowed_ips : idx => ip }
}

resource "azurerm_linux_web_app" "appservice" {
  name                      = var.name
  location                  = var.region_name
  resource_group_name       = var.rg_name
  service_plan_id           = var.app_plan_id

  site_config {
    always_on = true

    application_stack {
      docker_image_name        = var.acr_image_repository
      docker_registry_url      = "https://${var.acr_login_server}"
      docker_registry_username = var.acr_login_username
      docker_registry_password = var.acr_login_password
    }

    health_check_path          = var.health_check_path

    dynamic "ip_restriction" {
      for_each = local.allowed_ips_map
      content {
        ip_address = "${ip_restriction.value}/32"
        action = "Allow"
        priority = 100 + ip_restriction.key
        name = "GlobalProtect_${replace(ip_restriction.value, ".", "-")}"
      }
    }

    ip_restriction_default_action = "Deny"
  }

  app_settings = var.environment

  logs {
    application_logs {
      file_system_level = "Verbose"
    }

    http_logs {
      file_system {
        retention_in_days = 7
        retention_in_mb   = 35
      }
    }
  }  

  public_network_access_enabled = var.public_network_access_enabled

  identity {
    type = "SystemAssigned"
  }

  connection_string {
    name  = "BlogConnectionString"
    type  = "SQLServer"
    value = var.sql_connection_strings
  }
}