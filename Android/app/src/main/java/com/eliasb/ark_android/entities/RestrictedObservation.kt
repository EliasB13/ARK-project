package com.eliasb.ark_android.entities

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity
data class RestrictedObservation(
    @PrimaryKey
    val id: Int,
    val time: String,
    val isRestricted: Boolean,
    val personId: Int
)