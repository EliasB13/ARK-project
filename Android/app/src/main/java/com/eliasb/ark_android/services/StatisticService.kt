package com.eliasb.ark_android.services

import com.eliasb.ark_android.model.dtos.ReaderCountStat
import com.eliasb.ark_android.model.dtos.ReaderStatResponse
import io.reactivex.Observable
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Path
import retrofit2.http.Query

interface StatisticService {
    @GET("api/BusinessUsers/full-count-statistic")
    fun getFullCountStatistic(@Header("Authorization") token: String, @Query("lowerBound") lowerBound: String, @Query("upperBound") upperBound: String): Observable<List<ReaderCountStat>>

    @GET("api/BusinessUsers/reader-statistic/{readerId}")
    fun getReaderStatistic(@Header("Authorization") token: String, @Path("readerId") readerId: Int, @Query("lowerBound") lowerBound: String, @Query("upperBound") upperBound: String): Observable<Response<ReaderStatResponse>>

    @GET("api/BusinessUsers/full-statistic")
    fun getFullStatistic(@Header("Authorization") token: String, @Query("lowerBound") lowerBound: String, @Query("upperBound") upperBound: String): Observable<List<ReaderStatResponse>>


    companion object {
        fun create(): StatisticService {
            val retrofit = RetrofitBuilder.build()

            return retrofit.create(StatisticService::class.java)
        }
    }
}