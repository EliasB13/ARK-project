package com.eliasb.ark_android.model.dtos

data class ObservationResponse(
    val id: Int,
    val time: String,
    val isRestricted: Boolean,
    val person: PersonCardResponse
)