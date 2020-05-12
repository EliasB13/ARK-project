package com.eliasb.apz_android.services

import android.app.Activity
import android.content.Context
import android.content.Intent
import com.eliasb.apz_android.model.LoginRequest
import com.eliasb.apz_android.model.LoginResponse
import com.eliasb.ark_android.R
import com.eliasb.ark_android.model.dtos.AccountDataResponse
import com.eliasb.ark_android.model.dtos.RegisterRequest
import com.eliasb.ark_android.model.dtos.UpdateAccountRequest
import com.eliasb.ark_android.services.PreferencesService
import com.eliasb.ark_android.ui.LoginActivity
import io.reactivex.Observable
import retrofit2.Response
import retrofit2.http.*


interface AccountService {
    @POST("api/BusinessUsers/authenticate-business")
    fun login(@Body loginRequest: LoginRequest): Observable<Response<LoginResponse>>

    @POST("api/BusinessUsers/register-business")
    fun register(@Body registerRequest: RegisterRequest): Observable<Response<Void>>

    @GET("api/BusinessUsers/account-data")
    fun getProfile(@Header("Authorization")token: String): Observable<Response<AccountDataResponse>>

    @PUT("PrivateUsers")
    fun updateProfile(@Header("Authorization")token: String, @Body updateRequest: UpdateAccountRequest): Observable<Response<AccountDataResponse>>

    companion object {
        fun create(): AccountService {
            val retrofit = RetrofitBuilder.build()

            return retrofit.create(AccountService::class.java)
        }

        fun logout(context: Context) {
            val prefService = PreferencesService
            prefService.create(context, context.getString(R.string.user_pref))
            prefService.clearPreference(context.getString(R.string.user_pref))
            val activity = context as Activity
            val intent = Intent(context, LoginActivity::class.java)
            context.startActivity(intent)
            activity.finish()
        }
    }
}