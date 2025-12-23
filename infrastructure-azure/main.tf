# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }

  backend "azurerm" {
    storage_account_name = "voyagerblogtfstate"
    container_name       = "voyagerblog-tfstate"
    key                  = "terraform.tfstate"
    resource_group_name  = "wt-internal-blog-project-rg"
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy    = true
      recover_soft_deleted_key_vaults = true
    }
  }
}

# Get current client configuration
data "azurerm_client_config" "current" {}

# Get resource group
data "azurerm_resource_group" "wt-internal-blog-project-rg" {
  name = var.rg_name
}

# Create Application Insights
module "voyagerblog-application-Insights" {
  source      = "./modules/application_insights"
  name        = "${var.app_name}-api"
  region_name = var.region_name
  rg_name     = var.rg_name
}

# Create a Key Vault
module "voyagerblog-keyvault" {
  source      = "./modules/keyvault"
  name        = "${var.app_name}-vault"
  region_name = var.region_name
  rg_name     = var.rg_name
  tenant_id   = data.azurerm_client_config.current.tenant_id
  object_id   = data.azurerm_client_config.current.object_id
}

# Create a SQL Server
module "voyagerblog-dbserver" {
  source      = "./modules/dbserver"
  name        = "${var.app_name}-dbserver"
  region_name = var.region_name
  rg_name     = var.rg_name
}

# Create a SQL Database
module "voyagerblog-database" {
  source             = "./modules/database"
  name               = "${var.app_name}-database"
  rg_name            = var.region_name
  key_vault_id       = module.voyagerblog-keyvault.id
  db_server_id       = module.voyagerblog-dbserver.id
  db_server_host     = module.voyagerblog-dbserver.host
  db_server_username = module.voyagerblog-dbserver.name
  db_server_password = module.voyagerblog-dbserver.password
}

# Create an Azure Container Registry
module "voyagerblog-acr" {
  source      = "./modules/container_registry"
  name        = "${var.app_name}apiacr"
  region_name = var.region_name
  rg_name     = var.rg_name
}

# Create an App Service Plan
module "voyagerblog-serviceplan" {
  source      = "./modules/service_plan"
  name        = "${var.app_name}-asp"
  region_name = var.region_name
  rg_name     = var.rg_name
  asp_tier    = var.asp_tier
}

# Create a Storage Account and Blob Storage Container
module "voyagerblog-storage" {
  source                  = "./modules/storage"
  name                    = "${var.app_name}storage"
  region_name             = var.region_name
  rg_name                 = var.rg_name
  container_name          = "${var.app_name}container"
  cors_allowed_methods    = ["PUT", "GET", "DELETE", "POST"]
  cors_allowed_origins    = var.allowed_origins_storage
  cors_allowed_headers    = ["*"]
  cors_exposed_headers    = ["*"]
  cors_max_age_in_seconds = 300
}

# Create API App Service
module "voyagerblog-webapp-api" {
  name                 = "${var.app_name}-api"
  source               = "./modules/appservice"
  region_name          = var.region_name
  rg_name              = var.rg_name
  app_plan_id          = module.voyagerblog-serviceplan.plan_id
  acr_registry_name    = module.voyagerblog-acr.name
  acr_login_username   = module.voyagerblog-acr.login_username
  acr_login_password   = module.voyagerblog-acr.login_password
  acr_login_server     = module.voyagerblog-acr.login_server
  acr_image_repository = "voyagerblog-api"
  health_check_path    = "/health"
  allowed_ips          = var.allowed_external_ips

  environment = {
    WEBSITES_PORT                              = "8080"
    BLOB_BUCKET_NAME                           = module.voyagerblog-storage.storage_account_name
    AZURE_STORAGE_ACCOUNT_NAME                 = module.voyagerblog-storage.storage_account_name
    AZURE_STORAGE_ACCOUNT_KEY                  = module.voyagerblog-storage.storage_access_key
    APPINSIGHTS_INSTRUMENTATIONKEY             = module.voyagerblog-application-Insights.instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING      = module.voyagerblog-application-Insights.connection_string
    ApplicationInsightsAgent_EXTENSION_VERSION = "~3"
  }

  public_network_access_enabled = true

  sql_connection_strings = "Server=tcp:${module.voyagerblog-dbserver.name}.database.windows.net,1433;Initial Catalog=${module.voyagerblog-database.name};User ID=${module.voyagerblog-dbserver.username};Password=${module.voyagerblog-dbserver.password};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}

# Create UI Static App
module "voyagerblog-staticwebapp-ui" {
  name    = "${var.app_name}-ui"
  source  = "./modules/static_webapp"
  rg_name = var.rg_name
}

# Create App Service Domain
module "voyagerblog-appservice_domain" {
  depends_on        = [module.voyagerblog-staticwebapp-ui]
  source            = "./modules/appservice_domain"
  rg_name           = var.rg_name
  location          = var.region_name
  dns_name          = "poatekvoyagerblog.com"
  c_name            = "www"
  default_host_name = module.voyagerblog-staticwebapp-ui.default_hostname
  static_webapp_id  = module.voyagerblog-staticwebapp-ui.id
}

# Create firewall rules for Global Protect ips
module "voyagerblog-dbfirewall" {
  source      = "./modules/db_firewall"
  server_id   = module.voyagerblog-dbserver.id
  allowed_ips = var.allowed_external_ips
}

