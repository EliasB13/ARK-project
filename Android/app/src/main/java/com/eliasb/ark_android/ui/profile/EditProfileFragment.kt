package com.eliasb.ark_android.ui.profile

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.eliasb.ark_android.R
import com.eliasb.ark_android.model.dtos.AccountDataResponse
import com.eliasb.ark_android.model.dtos.ErrorBody
import com.eliasb.ark_android.model.dtos.UpdateAccountRequest
import com.eliasb.ark_android.services.AccountService
import com.eliasb.ark_android.services.PreferencesService
import com.google.gson.Gson
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.fragment_edit_profile.*
import kotlinx.android.synthetic.main.fragment_edit_profile.login_top

class EditProfileFragment : Fragment() {

    private var accountData: AccountDataResponse? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        arguments?.let {
            accountData = it.getSerializable(getString(R.string.account_data)) as AccountDataResponse
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_edit_profile, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        loadUi()
    }

    private fun loadUi() {
        accountData?.let {

            login_top.text = it.login
            loginEdit.setText(it.login)
            emailEdit.setText(it.email)
            companyNameEdit.setText(it.companyName)
            addressEdit.setText(it.address)
            phoneEdit.setText(it.phone)
            descriptionEdit.setText(it.description)
            saveFab.setOnClickListener {
                updateAccountData()
            }
        }
    }

    private fun updateAccountData() {
        val updateAccRequest = UpdateAccountRequest(
            login = loginEdit.text.toString(),
            email = emailEdit.text.toString(),
            companyName = companyNameEdit.text.toString(),
            address = addressEdit.text.toString(),
            phone = phoneEdit.text.toString(),
            description = descriptionEdit.text.toString()
        )

        val prefService = PreferencesService
        prefService.create(context!!, getString(R.string.user_pref))
        val token = prefService.getPreference(getString(R.string.token))
        if (token != null) {
            spinner.visibility = View.VISIBLE
            val accService = AccountService.create()
            accService.updateProfile("Bearer $token", updateAccRequest)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(
                    {
                    response ->
                        Log.i("Update acc res", response.toString())
                        response.body()?.let {
                            Toast.makeText(context, getString(R.string.update_success), Toast.LENGTH_LONG).show()
                            findNavController().navigateUp()
                        }
                        response.errorBody()?.let {
                            val gson = Gson()
                            val json = gson.fromJson<ErrorBody>(it.charStream(), ErrorBody::class.java)
                            Toast.makeText(context, "Update profile error: ${json.message}", Toast.LENGTH_LONG).show()
                            Log.e("RefreshServiceItems err", json.message)
                        }
                    spinner.visibility = View.GONE
                    }, {
                            error ->
                        Log.d("Error", error.message)
                        spinner.visibility = View.GONE
                    })
        }
    }
}
