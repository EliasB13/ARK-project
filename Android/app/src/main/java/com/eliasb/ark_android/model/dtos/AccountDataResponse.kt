package com.eliasb.ark_android.model.dtos

import java.io.Serializable

data class AccountDataResponse(
    val id: Int,
    val login: String,
    val email: String,
    val firstName: String,
    val lastName: String,
    val phone: String,
    val photo: String
) : Serializable