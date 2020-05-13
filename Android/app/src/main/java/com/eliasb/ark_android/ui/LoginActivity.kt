package com.eliasb.ark_android.ui

import com.eliasb.ark_android.MainActivity
import com.eliasb.ark_android.R
import com.eliasb.ark_android.services.PreferencesService
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.*
import com.eliasb.ark_android.helpers.SpinnerHelper.Companion.dismissSpinner
import com.eliasb.ark_android.helpers.SpinnerHelper.Companion.showSpinner
import com.eliasb.ark_android.model.dtos.ErrorBody
import com.eliasb.ark_android.model.dtos.LoginRequest
import com.eliasb.ark_android.model.dtos.LoginResponse
import com.eliasb.ark_android.services.AccountService
import com.google.gson.Gson
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers

class LoginActivity : AppCompatActivity() {

    private lateinit var signInBtn: Button
    private lateinit var spinner: View


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        supportActionBar?.hide()

        val signUpRedirect = findViewById<TextView>(R.id.sign_up_redirect_btn)
        signUpRedirect.setOnClickListener{ goToSignUp() }

        signInBtn = findViewById(R.id.sign_in_btn)
        signInBtn.setOnClickListener{ signInClick() }

        spinner = findViewById(R.id.spinner)
    }

    private fun signInClick() {
        showSpinner(spinner, signInBtn)

        val loginEdit = findViewById<EditText>(R.id.loginEditText)
        val passEdit = findViewById<EditText>(R.id.passEditText)
        val accService = AccountService.create()
        accService.login(LoginRequest(loginEdit.text.toString(), passEdit.text.toString()))
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe({
                    loginResponse ->
                Log.d("Result", loginResponse.toString())
                loginResponse.body()?.let {
                    saveUser(it)
                    dismissSpinner(spinner, signInBtn)
                    goToMain()
                }
                loginResponse.errorBody()?.let {
                    val gson = Gson()
                    val json = gson.fromJson<ErrorBody>(it.charStream(), ErrorBody::class.java)
                    Log.w("ResponseError", json.message)
                    val codeStringId = resources.getIdentifier("code_" + json.code.toString(), "string", packageName)
                    Toast.makeText(this, getString(codeStringId), Toast.LENGTH_SHORT).show()
                    dismissSpinner(spinner, signInBtn)
                }
            },{ error ->
                Log.d("Error", error.message)
                dismissSpinner(spinner, signInBtn)
            })
    }

    private fun goToSignUp() {
        val intent = Intent(this, RegisterActivity::class.java)
        startActivity(intent)
    }

    private fun goToMain() {
        val intent = Intent(this, MainActivity::class.java)
        startActivity(intent)
        finish()
    }

    private fun saveUser(response: LoginResponse) {
        val prefService = PreferencesService
        prefService.create(this, getString(R.string.user_pref))
        prefService.savePreference(getString(R.string.token), response.token)
        prefService.savePreference(getString(R.string.id), response.id)
    }
}
