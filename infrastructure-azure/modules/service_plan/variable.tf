variable "name" {
  validation {
    condition     = length(var.name) > 0
    error_message = "The name cannot be empty."
  }
}

variable "region_name" {
  validation {
    condition     = length(var.region_name) > 0
    error_message = "The region_name cannot be empty."
  }
}

variable "rg_name" {
  validation {
    condition     = length(var.rg_name) > 0
    error_message = "The rg_name cannot be empty."
  }
}

variable "asp_tier" {
  type    = string
  default = "F1"
}