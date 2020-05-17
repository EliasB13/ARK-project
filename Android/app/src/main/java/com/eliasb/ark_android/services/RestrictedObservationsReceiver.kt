package com.eliasb.ark_android.services

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent

class RestrictedObservationsReceiver : BroadcastReceiver() {

    override fun onReceive(context: Context?, intent: Intent?) {
        val intent = Intent(context, RestrictedObservationsService::class.java)
        context!!.startService(intent)
    }

    companion object {
        const val REQUEST_CODE = 12345
    }
}