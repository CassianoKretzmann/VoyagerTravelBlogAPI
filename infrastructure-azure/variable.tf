variable "rg_name" {
  default = "wt-internal-blog-project-rg"
}

variable "app_name" {
  type    = string
  default = "voyagerblog"
  validation {
    condition     = length(var.app_name) > 0
    error_message = "The app_name cannot be empty."
  }
}

variable "env" {
  default = "dev"
  validation {
    condition     = length(var.env) > 0
    error_message = "The env cannot be empty."
  }
}

variable "region_name" {
  type    = string
  default = "eastus"
  validation {
    condition     = length(var.region_name) > 0
    error_message = "The region_name cannot be empty."
  }
}

variable "allowed_external_ips" {
  type    = list(string)
  default = ["140.209.213.84"]
}

variable "api_version" {
  default = "1"
}

variable "openai_api_key" {
  default = ""
}

variable "asp_tier" {
  type    = string
  default = "P0v3"
}

variable "allowed_origins_storage" {
  type    = list(string)
  default = ["*"]
}