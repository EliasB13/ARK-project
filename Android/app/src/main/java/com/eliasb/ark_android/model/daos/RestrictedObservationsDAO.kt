package com.eliasb.ark_android.model.daos

import androidx.room.*
import com.eliasb.ark_android.entities.RestrictedObservation

@Dao
interface RestrictedObservationsDAO {
    @Insert(onConflict = OnConflictStrategy.IGNORE)
    fun insertObservation(observation: RestrictedObservation)

    @Delete
    fun deleteObservation(observation: RestrictedObservation)

    @Query("SELECT * FROM RestrictedObservation WHERE id == :id")
    fun getObservationById(id: Int): RestrictedObservation

    @Query("SELECT * FROM RestrictedObservation")
    fun getObservations(): List<RestrictedObservation>
}