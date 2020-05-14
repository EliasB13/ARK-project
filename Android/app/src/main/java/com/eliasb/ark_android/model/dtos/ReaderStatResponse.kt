package com.eliasb.ark_android.model.dtos

data class ReaderStatResponse(
    val id: Int,
    val name: String,
    val description: String,
    val isEntrance: Boolean,
    val observations: ArrayList<ObservationResponse>
)