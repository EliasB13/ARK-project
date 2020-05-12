package com.eliasb.ark_android.model.dtos

data class RegisterRequest(
    val login: String,
    val companyName: String,
    val email: String,
    val password: String,
    val passwordConfirmation: String
)