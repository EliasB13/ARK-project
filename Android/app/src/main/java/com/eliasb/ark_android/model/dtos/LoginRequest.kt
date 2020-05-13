package com.eliasb.ark_android.model.dtos

import com.google.gson.annotations.SerializedName

data class LoginRequest (
    @SerializedName("login") var login: String,
    @SerializedName("password") var password: String
)