package com.eliasb.ark_android.model.dtos

data class UpdateAccountRequest(
    val login: String,
    val email: String,
    val firstName: String,
    val lastName: String,
    val phone: String
)