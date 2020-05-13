package com.eliasb.ark_android.ui

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.*
import com.eliasb.ark_android.R
import com.eliasb.ark_android.helpers.SpinnerHelper.Companion.dismissSpinner
import com.eliasb.ark_android.helpers.SpinnerHelper.Companion.showSpinner
import com.eliasb.ark_android.model.dtos.ErrorBody
import com.eliasb.ark_android.model.dtos.RegisterRequest
import com.eliasb.ark_android.services.AccountService
import com.google.gson.Gson
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers

class RegisterActivity : AppCompatActivity() {

    private lateinit var spinner: View
    private lateinit var signUpBtn: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        supportActionBar?.hide()

        val signInRedirect = findViewById<TextView>(R.id.sign_in_redirect_btn)
        signInRedirect.setOnClickListener{ goToSignIn() }

        signUpBtn = findViewById(R.id.sign_up_btn)
        signUpBtn.setOnClickListener{ signUpClick() }

        spinner = findViewById(R.id.spinner)
    }

    private fun signUpClick() {
        val request = RegisterRequest(
            findViewById<EditText>(R.id.loginEdit).text.toString(),
            findViewById<EditText>(R.id.companyNameEdit).text.toString(),
            findViewById<EditText>(R.id.emailEdit).text.toString(),
            findViewById<EditText>(R.id.passEdit).text.toString(),
            findViewById<EditText>(R.id.passConfirmEdit).text.toString()
        )

        showSpinner(spinner, signUpBtn)

        val accService = AccountService.create()
        accService.register(request)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe({ response ->
                response.errorBody()?.let {
                    Log.w("ResponseError", response.body().toString())
                    val gson = Gson()
                    val json = gson.fromJson<ErrorBody>(it.charStream(), ErrorBody::class.java)
                    val codeStringId = resources.getIdentifier(
                        "code_" + json.code.toString(),
                        "string",
                        packageName)
                    Toast.makeText(
                        this,
                        if (json.code != 0) getString(codeStringId)
                        else getString(R.string.validation_error),
                        Toast.LENGTH_SHORT).show()

                    dismissSpinner(spinner, signUpBtn)
                }
                if (response.errorBody() == null) {
                    Toast.makeText(
                        this,
                        getString(R.string.registerSuccess),
                        Toast.LENGTH_SHORT).show()
                    goToSignIn()

                    dismissSpinner(spinner, signUpBtn)
                }
            }, {
                error -> Log.e("Error", error.message)
                dismissSpinner(spinner, signUpBtn)
            })
    }

    private fun goToSignIn() {
        val intent = Intent(this, LoginActivity::class.java)
        startActivity(intent)
        finish()
    }
}
