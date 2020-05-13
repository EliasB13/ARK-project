
package com.eliasb.ark_android.model.dtos

data class ReaderCountStat(
    val id: Int,
    val name: String,
    val description: String,
    val isEntrance: Boolean,
    val employeesObservationsCount: Int,
    val anonymObservationsCount: Int
)