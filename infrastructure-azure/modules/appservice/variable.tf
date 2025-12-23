variable "name" {}
variable "region_name" {}
variable "rg_name" {}
variable "app_plan_id" {}
variable "acr_login_server" {
  default = ""
}
variable "acr_image_repository" {}
variable "acr_login_username" {
  default = ""
}
variable "acr_login_password" {
  default = ""
}
variable "acr_registry_name" {}
variable "public_network_access_enabled" {
  default = true
}
variable "environment" {
  type    = map(string)
  default = {}
}
variable "health_check_path" {
  default = ""
}
variable "sql_connection_strings" {
  default = ""
}
variable "allowed_ips" {
  type = list(string)
}
