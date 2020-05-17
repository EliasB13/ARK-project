package com.eliasb.ark_android.services

import android.annotation.SuppressLint
import android.app.IntentService
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.os.Build
import android.util.Log
import androidx.core.app.NotificationCompat
import androidx.navigation.NavDeepLinkBuilder
import com.eliasb.ark_android.MainActivity
import com.eliasb.ark_android.R
import com.eliasb.ark_android.entities.RestrictedObservation
import com.eliasb.ark_android.database.AppDatabase
import com.eliasb.ark_android.model.dtos.ObservationResponse
import io.reactivex.Observable
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import org.threeten.bp.LocalDateTime

class RestrictedObservationsService : IntentService("RestrictedObservationsService") {

    override fun onHandleIntent(intent: Intent?) {
        Log.i("Background service", "Service running")
        loadStatistic()
    }

    @SuppressLint("CheckResult")
    private fun loadStatistic() {
        val context = applicationContext
        val token = getToken(context)

        getSavedObservations(token)
    }

    @SuppressLint("CheckResult")
    private fun findNewRestrictedObservations(token: String, savedObservations: List<RestrictedObservation>) {
        val currDate = LocalDateTime.now()
        val minutesAgo = currDate.minusMinutes(10)

        val service = StatisticService.create()
        service.getFullStatistic("Bearer $token", minutesAgo.toString(), currDate.toString())
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe({
                result -> Log.i("Res", result.toString())
                if (result.isNotEmpty()) {
                    result.forEach { r ->
                        r.observations.forEach { o ->
                            if (!savedObservations.any { ro -> ro.id == o.id }) {
                                insertSavedObservation(o)

                                val title = "New restricted observation!"
                                val content = "${o.person.name} ${o.person.surname} was observed by reader: ${r.name}"

                                raiseNotification(title, content)
                            }
                        }
                    }
                }


            },
            {
                error ->
                Log.e("ERRORS", error.message)
            })
    }


    private fun getToken(context: Context): String {
        val prefService = PreferencesService
        prefService.create(context, context.getString(R.string.user_pref))
        return prefService.getPreference(context.getString(R.string.token))!!
    }

    @SuppressLint("CheckResult")
    private fun getSavedObservations(token: String): List<RestrictedObservation> {
        Observable.fromCallable {
            var db = AppDatabase.getAppDataBase(context = this)
            var dao = db?.dao()

            with(dao) {
                this?.getObservations()
            }
        }.subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe { result ->
                findNewRestrictedObservations(token, result!!)
            }
        return ArrayList()
    }

    private fun insertSavedObservation(observation: ObservationResponse) {
        Observable.fromCallable {
            var db = AppDatabase.getAppDataBase(context = this)
            var dao = db?.dao()

            var obs = RestrictedObservation(
                observation.id,
                observation.time,
                observation.isRestricted,
                observation.person.id
            )
            with(dao) {
                this?.insertObservation(obs)
            }
        }.subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe()
    }

    private fun createNotificationChannel(notificationManager: NotificationManager): String {
        val channelId = "com.elias.ark_android"

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {

            val channel = NotificationChannel(channelId, "Restricted Observations", NotificationManager.IMPORTANCE_HIGH)

            channel.description = "Notifications about new restricted observations"
            channel.enableLights(true)
            channel.lightColor = Color.RED
            channel.enableVibration(true)
            channel.vibrationPattern =
                longArrayOf(100, 200, 300, 400, 500, 400, 300, 200, 400)

            notificationManager.createNotificationChannel(channel)
        }
        return channelId
    }

    private fun createNotification(channelId: String, notificationManager: NotificationManager, title: String, content: String) {

        val pendingIntent = NavDeepLinkBuilder(this)
            .setComponentName(MainActivity::class.java)
            .setGraph(R.navigation.mobile_navigation)
            .setDestination(R.id.nav_restricted_obs)
            .createPendingIntent()

        val builder = NotificationCompat.Builder(this, channelId)
            .setSmallIcon(R.drawable.ic_reader)
            .setContentTitle(title)
            .setContentText(content)
            .setPriority(NotificationCompat.PRIORITY_HIGH)
            .setContentIntent(pendingIntent)
            .setAutoCancel(true)

        notificationManager.notify(101, builder.build())
    }

    private fun raiseNotification(title: String, content: String) {
        val notificationManager =
            getSystemService(
                Context.NOTIFICATION_SERVICE) as NotificationManager
        val id = createNotificationChannel(notificationManager)
        createNotification(id, notificationManager, title, content)
    }
}