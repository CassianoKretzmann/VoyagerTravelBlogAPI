variable "name" {}
variable "region_name" {}
variable "rg_name" {}
variable "container_name" {}
variable "cors_allowed_methods" {
  type = list(string)
}
variable "cors_allowed_headers" {
  type = list(string)
}
variable "cors_allowed_origins" {
  type = list(string)
}
variable "cors_exposed_headers" {
  type = list(string)
}
variable "cors_max_age_in_seconds" {
  type = number
} 