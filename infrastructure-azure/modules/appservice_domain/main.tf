resource "azurerm_dns_zone" "dns_zone" {
  name                = var.dns_name
  resource_group_name = var.rg_name
}

resource "azurerm_static_web_app_custom_domain" "appservice_domain" {
  static_web_app_id  = var.static_webapp_id
  domain_name     = "${var.c_name}.${var.dns_name}"
  validation_type = "dns-txt-token"
}

resource "azurerm_dns_txt_record" "dns_txt_record" {
  name                = "${var.c_name}.${var.dns_name}"
  zone_name           = var.dns_name
  resource_group_name = var.rg_name
  ttl                 = 300
  record {
    value = azurerm_static_web_app_custom_domain.appservice_domain.validation_token
  }
}
