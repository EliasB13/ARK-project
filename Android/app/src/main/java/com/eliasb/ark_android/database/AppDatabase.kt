package com.eliasb.ark_android.database

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import com.eliasb.ark_android.entities.RestrictedObservation
import com.eliasb.ark_android.model.daos.RestrictedObservationsDAO


@Database(entities = [RestrictedObservation::class], version = 1)
abstract class AppDatabase : RoomDatabase() {
    abstract fun dao(): RestrictedObservationsDAO

    companion object {
        var INSTANCE: AppDatabase? = null

        fun getAppDataBase(context: Context): AppDatabase? {
            if (INSTANCE == null){
                synchronized(AppDatabase::class){
                    INSTANCE = Room.databaseBuilder(context.applicationContext, AppDatabase::class.java, "appDB").build()
                }
            }
            return INSTANCE
        }

        fun destroyDataBase(){
            INSTANCE = null
        }
    }
}