package com.eliasb.ark_android.services

import com.eliasb.ark_android.model.dtos.ReaderCountStat
import io.reactivex.Observable
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Query
import java.util.*

interface StatisticService {
    @GET("api/BusinessUsers/full-count-statistic")
    fun getFullCountStatistic(@Header("Authorization") token: String, @Query("lowerBound") lowerBound: String, @Query("upperBound") upperBound: String): Observable<List<ReaderCountStat>>

    companion object {
        fun create(): StatisticService {
            val retrofit = RetrofitBuilder.build()

            return retrofit.create(StatisticService::class.java)
        }
    }
}