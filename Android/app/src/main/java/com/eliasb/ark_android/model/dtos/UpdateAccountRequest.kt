package com.eliasb.ark_android.model.dtos

data class UpdateAccountRequest(
    val login: String,
    val email: String,
    val companyName: String,
    val address: String,
    val phone: String,
    val description: String
)