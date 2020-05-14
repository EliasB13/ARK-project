package com.eliasb.ark_android.model.dtos

data class PersonCardResponse(
    val id: Int,
    val name: String,
    val surname: String,
    val isEmployee: Boolean,
    val photo: String
)