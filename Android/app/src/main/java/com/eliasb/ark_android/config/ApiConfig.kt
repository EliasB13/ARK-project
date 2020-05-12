package com.eliasb.ark_android.config

private const val BASE_URL = "http://10.0.2.2:50518/"

class ApiConfig {
    companion object {
        fun getBaseUrl(): String = BASE_URL
    }
}